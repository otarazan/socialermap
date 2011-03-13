// ----------------------------------------------------------------------
// <copyright file="FacebookLayer.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace CSASPNETWebsite.App_Code
{
    using System.Collections.Generic;
    using Facebook;
    using Facebook.Web;
    using SocialerMapLib;

    /// <summary>
    /// Facebook Layer Class to interract with Facebook Data Layer
    /// </summary>
    public class FacebookLayer
    {
        public static int RANKING_LIST_SIZE = 3;
        /// <summary>
        /// Facebook Data Layer instance
        /// </summary>
        public readonly FacebookData facebookData;

/*
        /// <summary>
        /// Initializes a new instance of the FacebookLayer class
        /// </summary>
        public FacebookLayer()
        {
            this.Auth = new Authorizer();

            if (this.Auth.Authorize())
            {
                this.FbClient = new FacebookClient(CurrentSession.AccessToken);
                this.User = new FacebookUser();
                try
                {
                    var me = (IDictionary<string, object>) this.FbClient.Get("me");

                    this.User.FacebookId = (string) me["id"];
                    this.User.FacebookName = (string) me["first_name"];
                }
                catch
                {
                    this.IsAccessTokenValid = false;
                    return;
                }

                this.IsAccessTokenValid = true;
                IDictionary<string, object> friendsData = (IDictionary<string, object>) this.FbClient.Get("me/friends");
                this.facebookData = new FacebookData(this.User, friendsData);
                this.SortedFriends = this.facebookData.SortedFriends;
            }
        }
*/

        /// <summary>
        /// Initializes a new instance of the FacebookLayer class using authorization
        /// </summary>
        /// <param name="auth">authorization instance</param>
        public FacebookLayer(Authorizer auth)
        {
            this.Auth = auth;

            if (auth.Authorize())
            {
                this.FbClient = new FacebookClient(CurrentSession.AccessToken);
                this.User = new FacebookUser();
                try
                {
                    var me = (IDictionary<string, object>) this.FbClient.Get("me");

                    this.User.FacebookId = (string) me["id"];
                    this.User.FacebookName = (string) me["first_name"];
                }
                catch
                {
                    this.IsAccessTokenValid = false;
                    return;
                }

                this.IsAccessTokenValid = true;
                IDictionary<string, object> friendsData = (IDictionary<string, object>)FbClient.Get("me/friends");
                facebookData = new FacebookData(User, (IList<object>)friendsData["data"]);
            }
        }

        /// <summary>
        /// Gets the current canvas facebook session.
        /// </summary>
        public static FacebookSession CurrentSession
        {
            get { return (new Authorizer()).Session; }
        }

        /// <summary>
        /// Gets Checks if access token is valid
        /// </summary>
        public bool IsAccessTokenValid { get; private set; }

        /// <summary>
        /// Gets or Sets CanvasAuthorizer
        /// </summary>
        public Authorizer Auth { private get; set; }

        /// <summary>
        /// Gets or Sets FacebookClient info
        /// </summary>
        public FacebookClient FbClient { get; set; }

        /// <summary>
        /// Gets or Sets facebok User Info
        /// </summary>
        public FacebookUser User { get; set; }

        /// <summary>
        /// Loads Map according to parameters
        /// </summary>
        /// <param name="date">load Map for that date</param>
        /// <param name="facebookID">load Map for that facebookUser</param>
        /// <param name="locationID">load Map for location</param>
        /// <returns>javascript to load Map</returns>
        public string LoadMap(string date, string facebookID, string locationID)
        {
            return this.facebookData.LoadMap(date, facebookID, locationID);
        }
    }
}