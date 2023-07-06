<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128551320/19.2.6%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4398)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

#  How to implement a simple custom binding scenario
# Grid View for ASP.NET MVC - How to implement a master-detail grid with a simple custom binding scenario
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/e4398/)**
<!-- run online end -->

This example demonstrates how to implement a simple custom binding scenario for two [GridView](https://docs.devexpress.com/AspNetMvc/8966/components/grid-view) extensions that are used in a master-detail relationship, and handle sorting and paging operations in the corresponding Action methods.

You can modify this approach to use it with any data source object that implements the `IQueryable` interface.

## Implementation details

Custom data binding requires that the [DevExpressEditorsBinder](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.DevExpressEditorsBinder) is used instead of the default model binder to correctly transfer values from DevExpress editors back to the corresponding data model fields. 
Assign `DevExpressEditorsBinder`  to the `ModelBinders.Binders.DefaultBinder` property in the **Global.asax** file to override the default model binder.

```csharp
ModelBinders.Binders.DefaultBinder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
```

### Grid partial view

Call the master grid's [SetDetailRowTemplateContent](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.GridViewSettings.SetDetailRowTemplateContent.overloads) method to define detail row content and provide it with the master row's key field value (the value of the "CustomerID" column).

The [CustomBindingRouteValuesCollection](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.GridViewSettings.CustomBindingRouteValuesCollection) property allows you to assign particular handling Actions for four data operations - paging, sorting, grouping, and filtering. In this example, the property specifies custom routing for sorting and paging operations.

The [CallbackRouteValues](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.GridSettingsBase.CallbackRouteValues) property specifies the action that handles all other (standard) grid callbacks.

**Master grid:**
```razor
@Html.DevExpress().GridView(
    settings => {
        settings.Name = "masterGrid";
        settings.KeyFieldName = "CustomerID";
        settings.SettingsDetail.ShowDetailRow = true;
        settings.SetDetailRowTemplateContent(c => {
            Html.RenderAction("DetailGridViewPartial", new { CustomerID = DataBinder.Eval(c.DataItem, "CustomerID") });
        });

        settings.CallbackRouteValues = new { Controller = "Home", Action = "MasterGridViewPartial" };
        settings.CustomBindingRouteValuesCollection.Add(
            GridViewOperationType.Sorting,
            new { Controller = "Home", Action = "MasterGridViewSortingAction" }
        );
        settings.CustomBindingRouteValuesCollection.Add(
            GridViewOperationType.Paging,
            new { Controller = "Home", Action = "MasterGridViewPagingAction" }
        );
        // ...
```

**Detail grid:**
```razor
@Html.DevExpress().GridView(
    settings => {
        settings.Name = "detailGrid" + ViewData["CustomerID"];
        settings.SettingsDetail.MasterGridName = "masterGrid";
        settings.KeyFieldName = "OrderID";

        settings.CallbackRouteValues = new { Controller = "Home", Action = "DetailGridViewPartial", CustomerID = ViewData["CustomerID"] };
        settings.CustomBindingRouteValuesCollection.Add(
            GridViewOperationType.Sorting,
            new { Controller = "Home", Action = "DetailGridViewSortingAction", CustomerID = ViewData["CustomerID"] }
        );
        settings.CustomBindingRouteValuesCollection.Add(
            GridViewOperationType.Paging,
            new { Controller = "Home", Action = "DetailGridViewPagingAction", CustomerID = ViewData["CustomerID"] }
        );
        // ...
```

### Controller

Action methods update the [GridViewModel](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.GridViewModel) object with the information from the performed operation. The [ProcessCustomBinding](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.GridViewModel.ProcessCustomBinding.overloads) method delegates the binding implementation to specific model-layer methods specified by the Action method's parameters.

**Master grid:**
```csharp
        public ActionResult MasterGridViewPartial() {
            var viewModel = GridViewExtension.GetViewModel("masterGrid");
            if (viewModel == null)
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
```

**Detail grid:**
```csharp
        public ActionResult DetailGridViewPartial(string customerID) {
            var viewModel = GridViewExtension.GetViewModel("detailGrid" + customerID);
            if (viewModel == null)
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
```

### Model

The specified delegates populate the Grid View model with data. To bind the Grid to your particular model object, modify the following code line:

```cs
static IQueryable Model { get { return NorthwindDataProvider.GetCustomers(); } }
```

The Grid View model object is passed from the Controller to the grid's Partial View as a Model. In the Partial View, the [BindToCustomData](https://docs.devexpress.com/AspNetMvc/DevExpress.Web.Mvc.GridViewExtension.BindToCustomData(DevExpress.Web.Mvc.GridViewModel)) method binds the grid to the Model.

## Files to Review todo

* [MasterGridViewPartial.cshtml](./CS/Example/Views/Home/MasterGridViewPartial.cshtml) (VB: [MasterGridViewPartial.vbhtml](./VB/Example/Views/Home/MasterGridViewPartial.vbhtml))
* [DetailGridViewPartial.cshtml](./CS/Example/Views/Home/DetailGridViewPartial.cshtml) (VB: [DetailGridViewPartial.vbhtml](./VB/Example/Views/Home/DetailGridViewPartial.vbhtml))
* [HomeController.cs](./CS/Example/Controllers/HomeController.cs) (VB: [HomeController.vb](./VB/Example/Controllers/HomeController.vb))

* [DetailCustomBindingModel.cs](./CS/Example/Models/DetailCustomBindingModel.cs) (VB: [DetailCustomBindingModel.vb](./VB/Example/Models/DetailCustomBindingModel.vb))
* [MasterCustomBindingModel.cs](./CS/Example/Models/MasterCustomBindingModel.cs) (VB: [MasterCustomBindingModel.vb](./VB/Example/Models/MasterCustomBindingModel.vb))
* [Northwind.cs](./CS/Example/Models/Northwind.cs) (VB: [Northwind.vb](./VB/Example/Models/Northwind.vb))



## Documentation

* [Custom Data Binding](https://docs.devexpress.com/AspNetMvc/14321/components/grid-view/binding-to-data/custom-data-binding)

## More Examples

* [Grid View for ASP.NET MVC - How to implement a simple custom binding scenario](https://github.com/DevExpress-Examples/asp-net-mvc-grid-custom-binding-with-sorting-paging)
