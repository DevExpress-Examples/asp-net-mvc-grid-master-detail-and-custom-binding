using System.Linq;
using System.Web;

namespace Example.Models {
    public static class NorthwindDataProvider {
        const string NorthwindDataContextKey = "DXNorthwindDataContext";

        public static NorthwindDataContext DB {
            get {
                if (HttpContext.Current.Items[NorthwindDataContextKey] == null)
                    HttpContext.Current.Items[NorthwindDataContextKey] = new NorthwindDataContext();
                return (NorthwindDataContext)HttpContext.Current.Items[NorthwindDataContextKey];
            }
        }

        public static IQueryable<Customer> GetCustomers() {
            return DB.Customers;
        }
        public static IQueryable<Invoice> GetInvoices() {
            return DB.Invoices;
        }
    }
}