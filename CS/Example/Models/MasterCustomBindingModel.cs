using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;

namespace Example.Models {
    public class MasterCustomBindingHandlers {
        static IQueryable Model { get { return NorthwindDataProvider.GetCustomers(); } }

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e) {
            e.DataRowCount = Model.Count();
        }
        public static void GetData(GridViewCustomBindingGetDataArgs e) {
            e.Data = Model
                .ApplySorting(e.State.SortedColumns)
                .Skip(e.StartDataRowIndex)
                .Take(e.DataRowCount);
        }
    }

    public static class GridViewCustomOperationDataHelper {
        static ICriteriaToExpressionConverter CreateConverter(Type queryType) {
            return new CriteriaToEFExpressionConverter(queryType);
        }
        public static IQueryable Select(this IQueryable query, string fieldName) {
            return query.MakeSelect(CreateConverter(query.Provider.GetType()), new OperandProperty(fieldName));
        }
        public static IQueryable ApplySorting(this IQueryable query, IEnumerable<GridViewColumnState> sortedColumns) {
            foreach(GridViewColumnState column in sortedColumns)
                query = ApplySorting(query, column.FieldName, column.SortOrder);
            return query;
        }
        public static IQueryable ApplySorting(this IQueryable query, string fieldName, ColumnSortOrder order) {
            return query.MakeOrderBy(CreateConverter(query.Provider.GetType()), new ServerModeOrderDescriptor(new OperandProperty(fieldName), order == ColumnSortOrder.Descending));
        }
        public static IQueryable ApplyFilter(this IQueryable query, string filterExpression) {
            return query.AppendWhere(CreateConverter(query.Provider.GetType()), CriteriaOperator.Parse(filterExpression));
        }
    }
}