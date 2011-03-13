// ----------------------------------------------------------------------
// <copyright file="FacebookData.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace SocialerMapLib
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Text;

    /// <summary>
    /// Facebook Data Layer
    /// </summary>
    public class FacebookData
    {
        /// <summary>
        /// Location information
        /// </summary>
        private DataTable tLocations = new DataTable();

        /// <summary>
        /// User name and ID information for FDL
        /// </summary>
        private readonly FacebookUser user = new FacebookUser();

        /// <summary>
        /// Database access layer
        /// </summary>
        private readonly Database db = new Database();

        /// <summary>
        /// All friends are kept here.
        /// </summary>
        public readonly IList<FacebookUser> FriendsData=new List<FacebookUser>();

        /// <summary>
        /// sorted checkin Information
        /// </summary>
        public DataTable SortedFriends = new DataTable();

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookData"/> class.initializes User and Friends information and sorts friends
        /// </summary>
        /// <param name="user">User for friend inforamtion</param>
        /// <param name="friendsData">person's friends array</param>
        public FacebookData(FacebookUser user, IList<object> friendsData)
        {
            this.user = user;
            foreach (dynamic o in friendsData)
            {
                FacebookUser f = new FacebookUser { FacebookName = o.name, FacebookId = o.id };
                this.FriendsData.Add(f);
            }

            LoadDtLocations();
        }

        /// <summary>
        /// Loads the map with uesr and friend locations
        /// </summary>
        /// <param name="queryDate">location on that date </param>
        /// <param name="friendFid">friend id to load map</param>
        /// <param name="locationId">location id to load map</param>
        /// <returns>map script for page which can be called simply by loadmap()</returns>
        public string LoadMap(string queryDate, string friendFid, string locationId)
        {
            StringBuilder sb = new StringBuilder("<script language=javascript>function loadMap() {");
            sb.Append(String.Format("userFID={0};", user.FacebookId));
            try
            {
                DataRow location;
                if (locationId != null)
                {
                    location = tLocations.Select("locationID=" + locationId)[0];
                }
                else if (friendFid != null)
                {
                    location = tLocations.Select("ID=" + friendFid)[0];
                }
                else if (!string.IsNullOrEmpty(queryDate))
                {
// ReSharper disable UndocumentedThrownException
                    throw new Exception("not implemeneted yet");
// ReSharper restore UndocumentedThrownException
                    //queryDate = DateTime.Now.ToString("u").Replace("Z", string.Empty);
                    //DataRow location = dtLocations.Select("ID=" + friendFid)[0];
                    //sb = SocialerMap.InitializeMap(sb, location[3].ToString(), location[4].ToString(), 15);
                }
                else
                {
                    location = tLocations.Select("ID=" + this.user.FacebookId)[0];   
                }

                sb = SocialerMap.InitializeMap(sb, location[3].ToString(), location[4].ToString(), 15); 
            }
            catch
            {
                sb = SocialerMap.InitializeMap(sb, "41.00527", "28.97696", 2);
            }
            // load user locations)
            foreach (DataRow item in tLocations.Rows)
            {
                SocialerMapDbObject socialerMap = new SocialerMapDbObject
                {
                    UserId  =  item[1].ToString(),
                    LocationId = item[0].ToString(),
                    Date = item[2].ToString().Replace('.', '/'),
                    Latitude = item[3].ToString().Replace(',', '.').Trim(' '),
                    Longitude = item[4].ToString().Replace(',', '.').Trim(' '),
                    PostToWall = (bool)item[5],
                    Desc = item[6].ToString(),
                    Common = (bool)item[7]
                };

                sb = SocialerMap.AddMarker(sb, socialerMap.UserId == this.user.FacebookId ? true : false, socialerMap, GetfriendName(socialerMap.UserId));
            }

            sb.Append("}</script>");
            return sb.ToString();
        }

        /// <summary>
        /// Gets user's friend name using friend facebookid
        /// </summary>
        /// <param name="friendFid">Friend's facebook ID</param>
        /// <returns>friends name</returns>
        public string GetfriendName(string friendFid)
        {
            if (friendFid==user.FacebookId)
            {
                return user.FacebookName;
            }
            foreach (FacebookUser obj in this.FriendsData)
            {
                if (friendFid == obj.FacebookId)
                {
                    return obj.FacebookName;
                }
            }

            db.InsertLog(friendFid, EventType.ApplicationError, "getfriendName",
                               "User friend ID can not be converted to name!");
            return null;
        }

        /// <summary>
        /// sorts the user friends
        /// </summary>
        private void LoadDtLocations()
        {
            //DataTable friendsToLoadMap = this._db.Get(String.Format("Select * FROM tlocation where ID='{0}' and (Convert(varchar(10),date,126)=Convert(varchar(10),'{1}',126) or isCommon=1 or locationID={2})", friendObj["id"], queryDate, locationId));
            string socialerMapFriendCheckinSQL = "Select * FROM tLocation WHERE (ID=" + this.user.FacebookId;
            string socialerMapAllUserRankingSQL = "SELECT ID,COUNT(locationID) as checkinCounter FROM tLocation group by ID having (ID=" + user.FacebookId;

            StringBuilder sbsocialerMapFriendCheckinSQL = new StringBuilder();

            foreach (FacebookUser friendObj in this.FriendsData)
            {
                sbsocialerMapFriendCheckinSQL.Append(" or ID=" + friendObj.FacebookId);
            }

            SortedFriends = db.Get(socialerMapAllUserRankingSQL + sbsocialerMapFriendCheckinSQL + ") ORDER BY checkinCounter desc");
           tLocations = db.Get(socialerMapFriendCheckinSQL + sbsocialerMapFriendCheckinSQL + ")");
        }
    }
}