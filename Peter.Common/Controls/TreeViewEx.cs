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

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Peter.Common.Controls
{
   /// <summary>
   /// Represents an extended tree control.
   /// </summary>
   public class TreeViewEx : TreeView
   {
      /// <summary>
      /// Initializes a new XTreeView.
      /// </summary>
      public TreeViewEx ()
      {
         this.SelectedItemChanged += this.SelectionChanged;
         this.PreviewKeyDown += this.OnKeyPress;
         this.MouseDoubleClick += this.OnDoubleClick;
         this.PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
         //this.ItemContainerGenerator.StatusChanged += ItemContainerGeneratorStatusChanged;
      }

      /// <summary>
      /// Dependency property for Selected tree item.
      /// </summary>
      public static readonly DependencyProperty SelectedTreeItemProperty =
         DependencyProperty.Register ("SelectedTreeItem", typeof (object), typeof (TreeViewEx),
         new PropertyMetadata (default (object)));

      /// <summary>
      /// Gets or Sets the selected tree item.
      /// </summary>
      public object SelectedTreeItem
      {
         get { return GetValue (SelectedTreeItemProperty); }
         set { SetValue (SelectedTreeItemProperty, value); }
      }

      /// <summary>
      /// Dependency property for the execute command.
      /// </summary>
      public static readonly DependencyProperty ExecuteCommandProperty =
         DependencyProperty.Register ("ExecuteCommand", typeof (ICommand), typeof (TreeViewEx),
         new PropertyMetadata (default (ICommand)));

      /// <summary>
      /// Gets or Sets the command to use when the user is attempting to execute an item.
      /// </summary>
      public ICommand ExecuteCommand
      {
         get { return (ICommand)GetValue (ExecuteCommandProperty); }
         set { SetValue (ExecuteCommandProperty, value); }
      }

      /// <summary>
      /// Occurs when the selection changes.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void SelectionChanged (object sender, RoutedPropertyChangedEventArgs<object> e)
      {
         this.SelectedTreeItem = e.NewValue;
         this.OnSelectedTreeItemChanged ();
      }

      /// <summary>
      /// Occurs when the selected tree item changes;
      /// </summary>
      protected virtual void OnSelectedTreeItemChanged (){ }

      /// <summary>
      /// Occurs when a double click event happens.
      /// </summary>
      /// <param name="sender">This tree view.</param>
      /// <param name="e">MouseButtonEventArgs</param>
      private void OnDoubleClick (object sender, MouseButtonEventArgs e)
      {
         this.Execute ();
      }

      /// <summary>
      /// Occurs when a key is pressed.
      /// </summary>
      /// <param name="sender">This tree view.</param>
      /// <param name="e">KeyEventArgs</param>
      private void OnKeyPress (object sender, KeyEventArgs e)
      {
         if (e.Key == Key.Enter)
            this.Execute ();
      }

      /// <summary>
      /// Occurs when the mouse is clicked.
      /// </summary>
      /// <param name="sender">This tree view.</param>
      /// <param name="e">MouseButtonEventArgs</param>
      private static void OnPreviewMouseRightButtonDown (object sender, MouseButtonEventArgs e)
      {
         var treeViewItem = VisualUpwardSearch (e.OriginalSource as DependencyObject);

         if (treeViewItem != null)
         {
            treeViewItem.Focus ();
            e.Handled = true;
         }
      }

      /// <summary>
      /// Searches for the tree view item.
      /// </summary>
      /// <param name="source">Original source</param>
      /// <returns>The found tree view item.</returns>
      static TreeViewItem VisualUpwardSearch (DependencyObject source)
      {
         while (source != null && !(source is TreeViewItem))
            source = VisualTreeHelper.GetParent (source);

         return source as TreeViewItem;
      }

      /// <summary>
      /// Executes the selected item.
      /// </summary>
      protected virtual void Execute ()
      {
         if (this.SelectedItem != null)
         {
            if (this.ExecuteCommand != null)
            {
               if (this.ExecuteCommand.CanExecute (this.SelectedItem))
                  this.ExecuteCommand.Execute (this.SelectedItem);
            }
         }
      }
   }
}
