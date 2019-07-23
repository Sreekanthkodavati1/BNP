using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Configuration;

namespace BnPBaseFramework.IO.FileFunctions_Common
{
    public static class DataReader
    {

        public static string OracleConnection = ConfigurationManager.AppSettings["OracleConn"];
        public static string PLSQLConnection = ConfigurationManager.AppSettings["PLSQLConn"];

        /// <summary>
        /// Used to create OracleDataReader by passing SELQuery in the parameter.
        /// </summary>
        /// <param name="SELQuery">SQL query to be executed through OracleDataReader</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader OracleDataReader(string SELQuery)
        {
            string conString = OracleConnection;
            OracleConnection OracleConn = new OracleConnection(conString);
            OracleConn.Open();
            OracleCommand OrclCMD = new OracleCommand(SELQuery, OracleConn);
            OrclCMD.CommandType = CommandType.Text;
            OracleDataReader ORCDR = OrclCMD.ExecuteReader();
            return ORCDR;

        }

        /// <summary>
        /// Used to create PLSQLDataReader by passing InsQuery in the parameter.
        /// </summary>
        /// <param name="InsQuery">SQL query to be executed through PLSQLDataReader</param>
        /// <returns>PLSQLDataReader</returns>
        public static void PLSQLDataReader(string InsQuery)
        {
            NpgsqlConnection PLSQLconn = new NpgsqlConnection(PLSQLConnection);
            PLSQLconn.Open();
            NpgsqlCommand NpgsqlCMD = new NpgsqlCommand(InsQuery, PLSQLconn);
            NpgsqlDataReader NpgsqlDR = NpgsqlCMD.ExecuteReader();
        }



        /// <summary>
        /// Used to create PLSQLDataReader by passing SELQuery in the parameter.
        /// </summary>
        /// <param name="SELQuery">SQL query to be executed through PLSQLDataReader</param>
        /// <returns>PLSQLDataReader</returns>
        public static NpgsqlDataReader PLSQLDataReader2(string SELQuery)
        {
            NpgsqlConnection PLSQLconn = new NpgsqlConnection(PLSQLConnection);
            PLSQLconn.Open();
            NpgsqlCommand NpgsqlCMD = new NpgsqlCommand(SELQuery, PLSQLconn);
            NpgsqlDataReader NpgsqlDR = NpgsqlCMD.ExecuteReader();
            return NpgsqlDR;
        }



    }
}
