Imports Microsoft.VisualBasic
Imports System.Linq
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Linq.Helpers
Imports DevExpress.Web.Mvc

Namespace Example.Models
	Public Class DetailCustomBindingHandlers
		Private Shared ReadOnly Property Model() As IQueryable
			Get
				Return NorthwindDataProvider.GetInvoices()
			End Get
		End Property

		Public Shared Sub GetDataRowCount(ByVal e As GridViewCustomBindingGetDataRowCountArgs)
			e.DataRowCount = Model.ApplyFilter(e.State.FilterExpression).Count()
		End Sub
		Public Shared Sub GetData(ByVal e As GridViewCustomBindingGetDataArgs)
			e.Data = Model.ApplyFilter(e.State.FilterExpression).ApplySorting(e.State.SortedColumns).Skip(e.StartDataRowIndex).Take(e.DataRowCount)
		End Sub
	End Class
End Namespace