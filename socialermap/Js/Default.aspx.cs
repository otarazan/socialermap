using System;
using System.Web;
using System.Web.UI.WebControls;
using Facebook.Web;
using Facebook;
using System.Configuration;
using System.Collections.Specialized;
using SocialerMapLib;
using System.Collections.Generic;
using System.Dynamic;

namespace CSASPNETWebsite
{
    public partial class Default : System.Web.UI.Page
    {
        FacebookLayer fb;
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

            Authorizer auth = new Authorizer { Perms = "publish_stream" };

            
            if (auth.Authorize())
            {
                FacebookLayer fb = new FacebookLayer(auth);
                if (Request.QueryString["action"] != null)
                {
                    LocationActions(Request.QueryString, fb);
                }
                ShowFacebookContent(fb);
            }
            else
            {
                Response.Redirect("~/Login.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery));
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
                Response.Redirect("~/Login.aspx?returnUrl=" + HttpUtility.UrlEncode(Request.Url.PathAndQuery));
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "loadMap", fb.LoadMap(Request.QueryString["date"], Request.QueryString["fid"]));
                cmbFriends.DataSource = fb.SortedFriends;
                cmbFriends.DataTextField = "Key";
                cmbFriends.DataValueField = "Value";
                cmbFriends.DataBind();
                cmbFriends.Items.Insert(0, new ListItem("All Friends", null));
            }
        }

        private void LocationActions(NameValueCollection QueryString,FacebookLayer fb)
        {
            Database db = new Database();
            SocialerMapDBObject obj = new SocialerMapDBObject();

            if (QueryString["action"] == "delete")
            {
                obj.LocationID = Request.QueryString["locationID"];
                db.deleteSocialerObj(obj);
            }
            else if (QueryString["action"] == "add")
            {
                obj.Fid = fb.user.facebookID;
                obj.Date = Request.QueryString["Date"];
                obj.Latitude = Request.QueryString["Latitude"];
                obj.Longitude = Request.QueryString["Longitude"];
                obj.postToWall = Convert.ToBoolean(Request.QueryString["postwall"]);
                obj.Desc = Request.QueryString["description"];
                db.InsertSocialerObj(obj);
                if (obj.postToWall)
                {
                    dynamic parameters = new ExpandoObject();
                    parameters.message = "Checked in via SocialerMap";
                    parameters.link = "www.socialermap.com/default.aspx?fid=" + fb.user.facebookID;
                    parameters.name = QueryString["description"];
                    parameters.privacy = new
                    {
                        value = "ALL_FRIENDS",
                    };
                    fb.fbClient.Post("/me/feed", parameters);
                }
            }
        }

    }
}