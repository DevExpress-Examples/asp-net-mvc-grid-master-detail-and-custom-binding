# How to create a master-detail GridView with paging and sorting using Custom Data Binding


<p>This sample demonstrates how to manually provide data for two GridView extensions that are used in a master-detail relationship. In this implementation, only sorting and paging operations of the grids are handled in the corresponding Action methods.</p><p>To learn more on the GridView's custom data binding feature, please refer to the <a href="http://documentation.devexpress.com/#AspNet/CustomDocument14374"><u>Custom Data Binding - Overview</u></a> help topic.</p><p>Note that this sample provides a universal implementation approach. It can be easily adopted and used for every custom data source object if it implements the <strong>IQueryable</strong> interface. </p><p>The common logic of each grid's custom binding implementation is similar to the implementation demonstrated by the <a href="http://www.devexpress.com/Support/Center/Example/Details/E4394"><u>E4394</u></a> code sample. The difference is that in this sample the master grid's detail row template is defined by using another (detail) GridView extension. Each detail grid instance is provided with information on the corresponding master row's key field value. This value is passed to a detail grid's Action methods (as a parameter) and to the grid's Partial View (as a ViewData object).</p><p></p><p>In short, this sample's implementation logic is as follows:</p><br />
<p>In both grid Partial Views (see MasterGridViewPartial.cshtml and DetailGridViewPartial.cshtml in Views > Home), a grid's <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebMvcGridViewSettings_CustomBindingRouteValuesCollectiontopic"><u>CustomBindingRouteValuesCollection</u></a> property is used to define handling actions for sorting and paging operations; the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebMvcGridViewSettings_CallbackRouteValuestopic"><u>CallbackRouteValues</u></a> property defines the action to handle all other (standard) grid callbacks. In the master grid's Partial View, the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebMvcGridViewSettings_SetDetailRowTemplateContenttopic"><u>SetDetailRowTemplateContent</u></a> method delegate is implemented to define detail row content and provide it with the master row's key field value - the value of the "CustomerID" column. In the detail grid's Partial View, the received corresponding master row key field value (the value of the "CustomerID" column) is passed to the specified actions.</p><p>In the Controller (Controller > HomeController.cs), the specified Action methods are implemented for both grids to update a specific grid view model object (<a href="http://documentation.devexpress.com/#AspNet/clsDevExpressWebMvcGridViewModeltopic"><u>GridViewModel</u></a> that maintains a grid's state) with information of the performed operation (if required). Then, a grid view model's <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebMvcGridViewModel_ProcessCustomBindingtopic"><u>ProcessCustomBinding</u></a> method is called to delegate a binding implementation to specific model-layer methods pointed by the method's certain parameters.</p><p>At the Model layer (see MasterCustomBindingModel.cs and DetailCustomBindingModel.cs in Models), two specified delegates are implemented for each grid to populate the corresponding grid view model with the required data. Generally, in the provided implementation of model-level binding delegates, you just need to modify a single code line in each model file to point to your particular model object:<br />
</p>

```cs
(MasterCustomBindingModel.cs)
        static IQueryable Model { get { return NorthwindDataProvider.GetCustomers(); } }

```



```cs
(DetailCustomBindingModel.cs)
        static IQueryable Model { get { return NorthwindDataProvider.GetInvoices(); } }

```

<p>Finally, the resulting grid view model object is passed from the Controller to a particular grid's Partial View as a Model. For a detail grid, the master row key value is also passed to the Partial View as a ViewData object. In the Partial View, each grid binds to the Model via the <a href="http://documentation.devexpress.com/#AspNet/DevExpressWebMvcGridViewExtension_BindToCustomDatatopic"><u>BindToCustomData</u></a> method.</p><p>Note that when implementing the grid's custom data binding, the <a href="http://help.devexpress.com/#AspNet/clsDevExpressWebMvcDevExpressEditorsBindertopic"><u>DevExpressEditorsBinder</u></a> must be used instead of the default model binder to correctly transfer values from DevExpress editors back to the corresponding data model fields. In this code example, the DevExpressEditorsBinder is assigned to the ModelBinders.Binders.DefaultBinder property within the Global.asax file, thus overriding the default model binder.</p><p><strong>See Also:<br />
</strong><a href="https://www.devexpress.com/Support/Center/p/E4394">How to implement a simple custom binding scenario for GridView</a></p>

<br/>


