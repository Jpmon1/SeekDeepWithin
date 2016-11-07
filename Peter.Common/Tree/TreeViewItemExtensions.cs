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
 *
 **/

using System.Windows.Controls;
using System.Windows.Media;

namespace Peter.Common.Tree
{
   /// <summary>
   /// Extension methods for tree view items.
   /// </summary>
   public static class TreeViewItemExtensions
   {
      /// <summary>
      /// Gets the depth of the given tree view item.
      /// </summary>
      /// <param name="item">Item to get depth for.</param>
      /// <returns>The depth of the item, 0 if not found.</returns>
      public static int GetDepth (this TreeViewItem item)
      {
         TreeViewItem parent;
         while ((parent = GetParent (item)) != null)
         {
            return GetDepth (parent) + 1;
         }
         return 0;
      }

      /// <summary>
      /// Gets the parent of the given tree view item.
      /// </summary>
      /// <param name="item">Item to get parent for.</param>
      /// <returns>Parent tree view item.</returns>
      private static TreeViewItem GetParent (TreeViewItem item)
      {
         var parent = VisualTreeHelper.GetParent (item);
         while (!(parent is TreeViewItem || parent is TreeView))
         {
            parent = VisualTreeHelper.GetParent (parent);
         }
         return parent as TreeViewItem;
      }
   }
}
