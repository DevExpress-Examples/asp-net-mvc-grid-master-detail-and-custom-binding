@Html.DevExpress().GridView(Sub(settings)
                                settings.Name = "masterGrid"
                                settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "MasterGridViewPartial"}
                                settings.Width = Unit.Percentage(100)

                                settings.KeyFieldName = "CustomerID"
                                settings.Columns.Add("ContactName")
                                settings.Columns.Add("CompanyName")
                                settings.Columns.Add("City")
                                settings.Columns.Add("Country")

                                settings.SettingsDetail.AllowOnlyOneMasterRowExpanded = False
                                settings.SettingsDetail.ShowDetailRow = True

                                settings.SetDetailRowTemplateContent(Sub(c)
                                                                         Html.RenderAction(
                                                                             "DetailGridViewPartial",
                                                                             New With {.CustomerID = DataBinder.Eval(c.DataItem, "CustomerID")})
                                                                     End Sub)

                                settings.CustomBindingRouteValuesCollection.Add(
                                    GridViewOperationType.Sorting,
                                    New With {.Controller = "Home", .Action = "MasterGridViewSortingAction"}
                                )
                                settings.CustomBindingRouteValuesCollection.Add(
                                    GridViewOperationType.Paging,
                                    New With {.Controller = "Home", .Action = "MasterGridViewPagingAction"}
                                )

                                settings.PreRender = _
                                    Sub(sender, e)
                                        Dim grid As MVCxGridView = CType(sender, MVCxGridView)
                                        grid.DetailRows.ExpandRow(0)
                                    End Sub
                            End Sub).BindToCustomData(Model).GetHtml()
