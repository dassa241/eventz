using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Dapper;


namespace Eventz.DAL.DB
{
    public class DbConnection
    {
        private static Dictionary<int, string> connections = new Dictionary<int, string>();
        public const int STORAGE_MOBILEAPP = 0;
        public static System.Data.IDbTransaction _transaction;
        public static System.Data.IDbConnection _conection;
        //aws sever

        //public static readonly string dbStorage = "Data Source=storge.cwuo5v9ypif8.us-east-1.rds.amazonaws.com;Initial Catalog=STORGEDB;User ID=admin;Password=tgCUNtnhyrcRnZY8uCkt;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        //gearhost server
        public static readonly string dbStorage = "Data Source=den1.mssql8.gear.host;Initial Catalog=Eventzlk;User ID=eventzlk;Password=Pv0q_0WKo!yB;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //public static readonly string dbStorage = "Data Source=LAPTOP-M3BJQKLN\\SQLEXPRESS;Initial Catalog = eventzlk; Integrated Security=True";


        //local server
        //public static readonly string dbStorage = "Data Source=JANA-DELL\\SQLEXPRESS;Initial Catalog=Storge;User ID=sa;Password=dev@123;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //Data Source = LAPTOP - M3BJQKLN\\SQLEXPRESS;Initial Catalog = AviorFleetQA; Integrated Security = True";



        static DbConnection()
        {
            connections.Add(STORAGE_MOBILEAPP, dbStorage);

        }

        public static SqlConnection GetOpenedConnection(int database)
        {
            SqlConnection connection = null;
            try
            {
                return connection = new SqlConnection(connections[database]);
            }
            finally
            {
                if (connection != null) { connection.Open(); }
            }
        }
        public static string GetOpenedConnectionString(int database)
        {
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connections[database]);
                return connection.ConnectionString;
            }
            finally
            {
                if (connection != null) { connection.Open(); }
            }
        }
        public Dapper.DynamicParameters para
        {
            get
            {
                return new DynamicParameters();
            }
        }

        public static System.Data.IDbConnection Conn(int database = STORAGE_MOBILEAPP)
        {
            return GetOpenedConnection(database);
            //return _conection;
        }

        public static void BeginTranaction()
        {
            _conection = new SqlConnection(connections[0]);
            _conection.Open();
            _transaction = _conection.BeginTransaction();
        }
        public static void CommitTranaction()
        {
            _transaction.Commit();
        }
        public static void RollbackTranaction()
        {
            _transaction.Rollback();
        }
    }
}
