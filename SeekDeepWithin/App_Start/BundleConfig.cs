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
            "~/Scripts/fastclick.js",
            "~/Scripts/sdw/sdw.panels.js"));
         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
         bundles.Add (new ScriptBundle ("~/bundles/modernizr").Include (
            "~/Scripts/modernizr-*"));

         /************************ COMMON STYLES ************************/
         bundles.Add (new StyleBundle ("~/Content/css").Include ("~/Content/normalize.css",
            "~/Content/foundation.css",
            "~/Content/whhg.css",
            "~/Content/jquery.remodal.css",
            "~/Content/sdw/sdw.autocomplete.css",
            "~/Content/sdw/sdw.switch.css",
            "~/Content/sdw/sdw.pagination.css",
            "~/Content/sdw/sdw.panels.css",
            "~/Content/sdw/sdw.button.css",
            "~/Content/sdw/sdw.base.css"));

         bundles.Add (new StyleBundle ("~/Content/css/slider").Include ("~/Content/jquery.nouislider.css",
            "~/Content/jquery.nouislider.pips.css"));

         /************************ BOOK PAGES ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw.book").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.remodal.js",
            "~/Scripts/sdw/sdw.common.js",
            "~/Scripts/sdw/sdw.book.js"));
         bundles.Add (new ScriptBundle ("~/bundles/sdw.book.index").Include ("~/Scripts/jquery.matchHeight.js",
            "~/Scripts/sdw/sdw.book.index.js"));

         /************************ VERSION PAGES ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw.version").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.remodal.js",
            "~/Scripts/sdw/sdw.common.js",
            "~/Scripts/sdw/sdw.version.js"));

         /************************ TERM PAGES ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw.term").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.remodal.js",
            "~/Scripts/sdw/sdw.common.js",
            "~/Scripts/sdw/sdw.term.js"));

         /************************ TERM ITEM PAGES ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw.item").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.remodal.js",
            "~/Scripts/sdw/sdw.common.js",
            "~/Scripts/sdw/sdw.item.js"));
         bundles.Add (new ScriptBundle ("~/bundles/sdw.item.edit").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.remodal.js",
            "~/Scripts/jquery.nouislider.all.js",
            "~/Scripts/sdw/sdw.select.js",
            "~/Scripts/sdw/sdw.common.js",
            "~/Scripts/sdw/sdw.link.js",
            "~/Scripts/sdw/sdw.style.js",
            "~/Scripts/sdw/sdw.footer.js",
            "~/Scripts/sdw/sdw.item.entry.js",
            "~/Scripts/sdw/sdw.item.edit.js",
            "~/Scripts/foundation.js"));

         /************************ SUBBOOK PAGES ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw.subbook").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.remodal.js",
            "~/Scripts/sdw/sdw.common.js",
            "~/Scripts/sdw/sdw.subbook.js"));

         /************************ CHAPTER PAGES ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw.chapter").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.remodal.js",
            "~/Scripts/jquery.nouislider.all.js",
            "~/Scripts/sdw/sdw.select.js",
            "~/Scripts/sdw/sdw.common.js",
            "~/Scripts/sdw/sdw.link.js",
            "~/Scripts/sdw/sdw.style.js",
            "~/Scripts/sdw/sdw.footer.js",
            "~/Scripts/sdw/sdw.chapter.js",
            "~/Scripts/sdw/sdw.passage.js",
            "~/Scripts/sdw/sdw.item.edit.js",
            "~/Scripts/foundation.js"));

         /************************ READ PAGES ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw.read").Include ("~/Scripts/sdw/sdw.read.js"));

         /************************ SEARCH PAGES ************************/
         bundles.Add (new ScriptBundle ("~/bundles/sdw.search").Include ("~/Scripts/jquery.matchHeight.js",
            "~/Scripts/sdw/sdw.search.js"));
         bundles.Add (new ScriptBundle ("~/bundles/sdw.index").Include ("~/Scripts/sdw/sdw.common.js",
            "~/Scripts/jquery.remodal.js",
            "~/Scripts/sdw/sdw.index.js"));
      }
   }
}