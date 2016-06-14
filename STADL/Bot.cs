using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace STADL
{
   
    public class Bot
    {
        // all  private variables have underscores
        private SqlConnection _sqlConn;
        private SqlCommand _sqlCommand;
        private string _connStr;
        public Decimal zContractContacts_ID;  // Array Value {0}
        public String zContractContacts_Contract; // Array Value {1}
        public String zContractContacts_Account; // Array Value {2}
        public String zContractContacts_AccountName; // Array Value {3}
        // Skipping ContactID from zContractContacts Table 
        public String zContractContacts_ContactFullName; // Array Value {5}


        /// Summary
        /// Setting the db connection string in app.config and then referencing it here
        /// This will also be in app.config on the client side.
        public void DBConnection()
        {
            _connStr = ConfigurationManager.ConnectionStrings["SYSPRO_SQL_SERVER"].ConnectionString;
            // Adding a call to DBOpenConnection() to ensure we are connected
            DBOpenConnection();
        }

        /// Summary
        /// Open Database Connection
        public bool DBOpenConnection()
        {
            try
            {
                _sqlConn = new SqlConnection(_connStr);
                _sqlConn.Open();               
                return true;                
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        //overload method
        public bool DBOpenConnection(string connection)
        {
            try
            {
                _sqlConn = new SqlConnection(connection);
                _sqlConn.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// Summary
        /// Close Database Connection
        public bool DBCloseConnection()
        {
            try
            {
                _sqlConn = new SqlConnection(_connStr);
                if (_sqlConn.State != ConnectionState.Closed)
                    _sqlConn.Close();
                return true;                
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// Summary
        /// Select Function
        /// Takes in Sql command as a parameter. 
        /// Reurns a SqlReader Object is successful, otherwise returns null.
        public SqlDataReader Select(string sql)
        {
            try
            {
                SqlDataReader sqlReader;                
                _connStr = ConfigurationManager.ConnectionStrings["SYSPRO_SQL_SERVER"].ConnectionString;
                DBOpenConnection();
                _sqlCommand = new SqlCommand(sql, _sqlConn);
                sqlReader = _sqlCommand.ExecuteReader();

                if(sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {                        
                        zContractContacts_ID = sqlReader.GetDecimal(0);
                        zContractContacts_Contract = sqlReader.GetString(1);
                        zContractContacts_Account = sqlReader.GetString(2);
                        zContractContacts_AccountName = sqlReader.GetString(3);                        
                        // Skipping ContactID
                        zContractContacts_ContactFullName = sqlReader.GetString(5);
                        Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}", sqlReader.GetDecimal(0), sqlReader.GetString(1), sqlReader.GetString(2), sqlReader.GetString(3), sqlReader.GetGuid(4), sqlReader.GetString(5));                       
                    }
                }
                else
                {
                    Console.WriteLine("No Rows Found");
                }
                

                return sqlReader;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// Summary
        /// Insert Function
        /// Takes in Sql command as a parameter.
        /// Returns "Sucessfully Saved" if successful, otherwise returns exception.
        public string Insert(string sql)
        {
            try
            {
                if (!DBOpenConnection())
                    return "Database Connection Error.";
                _sqlCommand = new SqlCommand(sql, _sqlConn);
                _sqlCommand.ExecuteNonQuery();
                return "Successfully Saved";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        /// Summary
        /// Update Function
        /// Takes in Sql command as a parameter.
        /// Returns "Sucessfully Updated" if successful, otherwise returns exception. 
        public string Update(string sql)
        {
            try
            {
                if (!DBOpenConnection())
                    return "Database Connection Error.";
                _sqlCommand = new SqlCommand(sql, _sqlConn);
                _sqlCommand.ExecuteNonQuery();
                return "Successfully Updated";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
