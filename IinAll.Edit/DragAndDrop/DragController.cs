/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System;
using System.Windows;
using System.Windows.Controls;
using IinAll.Edit.DragAndDrop.Adorners;

namespace IinAll.Edit.DragAndDrop
{
   /// <summary>
   /// Base controller for drag operations.
   /// </summary>
   public class DragController
   {
      #region Fields

      /// <summary>
      /// Event used when the drag source is changed.
      /// </summary>
      public event EventHandler DragSourceChanged;

      /// <summary>
      /// Event used when the drag is completed.
      /// </summary>
      public event EventHandler DragCompleted;

      /// <summary>
      /// Drag source container.
      /// </summary>
      private UIElement m_DragSource;

      /// <summary>
      /// Element being dragged.
      /// </summary>
      private UIElement m_OriginalDragElement;

      #endregion

      #region Properties

      /// <summary>
      /// Gets or Sets the drag source container.
      /// </summary>
      public UIElement DragSource
      {
         get { return this.m_DragSource; }
         internal set
         {
            if (this.m_DragSource != null) this.SetEvents (false);
            this.m_DragSource = value;
            if (this.m_DragSource != null) this.SetEvents (true);
         }
      }

      /// <summary>
      /// Gets or Sets the original drag element.
      /// </summary>
      public UIElement OriginalDragElement
      {
         get { return this.m_OriginalDragElement; }
         internal set
         {
            this.m_OriginalDragElement = value;
            this.SetDragData ();
         }
      }

      /// <summary>
      /// Gets or Sets the string representation of the original drag element type.
      /// </summary>
      public string OriginalDragElementType { get; internal set; }

      /// <summary>
      /// Gets or Sets the start drag point.
      /// </summary>
      public Point StartPosition { get; internal set; }

      /// <summary>
      /// Gets the dragging object.
      /// </summary>
      public virtual object DragData { get; protected set; }

      /// <summary>
      /// Gets the attached drag adorner.
      /// </summary>
      public DragNDropAdorner DragAdorner { get; private set; }

      /// <summary>
      /// Gets the drop controller.
      /// </summary>
      public DropController DropController { get; internal set; }

      #endregion

      #region Methods

      /// <summary>
      /// Ends the drag operation.
      /// </summary>
      public void CancelDrag ()
      {
         this.RemoveAdorner ();
         this.DragSource = null;

         if (this.DragCompleted != null)
            this.DragCompleted (this, new EventArgs ());
      }

      /// <summary>
      /// Removes the drag adorner.
      /// </summary>
      public void RemoveAdorner ()
      {
         if (this.DragAdorner != null)
         {
            this.DragAdorner.Detach ();
            this.DragAdorner = null;
         }
      }

      /// <summary>
      /// Shows the adorner.
      /// </summary>
      /// <param name="e"></param>
      internal void ShowAdorner (DragEventArgs e)
      {
         if (this.DragAdorner == null)
            this.CreateDragAdorner ();
         if (this.DragAdorner != null)
            this.DragAdorner.Position = e.GetPosition (this.DragSource);
      }

      /// <summary>
      /// Creates the desired drag adorner.
      /// </summary>
      protected virtual void CreateDragAdorner ()
      {
         if (this.DragSource == null) return;
         var template = DragNDrop.GetDragAdornerTemplate (this.DragSource);
         if (template == null) return;
         this.DragAdorner = new DataTemplateAdorner (this.DragSource, template, this.DragData);
      }

      /// <summary>
      /// Sets the drag data object.
      /// </summary>
      protected virtual void SetDragData ()
      {
         var itemsControl = this.DragSource as ItemsControl;
         if (itemsControl != null)
         {
            var itemContainer = itemsControl.GetItemContainer (this.OriginalDragElement);
            if (itemContainer != null)
            {
               this.DragData = itemsControl.ItemContainerGenerator.ItemFromContainer (itemContainer);
               return;
            }
         }

         this.DragData = null;
      }

      /// <summary>
      /// Gives feedback during a drag operation.
      /// </summary>
      /// <param name="sender">Drag source.</param>
      /// <param name="e">GiveFeedbackEventArgs</param>
      protected virtual void DragSourceGiveFeedback (object sender, GiveFeedbackEventArgs e)
      {
         if (this.DragAdorner != null)
         {
            e.UseDefaultCursors = false;
            e.Handled = true;
         }
      }

      /// <summary>
      /// Sets or removes the events for the drag source.
      /// </summary>
      /// <param name="add">True to add events, false to remove.</param>
      private void SetEvents (bool add)
      {
         if (add)
         {
            this.m_DragSource.GiveFeedback += DragSourceGiveFeedback;
            if (this.DragSourceChanged != null)
               this.DragSourceChanged (this, new EventArgs ());
         }
         else
            this.m_DragSource.GiveFeedback -= DragSourceGiveFeedback;
      }

      /// <summary>
      /// Called before a drag starts. Creates the drag adorner.
      /// </summary>
      internal bool DragStarting ()
      {
         this.CreateDragAdorner ();
         return this.DragStarted ();
      }

      /// <summary>
      /// Called before a drag starts, override to do something special.
      /// </summary>
      /// <returns></returns>
      public virtual bool DragStarted ()
      {
         return true;
      }

      #endregion
   }
}
