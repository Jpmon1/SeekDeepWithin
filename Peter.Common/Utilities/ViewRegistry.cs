/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 *
 *  This code is provided on an AS IS basis, with no WARRANTIES,
 *  CONDITIONS or GUARANTEES of any kind.
 **/

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Registry for determining which view to show with a given model.
   /// </summary>
   public static class ViewRegistry
   {
      private static readonly Dictionary <Type, Type> s_Types = new Dictionary <Type, Type> ();

      /// <summary>
      /// Registers the given view with the given model.
      /// </summary>
      /// <param name="model">Model to register.</param>
      /// <param name="view">View to register.</param>
      public static void Register (Type model, Type view)
      {
         if (s_Types.ContainsKey (model))
            throw new InvalidOperationException(String.Format ("The given model '{0}' is already associated with a view.", model));
         s_Types.Add (model, view);
      }

      /// <summary>
      /// Gets the view for the given model.
      /// </summary>
      /// <param name="model">Model to get view for.</param>
      /// <returns>The requested view type, null if nonexistant.</returns>
      public static Type Get (Type model)
      {
         if (s_Types.ContainsKey (model))
            return s_Types [model];
         return null;
      }

      /// <summary>
      /// Gets if there is a view for the given model.
      /// </summary>
      /// <param name="model">Model to check view for.</param>
      /// <returns>True if view has been registered, otherwise false.</returns>
      public static bool Contains (Type model)
      {
         return s_Types.ContainsKey (model);
      }

      /// <summary>
      /// Creates a view for the given model.
      /// </summary>
      /// <param name="viewModel">View model to create a view for.</param>
      /// <returns>The newly created view, null if unable to create.</returns>
      public static object CreateView (object viewModel)
      {
         if (viewModel != null)
         {
            var viewType = Get (viewModel.GetType ());
            if (viewType != null)
            {
               var view = Activator.CreateInstance (viewType);
               var fEl = view as FrameworkElement;
               if (fEl != null)
                  fEl.DataContext = viewModel;
               return view;
            }
         }
         return null;
      }

      /// <summary>
      /// Creates a basic data template for the given view model, and view.
      /// </summary>
      /// <param name="viewModelType">The type of view model for the template.</param>
      /// <returns>A created DataTemplate.</returns>
      public static DataTemplate CreateTemplate (Type viewModelType)
      {
         var viewType = Get (viewModelType);
         if (viewType == null)
            return null;

         const string xamlTemplate = "<DataTemplate DataType=\"{{x:Type vm:{0}}}\"><v:{1} /></DataTemplate>";
         var xaml = String.Format (xamlTemplate, viewModelType.Name, viewType.Name);

         var context = new ParserContext { XamlTypeMapper = new XamlTypeMapper (new string[0]) };

         context.XamlTypeMapper.AddMappingProcessingInstruction ("vm", viewModelType.Namespace, viewModelType.Assembly.FullName);
         context.XamlTypeMapper.AddMappingProcessingInstruction ("v", viewType.Namespace, viewType.Assembly.FullName);

         context.XmlnsDictionary.Add ("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
         context.XmlnsDictionary.Add ("x", "http://schemas.microsoft.com/winfx/2006/xaml");
         context.XmlnsDictionary.Add ("vm", "vm");
         context.XmlnsDictionary.Add ("v", "v");

         var template = (DataTemplate)XamlReader.Parse (xaml, context);
         return template;
      }
   }
}
