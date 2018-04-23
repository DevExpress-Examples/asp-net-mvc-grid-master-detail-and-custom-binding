Imports Microsoft.VisualBasic
Imports System.Linq
Imports System.Web

Namespace Example.Models
	Public NotInheritable Class NorthwindDataProvider
		Private Const NorthwindDataContextKey As String = "DXNorthwindDataContext"

		Private Sub New()
		End Sub
		Public Shared ReadOnly Property DB() As NorthwindDataContext
			Get
				If HttpContext.Current.Items(NorthwindDataContextKey) Is Nothing Then
					HttpContext.Current.Items(NorthwindDataContextKey) = New NorthwindDataContext()
				End If
				Return CType(HttpContext.Current.Items(NorthwindDataContextKey), NorthwindDataContext)
			End Get
		End Property

		Public Shared Function GetCustomers() As IQueryable(Of Customer)
			Return DB.Customers
		End Function
		Public Shared Function GetInvoices() As IQueryable(Of Invoice)
			Return DB.Invoices
		End Function
	End Class
End Namespace