using Shared.RequestFeatures;
using Shared.Types;

namespace Repository.Extensions.Utility
{
    public static class QueryBuilder
    {
        public static string NormalizeLookUpFilter(IEnumerable< LookupFilterDTO> lookupFilter, bool fromSearchAll)
        {
            if (lookupFilter == null || lookupFilter?.Count() == 0)
                return string.Empty;

            var sqlWhere = new List<string>();
            foreach (var el in lookupFilter)
            {
                sqlWhere.Add($"{el.ColumnName} {LookupClauseToSqlClause(el.Operation, el.Value)}");
            }

            string sql = string.Join(fromSearchAll ? " OR " : " AND ", sqlWhere);
            return sql;
        }

        public static string NormalizeLookUpOrderBy(IEnumerable<LookupSortingDTO> lookupSorting)
        {
            if (lookupSorting == null || lookupSorting?.Count() == 0) return "";
            return lookupSorting.Select(el => $"{el.ColumnName} {el.Direction}").Aggregate((curr, next) => $"{curr}, {next}");
        }

        private static string LookupClauseToSqlClause(LookupFilterOperation lookupClause, object value)
        {
            object val = lookupClause switch
            {
                LookupFilterOperation.Contains => $"%{value}%",
                LookupFilterOperation.StartsWith => $"{value}%",
                LookupFilterOperation.EndsWith => $"%{value}",
                _ => value
            };
            string where = lookupClause switch
            {
                LookupFilterOperation.Contains => $"LIKE '{val}'",
                LookupFilterOperation.StartsWith => $"LIKE '{val}'",
                LookupFilterOperation.EndsWith => $"LIKE '{val}'",
                LookupFilterOperation.Equals => $"= '{val}'",
                LookupFilterOperation.Less => $"< '{val}'",
                LookupFilterOperation.LessOrEquals => $"<= '{val}'",
                LookupFilterOperation.More => $"> '{val}'",
                LookupFilterOperation.MoreOrEquals => $">= '{val}'",
                LookupFilterOperation.RangeDate => $"BETWEEN '{val.ToString().Split('|')[0]}' AND '{val.ToString().Split('|')[1]}'",
                LookupFilterOperation.NotEqual => $"<> '{val}'",
                _ => throw new NotImplementedException("Filter operation not implemented")
            };
            return where;
        }
    }
}