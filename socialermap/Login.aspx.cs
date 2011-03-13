// ----------------------------------------------------------------------
// <copyright file="Login.aspx.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace CSASPNETWebsite
{
    using System;
    using System.Web;
    using Facebook.Web;

    /// <content>
    /// Contains auto-generated functionality for the Login Page class.
    /// </content>
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// loads LoginPage if user is aloready logged in redirect to default.aspx
        /// </summary>
        /// <param name="sender">sender information</param>
        /// <param name="e">event information</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            var authorizer = new Authorizer {Perms = "publish_stream"};

            if (authorizer.IsAuthorized())
            {
                Response.Redirect(HttpUtility.UrlDecode(Request.QueryString["returnUrl"] ?? "/"));
            }
        }
    }
}