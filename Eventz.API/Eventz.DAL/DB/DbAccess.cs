using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Data;
using System.Linq;
using Dapper;

namespace Eventz.DAL.DB
{
   public class DbAccess: DbConnection
    {
        protected static int ExecuteUpdateDelete(string sp, List<System.Data.SqlClient.SqlParameter> parameters = null, int database = DbConnection.STORAGE_MOBILEAPP)
        {
            using (System.Data.SqlClient.SqlConnection connection = GetOpenedConnection(database))
            {
                System.Data.SqlClient.SqlCommand cmd = new SqlCommand(sp, connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                return cmd.ExecuteNonQuery();
            }
        }

        protected static System.Data.DataSet ExecuteReader(string sp, List<System.Data.SqlClient.SqlParameter> parameters = null, int database = DbConnection.STORAGE_MOBILEAPP)
        {
            using (System.Data.SqlClient.SqlConnection connection = DbConnection.GetOpenedConnection(database))
            {
                System.Data.SqlClient.SqlCommand cmd = new SqlCommand(sp, connection);
                cmd.CommandTimeout = 300;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }
                System.Data.DataSet data;
                new SqlDataAdapter(cmd).Fill(data = new DataSet());
                return data;
            }
        }

        protected static int ExecuteUpdateDelete(string sp, int database)
        {
            return ExecuteUpdateDelete(sp, null, database);
        }

        protected static DataSet ExecuteReader(string sp, int database)
        {
            return ExecuteReader(sp, null, database);
        }

        protected static DataTable ConvertListToDataTable<T>(string[] columns, List<T> items, Action<DataRowCollection, T> convert)
        {
            if (columns == null || columns.Length == 0 || items == null || convert == null)
            {
                throw new Exception("Invalid arguments");
            }
            DataTable dataTable = new DataTable();
            foreach (string column in columns)
            {
                dataTable.Columns.Add(column);
            }
            foreach (T item in items)
            {
                convert(dataTable.Rows, item);
            }
            return dataTable;
        }

        protected static List<T> ConvertDataTableToList<T>(DataTable table, Action<DataRow, List<T>> convert)
        {
            if (table == null || convert == null)
            {
                throw new Exception("Invalid arguments");
            }
            List<T> list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                convert(row, list);
            }
            return list;
        }

