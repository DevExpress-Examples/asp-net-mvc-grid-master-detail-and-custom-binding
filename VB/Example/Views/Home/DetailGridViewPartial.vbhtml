@Html.DevExpress().GridView(Sub(settings)
                                settings.Name = "detailGrid" & ViewData("CustomerID")
                                settings.SettingsDetail.MasterGridName = "masterGrid"

                                settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "DetailGridViewPartial", .CustomerID = ViewData("CustomerID")}
                                settings.Width = Unit.Percentage(100)

                                settings.KeyFieldName = "OrderID"
                                settings.Columns.Add("OrderID")
                                settings.Columns.Add("OrderDate").PropertiesEdit.DisplayFormatString = "d"
                                settings.Columns.Add("ShipName")
                                settings.Columns.Add("ProductName")
                                settings.Columns.Add("Quantity")
                                settings.Columns.Add("UnitPrice").PropertiesEdit.DisplayFormatString = "c"

                                settings.CustomBindingRouteValuesCollection.Add(
                                    GridViewOperationType.Sorting,
                                    New With {.Controller = "Home", .Action = "DetailGridViewSortingAction", .CustomerID = ViewData("CustomerID")}
                                )
                                settings.CustomBindingRouteValuesCollection.Add(
                                    GridViewOperationType.Paging,
                                    New With {.Controller = "Home", .Action = "DetailGridViewPagingAction", .CustomerID = ViewData("CustomerID")}
                                )
                            End Sub).BindToCustomData(Model).GetHtml()