using System;
using System.Windows;
using System.Windows.Controls;
using IinAll.Edit.Data;

namespace IinAll.Edit.DragAndDrop
{
   public class LightTextDropController : DropController
   {
      /// <summary>
      /// Checks if a drop can happen or not.
      /// </summary>
      /// <param name="mousePoint">Position of mouse relative to drop target.</param>
      /// <returns>True if we can drop, otherwise false.</returns>
      public override bool OkToDrop (Point mousePoint)
      {
         return this.DropTarget is TextBox;
      }

      /// <summary>
      /// Handles the drop operation.
      /// </summary>
      /// <param name="mousePoint">Position of mouse relative to drop target.</param>
      protected override void Drop (Point mousePoint)
      {
         // This should handle most items control drops...
         if (this.DragController.DragData == null) return;
         var textBox = this.DropTarget as TextBox;
         if (textBox != null) {
            var light = this.DragController.DragData as Light;
            if (light != null) {
               var index = Math.Max (0, textBox.CaretIndex);
               textBox.Text = textBox.Text.Insert (index, light.Text);
            }
         }
      }
   }
}
