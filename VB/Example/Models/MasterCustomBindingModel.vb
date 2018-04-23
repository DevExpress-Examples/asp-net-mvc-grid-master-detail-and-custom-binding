Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.Data
Imports DevExpress.Data.Filtering
Imports DevExpress.Data.Linq
Imports DevExpress.Data.Linq.Helpers
Imports DevExpress.Web.Mvc

Namespace Example.Models
	Public Class MasterCustomBindingHandlers
		Private Shared ReadOnly Property Model() As IQueryable
			Get
				Return NorthwindDataProvider.GetCustomers()
			End Get
		End Property

		Public Shared Sub GetDataRowCount(ByVal e As GridViewCustomBindingGetDataRowCountArgs)
			e.DataRowCount = Model.Count()
		End Sub
		Public Shared Sub GetData(ByVal e As GridViewCustomBindingGetDataArgs)
			e.Data = Model.ApplySorting(e.State.SortedColumns).Skip(e.StartDataRowIndex).Take(e.DataRowCount)
		End Sub
	End Class

	Public Module GridViewCustomOperationDataHelper
        Sub New()
        End Sub
        Private ReadOnly Property Converter() As ICriteriaToExpressionConverter
            Get
                Return New CriteriaToExpressionConverter()
            End Get
        End Property

        <System.Runtime.CompilerServices.Extension()> _
        Public Function [Select](ByVal query As IQueryable, ByVal fieldName As String) As IQueryable
            Return query.MakeSelect(Converter, New OperandProperty(fieldName))
        End Function
        <System.Runtime.CompilerServices.Extension()> _
        Public Function ApplySorting(ByVal query As IQueryable, ByVal sortedColumns As IEnumerable(Of GridViewColumnState)) As IQueryable
            For Each column As GridViewColumnState In sortedColumns
                query = ApplySorting(query, column.FieldName, column.SortOrder)
            Next column
            Return query
        End Function
        <System.Runtime.CompilerServices.Extension()> _
        Public Function ApplySorting(ByVal query As IQueryable, ByVal fieldName As String, ByVal order As ColumnSortOrder) As IQueryable
            Return query.MakeOrderBy(Converter, New ServerModeOrderDescriptor(New OperandProperty(fieldName), order = ColumnSortOrder.Descending))
        End Function
        <System.Runtime.CompilerServices.Extension()> _
        Public Function ApplyFilter(ByVal query As IQueryable, ByVal filterExpression As String) As IQueryable
            Return query.AppendWhere(Converter, CriteriaOperator.Parse(filterExpression))
        End Function
	End Module
End Namespace