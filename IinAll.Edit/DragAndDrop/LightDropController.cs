using System.Collections;
using System.Windows;
using System.Windows.Controls;
using IinAll.Edit.Data;

namespace IinAll.Edit.DragAndDrop
{
   public class LightDropController : DropController
   {
      /// <summary>
      /// Handles the drop operation.
      /// </summary>
      /// <param name="mousePoint">Position of mouse relative to drop target.</param>
      protected override void Drop (Point mousePoint)
      {
         // This should handle most items control drops...
         if (this.DragController.DragData == null) return;
         var newItemsControl = this.DropTarget as ItemsControl;
         var oldItemsControl = this.DragController.DragSource as ItemsControl;
         if (newItemsControl != null && oldItemsControl != null)
         {
            var newItems = newItemsControl.ItemsSource as IList ?? newItemsControl.Items;
            if (newItems == null) return;
            var oldLight = this.DragController.DragData as Light;
            if (oldLight != null) {
               var newLight = new Light {
                  Id = oldLight.Id,
                  Text = oldLight.Text
               };
               newItems.Add (newLight);
            }
         }
      }
   }
}
