Imports Microsoft.VisualBasic
Imports System.Web.Mvc
Imports Example.Models
Imports DevExpress.Web.Mvc
Imports DevExpress.Data.Filtering

Namespace Example.Controllers
	Public Class HomeController
		Inherits Controller
		Public Function Index() As ActionResult
			Return View()
		End Function
		Public Function MasterGridViewPartial() As ActionResult
			Dim viewModel = GridViewExtension.GetViewModel("masterGrid")
			If viewModel Is Nothing Then
				viewModel = CreateMasterGridViewModel()
			End If
			Return MasterGridActionCore(viewModel)
		End Function
		Public Function MasterGridViewSortingAction(ByVal column As GridViewColumnState, ByVal reset As Boolean) As ActionResult
			Dim viewModel = GridViewExtension.GetViewModel("masterGrid")
			viewModel.SortBy(column, reset)
			Return MasterGridActionCore(viewModel)
		End Function
		Public Function MasterGridViewPagingAction(ByVal pager As GridViewPagerState) As ActionResult
			Dim viewModel = GridViewExtension.GetViewModel("masterGrid")
			viewModel.Pager.Assign(pager)
			Return MasterGridActionCore(viewModel)
		End Function
		Public Function MasterGridActionCore(ByVal gridViewModel As GridViewModel) As ActionResult
            gridViewModel.ProcessCustomBinding(AddressOf MasterCustomBindingHandlers.GetDataRowCount, AddressOf MasterCustomBindingHandlers.GetData)
			Return PartialView("MasterGridViewPartial", gridViewModel)
		End Function

		Public Function DetailGridViewPartial(ByVal customerID As String) As ActionResult
			Dim viewModel = GridViewExtension.GetViewModel("detailGrid" & customerID)
			If viewModel Is Nothing Then
				viewModel = CreateDetailGridViewModel(customerID)
			End If
			Return DetailGridActionCore(viewModel, customerID)
		End Function
		Public Function DetailGridViewPagingAction(ByVal pager As GridViewPagerState, ByVal customerID As String) As ActionResult
			Dim viewModel = GridViewExtension.GetViewModel("detailGrid" & customerID)
			viewModel.Pager.Assign(pager)
			Return DetailGridActionCore(viewModel, customerID)
		End Function
		Public Function DetailGridViewSortingAction(ByVal column As GridViewColumnState, ByVal reset As Boolean, ByVal customerID As String) As ActionResult
			Dim viewModel = GridViewExtension.GetViewModel("detailGrid" & customerID)
			viewModel.SortBy(column, reset)
			Return DetailGridActionCore(viewModel, customerID)
		End Function
		Public Function DetailGridActionCore(ByVal gridViewModel As GridViewModel, ByVal customerID As String) As ActionResult
            gridViewModel.ProcessCustomBinding(AddressOf DetailCustomBindingHandlers.GetDataRowCount, AddressOf DetailCustomBindingHandlers.GetData)
			ViewData("CustomerID") = customerID
			Return PartialView("DetailGridViewPartial", gridViewModel)
		End Function

		Private Shared Function CreateMasterGridViewModel() As GridViewModel
			Dim viewModel = New GridViewModel()
			viewModel.KeyFieldName = "CustomerID"
			viewModel.Columns.Add("ContactName")
			viewModel.Columns.Add("CompanyName")
			viewModel.Columns.Add("City")
			viewModel.Columns.Add("Country")
			Return viewModel
		End Function
		Private Shared Function CreateDetailGridViewModel(ByVal customerID As String) As GridViewModel
			Dim viewModel = New GridViewModel()
			viewModel.KeyFieldName = "OrderID"
			viewModel.Columns.Add("OrderID")
			viewModel.Columns.Add("OrderDate")
			viewModel.Columns.Add("ShipName")
			viewModel.Columns.Add("ProductName")
			viewModel.Columns.Add("Quantity")
			viewModel.Columns.Add("UnitPrice")
			viewModel.FilterExpression = (New BinaryOperator("CustomerID", customerID)).ToString()
			Return viewModel
		End Function
	End Class
End Namespace