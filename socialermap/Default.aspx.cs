// ----------------------------------------------------------------------
// <copyright file="Default.aspx.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace CSASPNETWebsite
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Dynamic;
    using System.Web;
    using System.Web.UI.WebControls;
    using Facebook;
    using Facebook.Web;
    using SocialerMapLib;
    using App_Code;
    using System.Data;

    /// <content>
    /// Contains auto-generated functionality for the Customer class.
    /// </content>
    public partial class Default : System.Web.UI.Page
    {
        /// <summary>
        /// Loads page
        /// </summary>
        /// <param name="sender">sender information</param>
        /// <param name="e">event information</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("p3p",
                                                   "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

            Authorizer auth = new Authorizer
                                  {
                                      Perms = "publish_stream"
                                  };

            if (auth.Authorize())
            {
                FacebookLayer fb = new FacebookLayer(auth);
                if (Request.QueryString["action"] != null)
                {
                    this.LocationActions(Request.QueryString, fb);
                }

                this.ShowFacebookContent(fb);
            }
            else
            {
                Response.Redirect("~/Login.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery));
            }
        }

        /// <summary>
        /// perform Location actions
        /// </summary>
        /// <param name="queryString">querystring of request</param>
        /// <param name="fb">facebook Instance</param>
        private void LocationActions(NameValueCollection queryString, FacebookLayer fb)
        {
            Database db = new Database();
            SocialerMapDbObject obj = new SocialerMapDbObject();
            switch (queryString["action"])
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
                        parameters.link = "www.socialermap.com/Default.aspx?locationID=" + locationID;
                        parameters.name = queryString["description"];
                        parameters.picture = "http://www.socialermap.com/socialermapfbapp/Resources/icon.png";
                        parameters.caption = "Find where i am at " + obj.Date;
                        parameters.privacy = new
                                                 {
                                                     value = "ALL_FRIENDS",
                                                 };
                        fb.FbClient.Post("/me/feed", parameters);
                    }
                    break;
            }
        }

        /// <summary>
        /// Load page content
        /// </summary>
        /// <param name="fb">facebook instance</param>
        private void ShowFacebookContent(FacebookLayer fb)
        {
            if (!fb.IsAccessTokenValid)
            {
                var settings = ConfigurationManager.GetSection("facebookSettings");
                var current = settings as IFacebookApplication;
                HttpCookie cookie = Response.Cookies["fbs_" + current.AppId];
                cookie.Value = null;
                cookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Redirect("~/Login.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery));
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "loadMap",
                                                   fb.LoadMap(Request.QueryString["date"], Request.QueryString["fid"],
                                                              Request.QueryString["locationID"]));
                cmbFriends.DataSource = fb.facebookData.SortedFriends;
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