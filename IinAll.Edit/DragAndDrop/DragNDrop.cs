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
using System.Windows.Input;

namespace IinAll.Edit.DragAndDrop
{
   /// <summary>
   /// Properties to perform a drag and drop operation.
   /// </summary>
   public static class DragNDrop
   {
      #region Fields

      private static DragController s_DragController;
      private static readonly DataFormat s_Format = DataFormats.GetDataFormat ("IinAll.DragAndDrop");

      #endregion

      #region Attached Properties

      /// <summary>
      /// Attached Property for a custom drag adorner.
      /// </summary>
      public static readonly DependencyProperty DragAdornerTemplateProperty =
         DependencyProperty.RegisterAttached ("DragAdornerTemplate", typeof (DataTemplate), typeof (DragNDrop));

      /// <summary>
      /// Gets the drag adorner template.
      /// </summary>
      /// <param name="target">UI element to get adorner from.</param>
      /// <returns>Adorner's data template.</returns>
      public static DataTemplate GetDragAdornerTemplate (UIElement target)
      {
         return (DataTemplate) target.GetValue (DragAdornerTemplateProperty);
      }

      /// <summary>
      /// Sets the drag adorner for the given UI element.
      /// </summary>
      /// <param name="target"></param>
      /// <param name="value"></param>
      public static void SetDragAdornerTemplate (UIElement target, DataTemplate value)
      {
         target.SetValue (DragAdornerTemplateProperty, value);
      }

      /// <summary>
      /// Attached Property for a custom drop adorner.
      /// </summary>
      public static readonly DependencyProperty DropAdornerTemplateProperty =
         DependencyProperty.RegisterAttached ("DropAdornerTemplate", typeof (DataTemplate), typeof (DragNDrop));

      /// <summary>
      /// Gets the drop adorner template.
      /// </summary>
      /// <param name="target">UI element to get adorner from.</param>
      /// <returns>Adorner's data template.</returns>
      public static DataTemplate GetDropAdornerTemplate (UIElement target)
      {
         return (DataTemplate)target.GetValue (DropAdornerTemplateProperty);
      }

      /// <summary>
      /// Sets the drag adorner for the given UI element.
      /// </summary>
      /// <param name="target"></param>
      /// <param name="value"></param>
      public static void SetDropAdornerTemplate (UIElement target, DataTemplate value)
      {
         target.SetValue (DropAdornerTemplateProperty, value);
      }

      /// <summary>
      /// Attached Property for drag dontroller if a custom one is needed.
      /// </summary>
      public static readonly DependencyProperty DragControllerProperty =
         DependencyProperty.RegisterAttached ("DragController", typeof (DragController), typeof (DragNDrop));

      /// <summary>
      /// Gets the drag controller for a drag and drop operation.
      /// </summary>
      /// <param name="target">UI Element to get controller for.</param>
      /// <returns>Drag controller.</returns>
      public static DragController GetDragController (UIElement target)
      {
         return (DragController)target.GetValue (DragControllerProperty);
      }

      /// <summary>
      /// Sets the drag controller for a drag and drop operation.
      /// </summary>
      /// <param name="target">UI Element to set controller for.</param>
      /// <param name="value">Drag controller to use.</param>
      public static void SetDragController (UIElement target, DragController value)
      {
         target.SetValue (DragControllerProperty, value);
      }

      /// <summary>
      /// Attached Property for drop controller if a custom one is needed.
      /// </summary>
      public static readonly DependencyProperty DropControllerProperty =
         DependencyProperty.RegisterAttached ("DropController", typeof (DropController), typeof (DragNDrop));

      /// <summary>
      /// Gets the drop handler for a drag and drop operation.
      /// </summary>
      /// <param name="target">UI Element to get handler for.</param>
      /// <returns>Drop handler.</returns>
      public static DropController GetDropController (UIElement target)
      {
         return (DropController)target.GetValue (DropControllerProperty);
      }

      /// <summary>
      /// Sets the drop handler for a drag and drop operation.
      /// </summary>
      /// <param name="target">UI Element to set handler for.</param>
      /// <param name="value">Drop handler to use.</param>
      public static void SetDropController (UIElement target, DropController value)
      {
         target.SetValue (DropControllerProperty, value);
      }

