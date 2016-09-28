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
using System.Windows;
using System.Windows.Controls;
using IinAll.Edit.DragAndDrop.Adorners;

namespace IinAll.Edit.DragAndDrop
{
   /// <summary>
   /// A drag controller.
   /// </summary>
   public class DropController
   {
      #region Fields

      /// <summary>
      /// Drop target.
      /// </summary>
      private UIElement m_DropTarget;

      /// <summary>
      /// Drag over handler.
      /// </summary>
      private static readonly DragEventHandler s_DropTargetDragOver = (s, e) => { e.Handled = true; e.Effects = DragDropEffects.Move; }; 

      #endregion

      #region Properties

      /// <summary>
      /// Gets the drop target.
      /// </summary>
      public UIElement DropTarget
      {
         get { return this.m_DropTarget; }
         set
         {
            this.m_DropTarget = value;
            this.SetEvents (true);
         }
      }

      /// <summary>
      /// Gets the initiating drag controller.
      /// </summary>
      public DragController DragController { get; internal set; }

      /// <summary>
      /// Gets the attached drop adorner.
      /// </summary>
      public DragNDropAdorner DropAdorner { get; protected set; }

      #endregion

      #region Methods

      /// <summary>
      /// Checks if a drop can happen or not.
      /// </summary>
      /// <param name="mousePoint">Position of mouse relative to drop target.</param>
      /// <returns>True if we can drop, otherwise false.</returns>
      public virtual bool OkToDrop (Point mousePoint)
      {
         var dropItemsControl = this.DropTarget as ItemsControl;
         var dragItemsControl = this.DragController.DragSource as ItemsControl;
         if (dropItemsControl != null && dragItemsControl != null)
         {
            if (dropItemsControl.ItemsSource != null && dragItemsControl.ItemsSource != null)
               return dropItemsControl.ItemsSource.GetType () == dragItemsControl.ItemsSource.GetType ();
         }

         return false;
      }

      /// <summary>
      /// Cancels the drop.
      /// </summary>
      public void CancelDrop ()
      {
         if (this.DropTarget == null) return;
         this.DropDone ();
         this.RemoveAdorner ();
         this.SetEvents (false);
         this.DragController.DropController = null;
      }

      /// <summary>
      /// Removes the drag adorner.
      /// </summary>
      public virtual void RemoveAdorner ()
      {
         if (this.DropAdorner != null)
         {
            this.DropAdorner.Detach ();
            this.DropAdorner = null;
         }
      }

      /// <summary>
      /// Handles the drop operation.
      /// </summary>
      /// <param name="mousePoint">Position of mouse relative to drop target.</param>
      protected virtual void Drop (Point mousePoint)
      {
         // This should handle most items control drops...
         if (this.DragController.DragData == null) return;
         var newItemsControl = this.DropTarget as ItemsControl;
         var oldItemsControl = this.DragController.DragSource as ItemsControl;
         if (newItemsControl != null && oldItemsControl != null)
         {
            var newItems = newItemsControl.ItemsSource as IList ?? newItemsControl.Items;
            var oldItems = oldItemsControl.ItemsSource as IList ?? oldItemsControl.Items;
            if (oldItems == null || newItems == null) return;

            int orgIndex = oldItems.IndexOf (this.DragController.DragData);
            var container = newItemsControl.GetItemContainerAt (mousePoint);
            int insertIndex = container == null ? 0 
               : newItemsControl.ItemContainerGenerator.IndexFromContainer (container);

            if (Equals (newItemsControl, oldItemsControl))
            {
               if (orgIndex < insertIndex)
                  insertIndex--;
            }
            if (insertIndex > newItems.Count) return;
            if (orgIndex != -1) oldItems.RemoveAt (orgIndex);
            if (insertIndex == -1) insertIndex = 0;
            newItems.Insert (insertIndex, this.DragController.DragData);
         }
      }

      /// <summary>
      /// Called when the drop for the drop target is done.
      /// </summary>
      protected virtual void DropDone ()
      {
      }

      /// <summary>
      /// Updates the attached adorner.
      /// </summary>
      /// <param name="mousePoint">Position of mouse relative to drop target.</param>
      protected virtual void UpdateAdorner (Point mousePoint)
      {
      }

      /// <summary>
      /// Occurs when a drag leaves the drop target. Override for special operations.
      /// </summary>
      protected virtual void DragLeaving (DragEventArgs e)
      {
      }

      /// <summary>
      /// Occurs when a drag is over the drop target. Override for special operations.
      /// </summary>
      protected virtual void DragOver (DragEventArgs e)
      {
      }

      /// <summary>
      /// Occurs when a drag enters over the drop target. Override for special operations.
      /// </summary>
      protected virtual void DragEnter (DragEventArgs e)
      {
      }

      /// <summary>
      /// Shows the adorner.
      /// </summary>
      /// <param name="mousePoint">Position of mouse relative to drop target.</param>
      protected void ShowAdorner (Point mousePoint)
      {
         if (DropAdorner == null)
            this.CreateAdorner ();
         UpdateAdorner (mousePoint);
      }

