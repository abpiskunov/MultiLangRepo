using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class QueryBuilder
    {
        private string _table;
        private readonly List<string> _selectColumns = new List<string>();
        private readonly List<string> _whereClauses = new List<string>();
        private string _orderBy;
        private int? _top;

        public QueryBuilder From(string tableName)
        {
            _table = tableName;
            return this;
        }

        public QueryBuilder Select(params string[] columns)
        {
            _selectColumns.AddRange(columns);
            return this;
        }

        public QueryBuilder Where(string clause)
        {
            _whereClauses.Add(clause);
            return this;
        }

        public QueryBuilder OrderBy(string column, bool descending = false)
        {
            _orderBy = "[" + column + "]" + (descending ? " DESC" : " ASC");
            return this;
        }

        public QueryBuilder Top(int count)
        {
            _top = count;
            return this;
        }

        public string Build()
        {
            var sb = new StringBuilder();
            sb.Append("SELECT ");

            if (_top.HasValue)
            {
                sb.AppendFormat("TOP {0} ", _top.Value);
            }

            if (_selectColumns.Count > 0)
            {
                sb.Append(string.Join(", ", _selectColumns));
            }
            else
            {
                sb.Append("*");
            }

            sb.AppendFormat(" FROM [{0}]", _table);

            if (_whereClauses.Count > 0)
            {
                sb.Append(" WHERE ");
                sb.Append(string.Join(" AND ", _whereClauses));
            }

            if (!string.IsNullOrEmpty(_orderBy))
            {
                sb.Append(" ORDER BY ");
                sb.Append(_orderBy);
            }

            return sb.ToString();
        }
    }
}