      /// <summary>
      /// Attached Property for specifying if a UIElement is a drag source.
      /// </summary>
      public static readonly DependencyProperty IsDragSourceProperty =
         DependencyProperty.RegisterAttached ("IsDragSource", typeof (bool), typeof (DragNDrop),
                                              new UIPropertyMetadata (false, IsDragSourceChanged));

      /// <summary>
      /// Gets if the element is a source for drag operations.
      /// </summary>
      /// <param name="target">UI element to check for drag source property.</param>
      /// <returns>True if element is a drag source, otherwise false.</returns>
      public static bool GetIsDragSource (UIElement target)
      {
         return (bool) target.GetValue (IsDragSourceProperty);
      }

      /// <summary>
      /// Sets if the element is a source for drag operations.
      /// </summary>
      /// <param name="target">Element to set is drag source property on.</param>
      /// <param name="value">True if element is a drag source, otherwise false.</param>
      public static void SetIsDragSource (UIElement target, bool value)
      {
         target.SetValue (IsDragSourceProperty, value);
      }

      /// <summary>
      /// Attached Property for specifying if a UIElement is a drop target.
      /// </summary>
      public static readonly DependencyProperty IsDropTargetProperty =
         DependencyProperty.RegisterAttached ("IsDropTarget", typeof (bool), typeof (DragNDrop),
                                              new UIPropertyMetadata (false, IsDropTargetChanged));

      /// <summary>
      /// Gets if the element is a target for drop operations.
      /// </summary>
      /// <param name="target">UI element to check for drop target property.</param>
      /// <returns>True if element is a drop target, otherwise false.</returns>
      public static bool GetIsDropTarget (UIElement target)
      {
         return (bool) target.GetValue (IsDropTargetProperty);
      }

      /// <summary>
      /// Sets if the element is a target for drop operations.
      /// </summary>
      /// <param name="target">Element to set is drop target property on.</param>
      /// <param name="value">True if element is a drop target, otherwise false.</param>
      public static void SetIsDropTarget (UIElement target, bool value)
      {
         target.SetValue (IsDropTargetProperty, value);
      }

      #endregion

      #region Property Change

      /// <summary>
      /// Occurs when the drag source property changes.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void IsDragSourceChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var dragSource = d as UIElement;
         if (dragSource != null)
         {
            if ((bool) e.NewValue)
            {
               dragSource.PreviewMouseLeftButtonDown += DragSourcePreviewMouseLeftButtonDown;
               dragSource.PreviewMouseLeftButtonUp += DragSourcePreviewMouseLeftButtonUp;
               dragSource.PreviewMouseMove += DragSourcePreviewMouseMove;
            }
            else
            {
               dragSource.PreviewMouseLeftButtonDown -= DragSourcePreviewMouseLeftButtonDown;
               dragSource.PreviewMouseLeftButtonUp -= DragSourcePreviewMouseLeftButtonUp;
               dragSource.PreviewMouseMove -= DragSourcePreviewMouseMove;
            }
         }
      }

