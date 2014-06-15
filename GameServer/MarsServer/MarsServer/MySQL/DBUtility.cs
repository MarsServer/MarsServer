using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using System.Data;

namespace MarsServer
{
    public class DBUtility
    {
       public static void RunSQL(string strSqlText)
        {
            //定义MySqlConnection引用，并且实例化
            MySqlConnection msConn = new MySqlConnection(SQLConstants.MySQL_CONNECTION);
            try
            {
                //定义MySqlCommand引用
                MySqlCommand msComm = new MySqlCommand();

                //MySqlCommand使用MySqlConnection
                msComm.Connection = msConn;

                msComm.CommandText = strSqlText;

                msConn.Open();

                msComm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                msConn.Close();
            }
        }

        public static object RunSQLReturnObject(string strSqlText)
        {
            //定义MySqlConnection引用，并且实例化
            MySqlConnection msConn = new MySqlConnection(SQLConstants.MySQL_CONNECTION);
            try
            {
                //定义MySqlCommand引用
                MySqlCommand msComm = new MySqlCommand();

                //MySqlCommand使用MySqlConnection
                msComm.Connection = msConn;

                msComm.CommandText = strSqlText;

                msConn.Open();

                return msComm.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                msConn.Close();
            }
        }
        public static DataTable RunSQLReturnDataTable(string strSqlText)
        {
            //定义MySqlConnection引用，并且实例化
            MySqlConnection msConn = new MySqlConnection(SQLConstants.MySQL_CONNECTION);
            DataTable dt = new DataTable();
            try
            {
                msConn.Open();

                MySqlDataAdapter msda = new MySqlDataAdapter(strSqlText,msConn);
                msda.SelectCommand = new MySqlCommand(strSqlText, msConn);
                msda.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                msConn.Close();
            }
        }
    }
}