        protected static List<Dictionary<string, object>> ConvertDataTableToDictionaryList(DataTable table, Action<DataRow, List<Dictionary<string, object>>> convert)
        {
            if (table == null || convert == null)
            {
                throw new Exception("Invalid arguments");
            }
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                convert(row, list);
            }
            return list;
        }

        protected static List<Dictionary<string, object>> ConvertDataTableToDictionaryList(DataTable table)
        {
            if (table == null)
            {
                throw new Exception("Invalid arguments");
            }
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            foreach (DataRow row in table.Rows)
            {
                Dictionary<string, object> rowData = new Dictionary<string, object>();
                foreach (DataColumn column in table.Columns)
                {
                    rowData.Add(column.Caption, Convert.IsDBNull(row[column]) ? null : row[column]);
                }
                list.Add(rowData);
            }
            return list;
        }

        protected static List<Dictionary<string, string>> TransformDataTableToList(DataTable table)
        {
            List<Dictionary<string, string>> data = new List<Dictionary<string, string>>();
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    var obj = new Dictionary<string, string>();
                    foreach (DataColumn column in table.Columns)
                    {
                        obj.Add(column.Caption, row[column.Ordinal].ToString());
                    }
                    data.Add(obj);
                }
            }
            return data;
        }

        public class CryptoEncrypt
        {
            public static string Encrypt(string toEncrypt, bool useHashing)
            {
                byte[] keyArray;
                byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);


                string key = "Syed Moshiur Murshed";

                if (useHashing)
                {
                    System.Security.Cryptography.MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                    keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    hashmd5.Clear();
                }
                else
                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }
        /*Dapper Functions*/
        public static List<IDictionary<string, object>> TransformDataToDic(IEnumerable<dynamic> result)
        {
            return result.Select(x => (IDictionary<string, object>)x)?.ToList();
        }

        public static void ExecuteWithoutReturn(string procedureName, DynamicParameters param = null, int database = DbConnection.STORAGE_MOBILEAPP)
        {

            Conn(database).Execute(procedureName, param, commandType: CommandType.StoredProcedure);

        }
        public static T ExecuteTranactionWIthReturnObject<T>(string procedureName, /*DynamicParameters*/ object param = null, IDbTransaction trans = null)
        {
            return _transaction.Connection.Query<T>(procedureName, param, _transaction, commandType: CommandType.StoredProcedure).FirstOrDefault();

        }
        public static T ExecuteReturnScalar<T>(string procedureName, DynamicParameters param = null, int database = STORAGE_MOBILEAPP)
        {
            return (T)Convert.ChangeType(DbConnection.Conn().ExecuteScalar(procedureName, param, commandType: CommandType.StoredProcedure), typeof(T));

        }
        public static List<T> ReturnList<T>(string procedureName, DynamicParameters param = null, int database = DbConnection.STORAGE_MOBILEAPP)
        {
            return Conn(database).Query<T>(procedureName, param, commandType: CommandType.StoredProcedure).ToList();
        }
        public static T ReturnObject<T>(string procedureName, DynamicParameters param = null, int database = DbConnection.STORAGE_MOBILEAPP)
        {
            return Conn(database).Query<T>(procedureName, param, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }
        //public static SqlMapper.GridReader ReturnMultipleList(string procedureName, object /*DynamicParameters*/ param = null, int database = STORAGE_MOBILEAPP)
        //{
        //    return Conn(database).QueryMultiple(procedureName, param, commandType: CommandType.StoredProcedure);
        //}

        public static SqlMapper.GridReader ReturnMultipleList(string procedureName, DynamicParameters param = null)
        {
            return DbAccess.Conn().QueryMultiple(procedureName, param, commandType: CommandType.StoredProcedure);
        }
        public static object ReturnCoumnValue(string procedureName, DynamicParameters param = null, string columnName = null, int database = STORAGE_MOBILEAPP)
        {
            var result = Conn(database).Query(procedureName, param, commandType: CommandType.StoredProcedure).Select(x => (IDictionary<string, object>)x).ToList().FirstOrDefault();
            return result[columnName];
        }
        public static List<Dictionary<string, object>> ReturnDicList(string procedureName, DynamicParameters param = null, int database = STORAGE_MOBILEAPP)
        {
            var list = Conn(database).Query(procedureName, param, commandType: CommandType.StoredProcedure).Cast<IDictionary<string, object>>();
            //return list.Select(r => r.ToDictionary(d => d.Key, d => d.Value?.ToString()));
            return list.Select(r => r.ToDictionary(d => d.Key, d => d.Value)).ToList(); ;

        }

        public static List<IDictionary<string, object>> RetunSingleObject(string procedureName, DynamicParameters param = null, int database = STORAGE_MOBILEAPP)
        {
            return Conn(database).Query(procedureName, param, commandType: CommandType.StoredProcedure).FirstOrDefault();
        }


        public static int ExecuteTranaction(string procedureName, /*DynamicParameters*/ object param = null, IDbTransaction trans = null, int database = STORAGE_MOBILEAPP)
        {
            return _transaction.Connection.Execute(procedureName, param, _transaction, commandType: CommandType.StoredProcedure);

        }
        protected void ThrowSqlException(int number)
        {
            switch (number)
            {
                case 547:
                    throw new Exception("Cannot delete this record, this is associated with other record.");
                case 2601:
                case 2627:
                    throw new Exception("Cannot insert duplicate value.");
                case 50000:
                    throw new Exception("Record has been changed while you work, refersh the record and try again.");
                default:
                    throw new Exception("Process can not be completed.");
            }
        }
    }
}
