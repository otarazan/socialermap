// ----------------------------------------------------------------------
// <copyright file="Database.cs" company="tarazan.net">
//     Copyright statement. All right reserved
// </copyright>
//
// ------------------------------------------------------------------------
namespace SocialerMapLib
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    /// <summary>
    /// Database Access Layer
    /// </summary>
    public class Database
    {
        /// <summary>
        /// Database Connection Object
        /// </summary>
        private readonly SqlConnection con;

        /// <summary>
        /// connecyion obj
        /// </summary>
        private readonly SqlConnection conn;

        /// <summary>
        /// command obj
        /// </summary>
        private readonly SqlCommand cmd = new SqlCommand();

        /// <summary>
        /// Initializes a new instance of the Database class and get Connection information is retrived from web.config file
        /// </summary>
        public Database()
        {
            ConnectionStringSettingsCollection settings = ConfigurationManager.ConnectionStrings;
            if (settings != null)
            {
                this.con = new SqlConnection(settings["ConnectionInfo"].ConnectionString);
            }

            this.conn = this.con;
        }

        /// <summary>
        /// inserts a log to database for debugging and improvement purposes
        /// </summary>
        /// <param name="userId">which user has this problem</param>
        /// <param name="eventype">what kind of problem occured</param>
        /// <param name="methodName">method name of problem</param>
        /// <param name="errorLog">Exception message of problem</param>
        public void InsertLog(string userId, EventType eventype, string methodName, string errorLog)
        {
            if (userId==null)
            {
                userId = "0";
            }

            if (methodName==null)
            {
                methodName = "unknown";
            }

            if (errorLog==null)
            {
                errorLog = "unkown";
            }

            Exec("insert into tLog(ID,eventType,errorLog) values(" + userId + "," + (int)eventype + ",'" + methodName + ":" +
                    errorLog + "')");
        }

        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="command">command to execute</param>
        public void Exec(string command)
        {
            try
            {
                this.cmd.CommandText = command;
                this.cmd.Connection = this.conn;
                this.cmd.Connection.Close();
                this.cmd.Connection.Open();
                this.cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                InsertLog(null, EventType.DbError, "Exec", ex.Message);
            }

            this.cmd.Connection.Close();
        }

        /// <summary>
        /// execute command
        /// </summary>
        /// <param name="command">command to execute</param>
        /// <returns>result of execution</returns>
        public object ExecuteScalar(string command)
        {
            object obj = null;
            try
            {
                this.cmd.CommandText = command;
                this.cmd.Connection = this.conn;
                this.cmd.Connection.Close();
                this.cmd.Connection.Open();
                obj = this.cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                InsertLog(null, EventType.DbError, "ExecuteScalar", ex.Message);
            }

            this.cmd.Connection.Close();
            return obj;
        }

        /// <summary>
        /// executes the command
        /// </summary>
        /// <param name="command">command to execute</param>
        /// <returns>result of execution</returns>
        public DataTable Get(string command)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(command, this.conn))
                {
                    this.conn.Close();
                    this.conn.Open();
                    adapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                this.conn.Close();
                InsertLog(null, EventType.DbError, "ExecuteScalar", ex.Message);
            }

            this.conn.Close();
            return dt;
        }

/*
        /// <summary>
        /// executes Stored Procedure
        /// </summary>
        /// <param name="sp">procedure to execute</param>
        /// <param name="values">parameters of SP</param>
        /// <returns>result of execution</returns>
        public DataSet ExecSp(string sp, params object[] values)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlCommand command = new SqlCommand(sp, this._conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    this._conn.Open();
                    SqlCommandBuilder.DeriveParameters(command);
                    for (int i = 1; i <= values.Length; i++)
                    {
                        command.Parameters[i].Value = values[i - 1];
                    }

                    SqlDataAdapter adap = new SqlDataAdapter(command);
                    adap.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                InsertLog(null, EventType.DbError, "ExecSp", ex.Message);
            }
            finally
            {
                this._conn.Close();
            }

            return ds;
        }
*/

        /// <summary>
        /// inserts socialerobject to database
        /// </summary>
        /// <param name="obj">SocialerMap db obj to save DB </param>
        /// <returns>locationID of last inserted item</returns>
        public string InsertSocialerObj(SocialerMapDbObject obj)
        {
            if (string.IsNullOrEmpty(obj.Date))
            {
                obj.Date = DateTime.Now.ToString("u").Replace("Z", string.Empty);
            }

            InsertLog(obj.UserId, EventType.Inserted, "InsertSocialerObj", obj.ToString());

            return this.ExecuteScalar(String.Format("exec saveLocationProc {0},'{1}','{2}','{3}',{4},'{5}',{6}",
                                                 obj.UserId, obj.Date, obj.Latitude, obj.Longitude,
                                                 (obj.PostToWall ? 1 : 0), obj.Desc, (obj.Common ? 1 : 0))).ToString();
        }

        /// <summary>
        /// Delete SocialerMap object from the database using userID validation
        /// </summary>
        /// <param name="obj">SocialerMap object to delete</param>
        /// <param name="userId"> user validator ID</param>
        public void DeleteSocialerObj(SocialerMapDbObject obj, string userId)
        {
            if (obj.LocationId != null)
            {
                return;
            }

            DataTable userLocation = this.Get("select ID from tLocation where locationID= " + obj.LocationId);
            if (userLocation.Rows[0][0].ToString() == userId)
            {
                this.Exec("delete from tLocation where locationID=" + obj.LocationId);
                InsertLog(userId, EventType.Deleted, "DeleteSocialerObj", null);
            }
            else
            {
                InsertLog(userId, EventType.HackingAttempt, "DeleteSocialerObj", null);
            }
        }
    }
}