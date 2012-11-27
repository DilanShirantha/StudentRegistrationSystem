using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace StudentRegistrationSystem
{
    public class DataBaseConnect
    {
        #region Fields

        /// <summary>
        /// Use to connect to database using UserName and Password Credentials
        /// </summary>
        //private const string ConnectString = "Data Source=SOBE-PC;Initial Catalog=master;User ID=@@;Password=@@@@@";

        /// <summary>
        /// Use to connect to database using windows login
        /// </summary>
        private const string ConnectString = "Data Source=SOBE-PC;Initial Catalog=master;Trusted_Connection=yes";

        #endregion Fields


        #region Public methods

        /// <summary>
        /// Save New Student Details
        /// </summary>
        /// <param name="query">Stored Procedure Name</param>
        /// <param name="studentDetailCollection">Collection of Student Details</param>   
        public void RunSpSaveNewStudentDetails(string query, string studentDetailCollection)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(ConnectString);
                SqlCommand comm = new SqlCommand(query, conn);

                comm.CommandType = CommandType.StoredProcedure;

                comm.Parameters.AddWithValue("@StudentDetailCollection", studentDetailCollection);

                conn.Open();
                reader = comm.ExecuteReader();
                reader.Close();
            }
            catch (NullReferenceException nullReferenceException)
            {
                // Need to log exception to a file. Not Implemented
                throw new Exception(nullReferenceException.Message);
            }
            catch (Exception exception)
            {
                // Need to log exception to a file. Not Implemented
                throw new Exception(exception.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();

                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

        }


        /// <summary>
        /// Get New StudentID
        /// </summary>
        /// <param name="query">Stored Procedure Name</param>
        /// <returns>Current Student ID</returns>
        public int RunSpGetNewStudentIdNumber(string query)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(ConnectString);
                SqlCommand comm = new SqlCommand(query, conn);

                comm.CommandType = CommandType.StoredProcedure;

                // Return value as parameter
                SqlParameter returnValue = new SqlParameter("returnVal", SqlDbType.Int);
                returnValue.Direction = ParameterDirection.ReturnValue;
                comm.Parameters.Add(returnValue);

                conn.Open();
                reader = comm.ExecuteReader();
                reader.Close();

                return Convert.ToInt32(returnValue.Value);
            }
            catch (NullReferenceException nullReferenceException)
            {
                // Need to log exception to a file. Not Implemented
                throw new Exception(nullReferenceException.Message);
            }
            catch (Exception exception)
            {
                // Need to log exception to a file. Not Implemented
                throw new Exception(exception.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();

                if (reader != null && !reader.IsClosed)
                    reader.Close();
            }

        }

        #endregion Public methods

    }
}
