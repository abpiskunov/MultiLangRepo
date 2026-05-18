using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DataAccess
{
    public class GenericRepository<T> where T : class, new()
    {
        private readonly SqlConnectionManager _connectionManager;
        private readonly string _tableName;

        public GenericRepository(SqlConnectionManager connectionManager, string tableName)
        {
            _connectionManager = connectionManager ?? throw new ArgumentNullException(nameof(connectionManager));
            _tableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        }

        public List<T> GetAll()
        {
            string sql = "SELECT * FROM [" + _tableName + "]";
            var dataTable = _connectionManager.ExecuteQuery(sql);
            return MapDataTableToList(dataTable);
        }

        public T GetById(int id)
        {
            string sql = "SELECT * FROM [" + _tableName + "] WHERE Id = @Id";
            var param = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            var dataTable = _connectionManager.ExecuteQuery(sql, param);

            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            return MapDataRowToObject(dataTable.Rows[0]);
        }

        public int Insert(T entity)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var columns = new List<string>();
            var paramNames = new List<string>();
            var parameters = new List<SqlParameter>();

            foreach (var prop in properties)
            {
                if (prop.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var value = prop.GetValue(entity);
                if (value != null)
                {
                    columns.Add("[" + prop.Name + "]");
                    paramNames.Add("@" + prop.Name);
                    parameters.Add(new SqlParameter("@" + prop.Name, value));
                }
            }

            string sql = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2}); SELECT SCOPE_IDENTITY();",
                _tableName,
                string.Join(", ", columns),
                string.Join(", ", paramNames));

            var result = _connectionManager.ExecuteScalar(sql, parameters.ToArray());
            return Convert.ToInt32(result);
        }

        public void Delete(int id)
        {
            string sql = "DELETE FROM [" + _tableName + "] WHERE Id = @Id";
            var param = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            _connectionManager.ExecuteNonQuery(sql, param);
        }

        private List<T> MapDataTableToList(DataTable dataTable)
        {
            var list = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                list.Add(MapDataRowToObject(row));
            }
            return list;
        }

        private T MapDataRowToObject(DataRow row)
        {
            var obj = new T();
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                if (row.Table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value)
                {
                    prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                }
            }

            return obj;
        }
    }
}
