using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess
{
    public class SqlConnectionManager : IDisposable
    {
        private SqlConnection _connection;
        private readonly string _connectionString;

        public SqlConnectionManager(string connectionStringName)
        {
            _connectionString = ConfigurationManager.ConnectionStrings[connectionStringName]?.ConnectionString
                ?? throw new ConfigurationErrorsException("Connection string '" + connectionStringName + "' not found.");
        }

        public SqlConnection GetConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            return _connection;
        }

        public DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
        {
            using (var command = new SqlCommand(sql, GetConnection()))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                var dataTable = new DataTable();
                using (var adapter = new SqlDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var command = new SqlCommand(sql, GetConnection()))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return command.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var command = new SqlCommand(sql, GetConnection()))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }
                return command.ExecuteScalar();
            }
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                if (_connection.State != ConnectionState.Closed)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
