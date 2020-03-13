Imports System.Linq
Imports System.Web

Namespace Example.Models
	Public Module NorthwindDataProvider
		Private Const NorthwindDataContextKey As String = "DXNorthwindDataContext"

		Public ReadOnly Property DB() As NorthwindDataContext
			Get
				If HttpContext.Current.Items(NorthwindDataContextKey) Is Nothing Then
					HttpContext.Current.Items(NorthwindDataContextKey) = New NorthwindDataContext()
				End If
				Return DirectCast(HttpContext.Current.Items(NorthwindDataContextKey), NorthwindDataContext)
			End Get
		End Property

		Public Function GetCustomers() As IQueryable(Of Customer)
			Return DB.Customers
		End Function
		Public Function GetInvoices() As IQueryable(Of Invoice)
			Return DB.Invoices
		End Function
	End Module
End Namespace