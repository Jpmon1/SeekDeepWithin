/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace IinAll.Edit.DragAndDrop
{
   /// <summary>
   /// Methods used for finding dependency objects in visual/logical dependency trees.
   /// </summary>
   public static class DependencyObjectHelper
   {
      /// <summary>
      /// Find a specific parent object type in the visual tree
      /// </summary>
      public static T FindVisualParent<T> (this DependencyObject current) where T : DependencyObject
      {
         DependencyObject item = VisualTreeHelper.GetParent (current);
         do
         {
            if (item is T)
               return (T)item;
            item = VisualTreeHelper.GetParent (item);
         } while (item != null);

         return null;
      }

      /// <summary>
      /// Gets a visual ancestor of the given dependency object with the given type.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="type">Type to search for.</param>
      /// <returns>Requested item if found, otherwise null.</returns>
      public static DependencyObject GetVisualAncestor (this DependencyObject d, Type type)
      {
         DependencyObject item = VisualTreeHelper.GetParent (d);

         while (item != null)
         {
            if (item.GetType () == type) return item;
            item = VisualTreeHelper.GetParent (item);
         }

         return null;
      }

      /// <summary>
      /// Gets the first visual descendent that matches the requested type.
      /// </summary>
      /// <typeparam name="T">Type of descendent to look for.</typeparam>
      /// <param name="d">Parent objects to search descendents.</param>
      /// <returns>Visaul descendent.</returns>
      public static T GetVisualDescendent<T> (this DependencyObject d) where T : DependencyObject
      {
         return d.GetVisualDescendents<T> ().FirstOrDefault ();
      }

      /// <summary>
      /// Gets an enumerable collection of descendents that match the given type.
      /// </summary>
      /// <typeparam name="T">Type of descendent to look for.</typeparam>
      /// <param name="d">Parent objects to search descendents.</param>
      /// <returns>Visaul descendent collection.</returns>
      public static IEnumerable<T> GetVisualDescendents<T> (this DependencyObject d) where T : DependencyObject
      {
         int childCount = VisualTreeHelper.GetChildrenCount (d);

         for (int n = 0; n < childCount; n++)
         {
            var child = VisualTreeHelper.GetChild (d, n);

            if (child is T)
               yield return (T)child;

            foreach (var match in GetVisualDescendents <T> (child))
               yield return match;
         }
      }

      /// <summary>
      /// Find a specific parent object type in the visual tree
      /// </summary>
      public static T FindLogicalParent<T> (this DependencyObject current) where T : DependencyObject
      {
         DependencyObject item = LogicalTreeHelper.GetParent (current);
         do
         {
            if (item is T)
               return (T)item;
            item = LogicalTreeHelper.GetParent (item);
         } while (item != null);

         return null;
      }
   }
}
