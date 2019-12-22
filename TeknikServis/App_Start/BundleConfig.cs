using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;

namespace TeknikServis
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));

            ScriptManager.ScriptResourceMapping.AddDefinition(
                "respond",
                new ScriptResourceDefinition
                {
                    Path = "~/Scripts/respond.js",
                    DebugPath = "~/Scripts/respond.min.js",
                });

            ScriptManager.ScriptResourceMapping.AddDefinition(
 "siminta",
 new ScriptResourceDefinition
 {
     Path = "~/Scripts/siminta.js",
     DebugPath = "~/Scripts/siminta.min.js",
 });
            ScriptManager.ScriptResourceMapping.AddDefinition(
"jquery",
new ScriptResourceDefinition
{
    Path = "~/Scripts/jquery-2.1.3.js",
    DebugPath = "~/Scripts/jquery-2.1.3.min.js",
});
            ScriptManager.ScriptResourceMapping.AddDefinition(
             "pace",
             new ScriptResourceDefinition
             {
                 Path = "~/Scripts/pace.js",
                 DebugPath = "~/Scripts/pace.min.js",
             });

            ScriptManager.ScriptResourceMapping.AddDefinition(
             "metis",
             new ScriptResourceDefinition
             {
                 Path = "~/Scripts/jquery.metisMenu.js",
                 DebugPath = "~/Scripts/jquery.metisMenu.min.js",
             });


            ScriptManager.ScriptResourceMapping.AddDefinition(
"alertify",
new ScriptResourceDefinition
{
    Path = "~/Scripts/alertify.js",
    DebugPath = "~/Scripts/alertify.min.js",
});
            ScriptManager.ScriptResourceMapping.AddDefinition(
"bootstrap",
new ScriptResourceDefinition
{
    Path = "~/Scripts/bootstrap.js",
    DebugPath = "~/Scripts/bootstrap.min.js",
});
            ScriptManager.ScriptResourceMapping.AddDefinition(
"momentlocals",
new ScriptResourceDefinition
{
    Path = "~/Scripts/moment-with-locales.js",
    DebugPath = "~/Scripts/moment-with-locales.min.js",
});

            ScriptManager.ScriptResourceMapping.AddDefinition(
"moments",
new ScriptResourceDefinition
{
    Path = "~/Scripts/moment.js",
    DebugPath = "~/Scripts/moment.min.js",
});
//            ScriptManager.ScriptResourceMapping.AddDefinition(
//"anten-kaydet",
//new ScriptResourceDefinition
//{
//    Path = "~/Scripts/harita-anten-kaydet2.js",
   
//});

            ScriptManager.ScriptResourceMapping.AddDefinition(
"datetime",
new ScriptResourceDefinition
{
    Path = "~/Scripts/bootstrap-datetimepicker.js",
    DebugPath = "~/Scripts/bootstrap-datetimepicker.min.js",
});

        }
    }
}