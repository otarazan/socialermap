using System.Collections.Generic;
using Facebook.Web;
using Facebook;
using SocialerMapLib;

namespace socialermapfbapp.App_Code
{
    public class FacebookLayer
    {
        public static int RANKING_LIST_SIZE = 3;
        public readonly CanvasAuthorizer authorizer;
        public readonly FacebookClient fbClient;
        public readonly FacebookData facebookData;
        public bool isAccessTokenValid { get; private set; }
        public readonly FacebookUser User;

/*
        public FacebookLayer()
        {
            authorizer = new CanvasAuthorizer { Perms = "publish_stream" };
            if (authorizer.Authorize())
            {
                fbClient = new FacebookClient(CurrentSession.AccessToken);
                User = new FacebookUser();
                try
                {
                    var me = (IDictionary<string, object>) fbClient.Get("me");

                    User.FacebookId = (string) me["id"];
                    User.FacebookName = (string) me["first_name"];
                }
                catch
                {
                    isAccessTokenValid = false;
                    return;
                }
                isAccessTokenValid = true;
                IDictionary<string, object> friendsData = (IDictionary<string, object>) fbClient.Get("me/friends");
                facebookData = new FacebookData(User, friendsData);
                SortedFriends = facebookData.SortedFriends;
            }
        }
*/

        public FacebookLayer(CanvasAuthorizer auth)
        {
            this.authorizer = auth;
            if (this.authorizer.Authorize())
            {
                fbClient = new FacebookClient(CurrentSession.AccessToken);
                User = new FacebookUser();
                try
                {
                    var me = (IDictionary<string, object>) fbClient.Get("me");

                    User.FacebookId = (string) me["id"];
                    User.FacebookName = (string) me["first_name"];
                }
                catch
                {
                    isAccessTokenValid = false;
                    return;
                }
                isAccessTokenValid = true;
                IDictionary<string, object> friendsData = (IDictionary<string, object>) fbClient.Get("me/friends");
                facebookData = new FacebookData(User, (IList<object>)friendsData["data"]);

            }
        }

        /// <summary>
        /// Gets the current canvas facebook session.
        /// </summary>
        public static FacebookSession CurrentSession
        {
            get { return (new CanvasAuthorizer()).Session; }
        }

        public string LoadMap(string Date, string FacebookID, string locationID)
        {
            return facebookData.LoadMap(Date, FacebookID, locationID);
        }
    }
}