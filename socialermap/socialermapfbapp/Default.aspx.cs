using System;
using System.Web;
using System.Web.UI.WebControls;
using Facebook.Web;
using Facebook;
using socialermapfbapp.App_Code;
using SocialerMapLib;
using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;
using System.Data;

namespace socialermapfbapp
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var auth = new CanvasAuthorizer{Perms = "publish_stream"};

            FacebookLayer fb = new FacebookLayer(auth);
            if (auth.Authorize())
            {
                if (Request.QueryString["action"] != null)
                {
                    LocationActions(Request.QueryString, fb);
                }
                ShowFacebookContent(fb);
            }
        }

        private void LocationActions(NameValueCollection QueryString, FacebookLayer fb)
        {
            Database db = new Database();
            SocialerMapDbObject obj = new SocialerMapDbObject();
            switch (QueryString["action"])
            {
                case "delete":
                    obj.LocationId = Request.QueryString["locationID"];
                    db.DeleteSocialerObj(obj, fb.User.FacebookId);
                    break;
                case "add":
                    obj.UserId = fb.User.FacebookId;
                    obj.Date = Request.QueryString["Date"];
                    obj.Latitude = Request.QueryString["Latitude"];
                    obj.Longitude = Request.QueryString["Longitude"];
                    obj.PostToWall = Convert.ToBoolean(Request.QueryString["postwall"]);
                    obj.Desc = Request.QueryString["description"];
                    obj.Common = Convert.ToBoolean(Request.QueryString["common"]);
                    string locationID = db.InsertSocialerObj(obj);
                    if (obj.PostToWall)
                    {
                        dynamic parameters = new ExpandoObject();
                        parameters.message = "Checked in via SocialerMap";
                        parameters.link = "http://apps.facebook.com/socialermapfbapp/Default.aspx?locationID=" +
                                          locationID;
                        parameters.name = QueryString["description"];
                        parameters.picture = "http://www.socialermap.com/socialermapfbapp/Resources/icon.png";
                        parameters.caption = "Find where i am at " + obj.Date;
                        parameters.privacy = new
                            {
                            value = "ALL_FRIENDS",
                        };
                        fb.fbClient.Post("/me/feed", parameters);
                    }
                    break;
            }
        }

        private void ShowFacebookContent(FacebookLayer fb)
        {
            if (!fb.isAccessTokenValid)
            {
                var settings = ConfigurationManager.GetSection("facebookSettings");
                var current = settings as IFacebookApplication;
                HttpCookie cookie = Response.Cookies["fbs_" + current.AppId];
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "loadMap",
                                                   fb.LoadMap(Request.QueryString["date"], Request.QueryString["fid"],
                                                              Request.QueryString["locationID"]));
                cmbFriends.DataSource = fb.facebookData.FriendsData;
                cmbFriends.DataValueField = "FacebookId";
                cmbFriends.DataTextField = "FacebookName";
                cmbFriends.DataBind();
                cmbFriends.Items.Insert(0, new ListItem("All Friends", null));
                loadStatistics(fb);
            }
        }

        private void loadStatistics(FacebookLayer fb)
        {
            // socialerMapFriends User/Facebook User
            lblFriendCount.Text = string.Format("{0}/{1}", fb.facebookData.SortedFriends.Rows.Count, fb.facebookData.FriendsData.Count);
            int i = 1;
            foreach (DataRow obj in fb.facebookData.SortedFriends.Rows)
            {
                lblRanking.Text += "<tr><td>" + i + "." + fb.facebookData.GetfriendName(obj[0].ToString()) + "</td><td>" + obj[1] + "</td></tr>";
                if (i++ > FacebookLayer.RANKING_LIST_SIZE)
                {
                    break;
                }
            }
        }
    }
}