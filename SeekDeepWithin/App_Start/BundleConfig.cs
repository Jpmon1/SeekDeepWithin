﻿using System.Web.Optimization;

namespace SeekDeepWithin
{
   public class BundleConfig
   {
      // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
      public static void RegisterBundles (BundleCollection bundles)
      {
         //BundleTable.EnableOptimizations = true;
         bundles.Add (new ScriptBundle ("~/bundles/jquery").Include (
            "~/Scripts/jquery-1.11.1.min.js",
            "~/Scripts/foundation.min.js"));

         bundles.Add (new ScriptBundle ("~/bundles/jqueryui").Include (
            "~/Scripts/jquery-ui-{version}.js"));

         bundles.Add (new ScriptBundle ("~/bundles/jqueryval").Include (
            "~/Scripts/jquery.unobtrusive-ajax.js",
            "~/Scripts/jquery.validate*"));

         // Use the development version of Modernizr to develop with and learn from. Then, when you're
         // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
         bundles.Add (new ScriptBundle ("~/bundles/modernizr").Include (
            "~/Scripts/modernizr-*"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.editchapter").Include ("~/Scripts/jquery.autocomplete.min.js",
            "~/Scripts/selection.js",
            "~/Scripts/sdw.editchapter.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.addpassages").Include ("~/Scripts/sdw.addpassages.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.addlink").Include ("~/Scripts/jquery.autocomplete.min.js",
            "~/Scripts/selection.js",
            "~/Scripts/sdw.addlink.js"));

         bundles.Add (new ScriptBundle ("~/bundles/sdw.addstyle").Include ("~/Scripts/jquery.autocomplete.min.js",
            "~/Scripts/selection.js",
            "~/Scripts/sdw.addstyle.js"));

         bundles.Add (new ScriptBundle ("~/bundles/jstree").Include ("~/Scripts/jstree.min.js"));

         bundles.Add (new StyleBundle ("~/Content/css").Include ("~/Content/normalize.css",
            "~/Content/jquery-ui.css",
            "~/Content/foundation.min.css",
            "~/Content/whhg.css",
            "~/Content/sdw.default.css"));

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