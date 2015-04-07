using System.Web.Optimization;

namespace SeekDeepWithin
{
   public class BundleConfig
   {
      // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
      public static void RegisterBundles (BundleCollection bundles)
      {
         //BundleTable.EnableOptimizations = true;
         bundles.Add (new ScriptBundle ("~/bundles/jquery").Include (
            "~/Scripts/jquery-1.11.2.js",
            "~/Scripts/jquery.cookie.js",
            "~/Scripts/foundation/foundation.js",
            "~/Scripts/foundation/foundation.*"));

         bundles.Add (new ScriptBundle ("~/bundles/jqueryui").Include (
            "~/Scripts/jquery-ui-{version}.js"));

         bundles.Add (new ScriptBundle ("~/bundles/jqueryval").Include (
            "~/Scripts/jquery.unobtrusive-ajax.js",
            "~/Scripts/jquery.validate*"));

         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
         bundles.Add (new ScriptBundle ("~/bundles/modernizr").Include (
            "~/Scripts/modernizr-*"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.editchapter").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/sdw/selection.js",
            "~/Scripts/sdw/sdw.chapter.js",
            "~/Scripts/jquery.nouislider.all.js",
            "~/Scripts/sdw/sdw.passage.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.editglossary").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/sdw/selection.js",
            "~/Scripts/jquery.nouislider.all.js",
            "~/Scripts/sdw/sdw.glossary.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.editTerm").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/sdw/sdw.link.js",
            "~/Scripts/sdw/sdw.tag.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.addpassages").Include ("~/Scripts/sdw/sdw.addpassages.js",
            "~/Scripts/sdw/sdw.convert.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.addentries").Include ("~/Scripts/sdw/sdw.addentries.js",
            "~/Scripts/sdw/sdw.convert.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.contentsedit").Include ("~/Scripts/jstree.js",
            "~/Scripts/jquery.sticky-kit.js",
            "~/Scripts/sdw/sdw.contentsedit.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.read").Include ("~/Scripts/jquery.sticky-kit.js",
            "~/Scripts/sdw/sdw.read.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.versionedit").Include ("~/Scripts/sdw/sdw.version.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.subbookedit").Include ("~/Scripts/sdw/sdw.subbook.js",
            "~/Scripts/sdw/sdw.tag.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.styleedit").Include ("~/Scripts/jquery.nouislider.all.js",
            "~/Scripts/sdw/sdw.select.js",
            "~/Scripts/sdw/sdw.style.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.footeredit").Include ("~/Scripts/jquery.nouislider.all.js",
            "~/Scripts/sdw/sdw.select.js",
            "~/Scripts/sdw/sdw.footer.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.headeredit").Include ("~/Scripts/sdw/sdw.header.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.linkedit").Include ("~/Scripts/jquery.autocomplete.js",
            "~/Scripts/jquery.nouislider.all.js",
            "~/Scripts/sdw/sdw.select.js",
            "~/Scripts/sdw/sdw.link.js"));

         bundles.Add (new StyleBundle ("~/Content/css").Include ("~/Content/normalize.css",
            "~/Content/jquery-ui.css",
            "~/Content/foundation.css",
            "~/Content/whhg.css",
            "~/Content/sdw.default.css"));

         bundles.Add (new StyleBundle ("~/Content/css/slider").Include ("~/Content/jquery.nouislider.css",
            "~/Content/jquery.nouislider.pips.css"));

         bundles.Add (new StyleBundle ("~/Content/themes/base/css").Include (
            "~/Content/themes/base/jquery.ui.core.css",
            "~/Content/themes/base/jquery.ui.resizable.css",
            "~/Content/themes/base/jquery.ui.selectable.css",
            "~/Content/themes/base/jquery.ui.accordion.css",
            "~/Content/themes/base/jquery.ui.autocomplete.css",
            "~/Content/themes/base/jquery.ui.button.css",
            "~/Content/themes/base/jquery.ui.dialog.css",
            "~/Content/themes/base/jquery.ui.slider.css",
            "~/Content/themes/base/jquery.ui.tabs.css",
            "~/Content/themes/base/jquery.ui.datepicker.css",
            "~/Content/themes/base/jquery.ui.progressbar.css",
            "~/Content/themes/base/jquery.ui.theme.css"));
      }
   }
}