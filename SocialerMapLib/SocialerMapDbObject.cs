// ----------------------------------------------------------------------
// <copyright file="SocialerMapDBObject.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------

namespace SocialerMapLib
{
    /// <summary>
    /// SocialerMap database tLocation object item
    /// </summary>
    public class SocialerMapDbObject
    {
        /// <summary>
        /// Gets or sets socialerID ID
        /// </summary>
        public string LocationId { get; set; }

        /// <summary>
        /// Gets or sets facebook ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets date in uct format yyyy:mm:dd hh:mm:ss
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Gets or sets latitude info
        /// </summary>
        public string Latitude { get; set; }

        /// <summary>
        /// Gets or sets longitude info
        /// </summary>
        public string Longitude { get; set; }

        /// <summary>
        /// Gets or sets PostToWall
        /// </summary>
        public bool PostToWall { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        public string Desc { get; set; }

        /// <summary>
        ///  Gets or sets Common bit
        /// </summary>
        public bool Common { get; set; }

        /// <summary>
        /// returns all values
        /// </summary>
        /// <returns>all values</returns>
        public override string ToString()
        {
            return LocationId + UserId + Date + Latitude + Longitude + PostToWall + Desc + Common;
        }
    }
}