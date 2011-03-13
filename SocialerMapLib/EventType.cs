// ----------------------------------------------------------------------
// <copyright file="EventType.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace SocialerMapLib
{
    /// <summary>
    /// Event Types in database
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// When any data deleted from database
        /// </summary>
        Deleted,

        /// <summary>
        /// when data is inserted to database
        /// </summary>
        Inserted,

        /// <summary>
        /// When any error occured
        /// </summary>
        ApplicationError,

        /// <summary>
        /// When an event occured which shouldn't be unless hacking attempt
        /// </summary>
        HackingAttempt,

        /// <summary>
        /// DataBase Error occured
        /// </summary>
        DbError
    }
}