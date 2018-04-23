using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;

namespace Example.Models {
    public class DetailCustomBindingHandlers {
        static IQueryable Model { get { return NorthwindDataProvider.GetInvoices(); } }

        public static void GetDataRowCount(GridViewCustomBindingGetDataRowCountArgs e) {
            e.DataRowCount = Model
                .ApplyFilter(e.State.FilterExpression)
                .Count();
        }
        public static void GetData(GridViewCustomBindingGetDataArgs e) {
            e.Data = Model
                .ApplyFilter(e.State.FilterExpression)
                .ApplySorting(e.State.SortedColumns)
                .Skip(e.StartDataRowIndex)
                .Take(e.DataRowCount);
        }
    }
}