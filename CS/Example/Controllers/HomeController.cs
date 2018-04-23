using System.Web.Mvc;
using Example.Models;
using DevExpress.Web.Mvc;
using DevExpress.Data.Filtering;

namespace Example.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();	
        }
		public ActionResult MasterGridViewPartial() {
            var viewModel = GridViewExtension.GetViewModel("masterGrid");
            if(viewModel == null)
                viewModel = CreateMasterGridViewModel();
            return MasterGridActionCore(viewModel);
        }
        public ActionResult MasterGridViewSortingAction(GridViewColumnState column, bool reset) {
            var viewModel = GridViewExtension.GetViewModel("masterGrid");
            viewModel.SortBy(column, reset);
            return MasterGridActionCore(viewModel);
        }
        public ActionResult MasterGridViewPagingAction(GridViewPagerState pager) {
            var viewModel = GridViewExtension.GetViewModel("masterGrid");
            viewModel.Pager.Assign(pager);
            return MasterGridActionCore(viewModel);
        }
        public ActionResult MasterGridActionCore(GridViewModel gridViewModel) {
            gridViewModel.ProcessCustomBinding(
                MasterCustomBindingHandlers.GetDataRowCount,
                MasterCustomBindingHandlers.GetData
            );
            return PartialView("MasterGridViewPartial", gridViewModel);
        }

        public ActionResult DetailGridViewPartial(string customerID) {
            var viewModel = GridViewExtension.GetViewModel("detailGrid" + customerID);
            if(viewModel == null)
                viewModel = CreateDetailGridViewModel(customerID);
            return DetailGridActionCore(viewModel, customerID);
        }
        public ActionResult DetailGridViewPagingAction(GridViewPagerState pager, string customerID) {
            var viewModel = GridViewExtension.GetViewModel("detailGrid" + customerID);
            viewModel.Pager.Assign(pager);
            return DetailGridActionCore(viewModel, customerID);
        }
        public ActionResult DetailGridViewSortingAction(GridViewColumnState column, bool reset, string customerID) {
            var viewModel = GridViewExtension.GetViewModel("detailGrid" + customerID);
            viewModel.SortBy(column, reset);
            return DetailGridActionCore(viewModel, customerID);
        }
        public ActionResult DetailGridActionCore(GridViewModel gridViewModel, string customerID) {
            gridViewModel.ProcessCustomBinding(
                DetailCustomBindingHandlers.GetDataRowCount,
                DetailCustomBindingHandlers.GetData
            );
            ViewData["CustomerID"] = customerID;
            return PartialView("DetailGridViewPartial", gridViewModel);
        }

        static GridViewModel CreateMasterGridViewModel() {
            var viewModel = new GridViewModel();
            viewModel.KeyFieldName = "CustomerID";
            viewModel.Columns.Add("ContactName");
            viewModel.Columns.Add("CompanyName");
            viewModel.Columns.Add("City");
            viewModel.Columns.Add("Country");
            return viewModel;
        }
        static GridViewModel CreateDetailGridViewModel(string customerID) {
            var viewModel = new GridViewModel();
            viewModel.KeyFieldName = "OrderID";
            viewModel.Columns.Add("OrderID");
            viewModel.Columns.Add("OrderDate");
            viewModel.Columns.Add("ShipName");
            viewModel.Columns.Add("ProductName");
            viewModel.Columns.Add("Quantity");
            viewModel.Columns.Add("UnitPrice");
            viewModel.FilterExpression = (new BinaryOperator("CustomerID", customerID)).ToString();
            return viewModel;
        }
	}
}