/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace IinAll.Edit.DragAndDrop
{
   /// <summary>
   /// Extension methods for drag and drop.
   /// </summary>
   public static class ItemControlHelpers
   {
      /// <summary>
      /// Gets a container for the item in the items control at the given point.
      /// </summary>
      /// <param name="itemsControl">Item control to look in.</param>
      /// <param name="position">Point in control to get container.</param>
      /// <returns>Container of item at given point, null if unable to do so.</returns>
      public static UIElement GetItemContainerAt (this ItemsControl itemsControl, Point position)
      {
         var uiElement = itemsControl.InputHitTest (position) as UIElement;
         return uiElement != null ? GetItemContainer (itemsControl, uiElement) : null;
      }

      /// <summary>
      /// Gets a container for the given child item.
      /// </summary>
      /// <param name="itemsControl">Items control to generate a container with.</param>
      /// <param name="child">Child item to get container for.</param>
      /// <returns>Requested container, null if unsuccessful.</returns>
      public static UIElement GetItemContainer (this ItemsControl itemsControl, UIElement child)
      {
         Type itemType = GetItemContainerType (itemsControl);

         if (itemType != null)
            return (UIElement) child.GetVisualAncestor (itemType);

         return null;
      }

      /// <summary>
      /// Gets the container type for the given item control.
      /// </summary>
      /// <param name="itemsControl">Items control to get container type.</param>
      /// <returns>Container type, null if unsuccessful.</returns>
      public static Type GetItemContainerType (this ItemsControl itemsControl)
      {
         return itemsControl.Items.Count > 0 ? itemsControl.ItemContainerGenerator.ContainerFromIndex (0).GetType () : null;
      }

      /// <summary>
      /// Gets the list of selected items for the given items control.
      /// </summary>
      /// <param name="itemsControl">Items control to get selected items for.</param>
      /// <returns>List of selected items.</returns>
      public static IEnumerable GetSelectedItems (this ItemsControl itemsControl)
      {
         if (itemsControl is MultiSelector)
            return ((MultiSelector) itemsControl).SelectedItems;
         if (itemsControl is ListBox)
         {
            var listBox = (ListBox) itemsControl;
            if (listBox.SelectionMode == SelectionMode.Single)
               return Enumerable.Repeat (listBox.SelectedItem, 1);
            return listBox.SelectedItems;
         }
         if (itemsControl is TreeView)
            return Enumerable.Repeat (((TreeView) itemsControl).SelectedItem, 1);
         if (itemsControl is Selector)
            return Enumerable.Repeat (((Selector) itemsControl).SelectedItem, 1);
         return Enumerable.Empty <object> ();
      }

      /// <summary>
      /// Tries to get the orientation for the given items control.
      /// </summary>
      /// <param name="itemsControl">Items control to get orientation for.</param>
      /// <returns>Orientation of items control, vertical if not found.</returns>
      public static Orientation GetItemsPanelOrientation (this ItemsControl itemsControl)
      {
         var itemsPresenter = itemsControl.GetVisualDescendent<ItemsPresenter> ();
         DependencyObject itemsPanel = VisualTreeHelper.GetChild (itemsPresenter, 0);
         PropertyInfo orientationProperty = itemsPanel.GetType ().GetProperty ("Orientation", typeof (Orientation));

         if (orientationProperty != null)
            return (Orientation) orientationProperty.GetValue (itemsPanel, null);
         // Default...
         return Orientation.Vertical;
      }
   }
}