      /// <summary>
      /// Creates the drop adorner.
      /// </summary>
      protected virtual void CreateAdorner ()
      {
         if (this.DropTarget == null) return;
         var template = DragNDrop.GetDropAdornerTemplate (this.DropTarget);
         if (template == null) return;
         this.DropAdorner = new DataTemplateAdorner (this.DropTarget, template, this.DragController.DragData);
      }

      /// <summary>
      /// Occurs when the drop target changes.
      /// </summary>
      protected virtual void DropTargetChanged ()
      {
      }

      /// <summary>
      /// Occurs when a drag is over the drop target.
      /// </summary>
      /// <param name="sender">Drop target</param>
      /// <param name="e">DragEventArgs</param>
      private void DropTargetPreviewDragOver (object sender, DragEventArgs e)
      {
         Scroll (this.DropTarget, e);
         Point realMousePosition = this.DropTarget.RealMousePosition ();
         bool okToDrop = this.OkToDrop (realMousePosition);
         if (okToDrop)
            this.ShowAdorner (realMousePosition);
         else
            this.RemoveAdorner ();

         e.Effects = okToDrop ? DragDropEffects.All : DragDropEffects.None;
         this.DragOver (e);
         e.Handled = true;
      }

      /// <summary>
      /// Occurs when a drag is over the drop target.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void DropTargetPreviewDragEnter (object sender, DragEventArgs e)
      {
         this.DragEnter (e);
         e.Handled = true;
      }

      /// <summary>
      /// Sets the events for the drop target.
      /// </summary>
      private void SetEvents (bool add)
      {
         if (add)
         {
            this.m_DropTarget.DragOver += s_DropTargetDragOver;
            this.m_DropTarget.DragEnter += s_DropTargetDragOver;

            this.m_DropTarget.PreviewDragEnter += DropTargetPreviewDragEnter;
            this.m_DropTarget.PreviewDragOver += DropTargetPreviewDragOver;
            this.m_DropTarget.PreviewDrop += DropTargetPreviewDrop;
            this.m_DropTarget.PreviewDragLeave += DropTargetPreviewDragLeave;
            this.DropTargetChanged ();
         }
         else
         {
            this.m_DropTarget.DragOver -= s_DropTargetDragOver;
            this.m_DropTarget.DragEnter -= s_DropTargetDragOver;

            this.m_DropTarget.PreviewDragEnter -= DropTargetPreviewDragEnter;
            this.m_DropTarget.PreviewDragOver -= DropTargetPreviewDragOver;
            this.m_DropTarget.PreviewDrop -= DropTargetPreviewDrop;
            this.m_DropTarget.PreviewDragLeave -= DropTargetPreviewDragLeave;
         }
      }

      /// <summary>
      /// Occurs when a drop happens over the drop target.
      /// </summary>
      /// <param name="sender">DropTarget</param>
      /// <param name="e">DragEventArgs</param>
      private void DropTargetPreviewDrop (object sender, DragEventArgs e)
      {
         this.Drop (this.DropTarget.RealMousePosition ());
         this.CancelDrop ();
      }

      /// <summary>
      /// Occurs when a drag leaves the drop target.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void DropTargetPreviewDragLeave (object sender, DragEventArgs e)
      {
         e.Handled = true;
         Point pos = this.DropTarget.RealMousePosition ();
         if (pos.X < 0 || pos.Y < 0 || pos.X > DropTarget.RenderSize.Width || pos.Y > DropTarget.RenderSize.Height)
         {
            this.DragLeaving (e);
            this.CancelDrop ();
         }
      }

      /// <summary>
      /// Used to scroll while dragging and item.
      /// </summary>
      /// <param name="o">DependencyObject that needs scrolling.</param>
      /// <param name="e">DragEventArgs</param>
      private static void Scroll (DependencyObject o, DragEventArgs e)
      {
         var scrollViewer = o.GetVisualDescendent<ScrollViewer> ();

         if (scrollViewer != null)
         {
            Point position = e.GetPosition (scrollViewer);
            double scrollMargin = Math.Min (scrollViewer.FontSize * 2, scrollViewer.ActualHeight / 2);

            if (position.X >= scrollViewer.ActualWidth - scrollMargin &&
                scrollViewer.HorizontalOffset < scrollViewer.ExtentWidth - scrollViewer.ViewportWidth)
               scrollViewer.LineRight ();
            else if (position.X < scrollMargin && scrollViewer.HorizontalOffset > 0)
               scrollViewer.LineLeft ();
            else if (position.Y >= scrollViewer.ActualHeight - scrollMargin &&
                     scrollViewer.VerticalOffset < scrollViewer.ExtentHeight - scrollViewer.ViewportHeight)
               scrollViewer.LineDown ();
            else if (position.Y < scrollMargin && scrollViewer.VerticalOffset > 0)
               scrollViewer.LineUp ();
         }
      }

      #endregion
   }
}
