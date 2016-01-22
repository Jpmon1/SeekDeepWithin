using System.Web.Optimization;

namespace SeekDeepWithin
{
   public class BundleConfig
   {
      // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
      public static void RegisterBundles (BundleCollection bundles)
      {
         /************************ COMMON SCRIPTS ************************/
         bundles.Add (new ScriptBundle ("~/bundles/jquery").Include ("~/Scripts/jquery-1.11.2.js",
            "~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.history.js",
            "~/Scripts/fastclick.js",
            "~/Scripts/velocity.js",
            "~/Scripts/velocity.ui.js",
            //"~/Scripts/jsPlumb-2.0.5.js",
            "~/Scripts/sdw/sdw.common.js"));

         /************************ COMMON STYLES ************************/
         bundles.Add (new StyleBundle ("~/Content/css").Include ("~/Content/normalize.css",
            "~/Content/sdw/sdw.autocomplete.css",
            "~/Content/foundation.css",
            //"~/Content/whhg.css",
            "~/Content/sdw/sdw.style.css",
            "~/Content/sdw/sdw.button.css"));

         /************************ SEEK SCRIPTS ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw/seek").Include ("~/Scripts/foundation.js",
            "~/Scripts/masonry.pkgd.js",
            "~/Scripts/sdw/sdw.seek.js"));

         /************************ EDIT SCRIPTS ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw/edit").Include (//"~/Scripts/Base64.js",
            "~/Scripts/hashids.js",
            "~/Scripts/sdw/sdw.edit.js"));
      }
   }
}