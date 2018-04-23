<!DOCTYPE html>

<html>
<head>
    <title>@ViewBag.Title</title>
    @Html.DevExpress().GetStyleSheets( 
        New StyleSheet With { .ExtensionSuite = ExtensionSuite.NavigationAndLayout }, _
        New StyleSheet With { .ExtensionSuite = ExtensionSuite.Editors }, _
        New StyleSheet With { .ExtensionSuite = ExtensionSuite.GridView } _
    )
    @Html.DevExpress().GetScripts( 
        New Script With { .ExtensionSuite = ExtensionSuite.NavigationAndLayout }, _ 
        New Script With { .ExtensionSuite = ExtensionSuite.HtmlEditor }, _ 
        New Script With { .ExtensionSuite = ExtensionSuite.GridView } _
    )
</head>
<body>
    @RenderBody()
</body>
</html>