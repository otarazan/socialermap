// ----------------------------------------------------------------------
// <copyright file="FacebookUser.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace SocialerMapLib
{
    /// <summary>
    /// Facebook User name and facebook ID information
    /// </summary>
    public class FacebookUser//:IComparable<FacebookUser>
    {
        /// <summary>
        /// Gets or sets facebook User Name
        /// </summary>
        public string FacebookName { get; set; }

        /// <summary>
        /// Gets or sets Facebook User ID
        /// </summary>
        public string FacebookId { get; set; }
    }
}