      /// <summary>
      /// Occurs when the drop target property changes.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void IsDropTargetChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var dropTarget = d as UIElement;
         if (dropTarget != null)
         {
            if ((bool) e.NewValue)
            {
               dropTarget.AllowDrop = true;
               dropTarget.PreviewDragEnter += DropTargetDragEnter;
            }
            else
            {
               dropTarget.AllowDrop = false;
               dropTarget.PreviewDragEnter -= DropTargetDragEnter;
            }
         }
      }

      #endregion

      #region Methods

      /// <summary>
      /// Occurs when the left mouse button is clicked over a drag source.
      /// </summary>
      /// <param name="sender">Drag source.</param>
      /// <param name="e">MouseButtonEventArgs</param>
      private static void DragSourcePreviewMouseLeftButtonDown (object sender, MouseButtonEventArgs e)
      {
         if (s_DragController == null && (Mouse.Captured == null || Mouse.Captured == sender))
         {
            var element = sender as UIElement;
            if (element != null)
            {
               var dragElement = e.OriginalSource as UIElement;
               if (dragElement != null)
               {
                  s_DragController = GetDragController (element) ?? new DragController ();
                  s_DragController.DragSource = element;
                  s_DragController.StartPosition = element.RealMousePosition ();//e.GetPosition (element);
                  s_DragController.OriginalDragElement = dragElement;
                  s_DragController.OriginalDragElementType = dragElement.GetType ().ToString ();
                  s_DragController.DragCompleted += delegate { s_DragController = null; };
               }
            }
         }
      }

      /// <summary>
      /// Occurs when the left mouse button is released.
      /// </summary>
      /// <param name="sender">Drag source.</param>
      /// <param name="e">MouseButtonEventArgs</param>
      private static void DragSourcePreviewMouseLeftButtonUp (object sender, MouseButtonEventArgs e)
      {
         if (s_DragController != null)
         {
            s_DragController.CancelDrag ();
            s_DragController = null;
         }
      }

      /// <summary>
      /// Occurs when the mouse moves over a drag source.
      /// </summary>
      /// <param name="sender">Drag source.</param>
      /// <param name="e">MouseEventArgs</param>
      private static void DragSourcePreviewMouseMove (object sender, MouseEventArgs e)
      {
         if (s_DragController != null && s_DragController.DragData != null)
         {
            Point dragStart = s_DragController.StartPosition;
            Point position = s_DragController.DragSource.RealMousePosition ();//e.GetPosition (s_DragController.OriginalDragElement);

            if (Math.Abs (position.X - dragStart.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs (position.Y - dragStart.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
               if (s_DragController.DragStarting ())
               {
                  var data = new DataObject (s_Format.Name, s_DragController.DragData);
                  Window parentWindow = Window.GetWindow (s_DragController.DragSource);
                  bool origAllowDrop = parentWindow.AllowDrop;
                  if (s_DragController.DragAdorner != null)
                  {
                     parentWindow.AllowDrop = true;
                     parentWindow.PreviewDragEnter += PreviewDragEnterHandler;
                     parentWindow.PreviewDragLeave += DragLeaveHandler;
                     //parentWindow.DragEnter += ParentWindowDrag;
                     //parentWindow.DragOver += ParentWindowDrag;
                  }
                  try
                  {
                     DragDrop.DoDragDrop (s_DragController.DragSource, data, DragDropEffects.All);
                  }
                  catch (Exception ex)
                  {
                     Console.WriteLine(ex.Message);
                  }
                  finally
                  {
                     parentWindow.AllowDrop = origAllowDrop;
                     parentWindow.PreviewDragEnter -= PreviewDragEnterHandler;
                     parentWindow.PreviewDragLeave -= DragLeaveHandler;
                     //parentWindow.DragEnter -= ParentWindowDrag;
                     //parentWindow.DragOver -= ParentWindowDrag;
                     if (s_DragController != null)
                        s_DragController.CancelDrag ();
                  }
               }
            }
         }
      }

      private static void PreviewDragEnterHandler (object sender, DragEventArgs e)
      {
         s_DragController.ShowAdorner (e);
      }

      private static void DragLeaveHandler (object sender, DragEventArgs e)
      {
         s_DragController.CancelDrag ();
      }


      /// <summary>
      /// Occurs when a drag operation enters a drop target.
      /// </summary>
      /// <param name="sender">Drop target.</param>
      /// <param name="e">DragEventArgs</param>
      private static void DropTargetDragEnter (object sender, DragEventArgs e)
      {
         if (s_DragController == null) return;
         e.Handled = true;

         var element = sender as UIElement;
         if (element != null)
         {
            if (s_DragController.DropController == null)
               s_DragController.DropController = FindDropController (element);
            else
            {
               if (s_DragController.DropController.DropTarget == e.Source) return;
               var dropController = FindDropController (element);
               if (dropController.OkToDrop (dropController.DropTarget.RealMousePosition ()))
               {
                  s_DragController.DropController.CancelDrop ();
                  s_DragController.DropController = dropController;
               }
            }
         }
      }

      /// <summary>
      /// Gets a drop controller for the given element.
      /// </summary>
      /// <param name="element">Element to get controller for.</param>
      /// <returns>Requested drop controller.</returns>
      private static DropController FindDropController (UIElement element)
      {
         var dropController = GetDropController (element) ?? new DropController ();
         dropController.DragController = s_DragController;
         dropController.DropTarget = element;
         return dropController;
      }

      #endregion
   }
}
