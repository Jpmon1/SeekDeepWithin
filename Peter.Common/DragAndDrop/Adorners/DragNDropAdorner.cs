/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Peter.Common.DragAndDrop.Adorners
{
   /// <summary>
   /// Base adorner class for drag and drop.
   /// </summary>
   public abstract class DragNDropAdorner : Adorner
   {
      private Point m_MousePosition;

      /// <summary>
      /// Initializes a new drag and drop adorner.
      /// </summary>
      /// <param name="adornedElement">Element hosting adorner.</param>
      protected DragNDropAdorner (UIElement adornedElement)
         : base (adornedElement)
      {
         this.AdornerLayer = AdornerLayer.GetAdornerLayer (adornedElement);
         this.AdornerLayer.Add (this);
         this.IsHitTestVisible = false;
      }

      /// <summary>
      /// Gets the adorner layer.
      /// </summary>
      public AdornerLayer AdornerLayer { get; private set; }

      /// <summary>
      /// Gets the number of visual child elements within this element.
      /// </summary>
      protected override int VisualChildrenCount { get { return 1; } }

      /// <summary>
      /// Gets the adornment to display.
      /// </summary>
      protected abstract UIElement Adornment { get; }

      /// <summary>
      /// Gets or Sets the position of the adorner.
      /// </summary>
      public Point Position
      {
         get { return this.m_MousePosition; }
         set
         {
            if (this.m_MousePosition != value)
            {
               this.m_MousePosition = value;
               this.AdornerLayer.Update (this.AdornedElement);
            }
         }
      }

      /// <summary>
      /// Removes the adorner layer for the adorned element.
      /// </summary>
      public void Detach ()
      {
         this.AdornerLayer.Remove (this);
      }

      /// <summary>
      /// Gets the adornment. 
      /// </summary>
      /// <param name="index">Index of the child to get.</param>
      /// <returns>The adornment.</returns>
      protected override Visual GetVisualChild (int index)
      {
         return this.Adornment;
      }

      /// <summary>
      /// Measures the adornment.
      /// </summary>
      /// <param name="constraint">Size constraint.</param>
      /// <returns>Desired size.</returns>
      protected override Size MeasureOverride (Size constraint)
      {
         if (this.Adornment != null)
         {
            this.Adornment.Measure (constraint);
            return this.Adornment.DesiredSize;
         }

         return new Size();
      }

      /// <summary>
      /// Arranges the adornment.
      /// </summary>
      /// <param name="finalSize">Arranges the adornment.</param>
      /// <returns>Final size.</returns>
      protected override Size ArrangeOverride (Size finalSize)
      {
         if (this.Adornment != null)
            this.Adornment.Arrange (new Rect( (finalSize)));

         return finalSize;
      }

      /// <summary>
      /// Overriding to add positioning transform so VisualChild is rendered at correct location.
      /// </summary>
      public override GeneralTransform GetDesiredTransform (GeneralTransform transform)
      {
         var generalTransformGroup = new GeneralTransformGroup ();
         generalTransformGroup.Children.Add (base.GetDesiredTransform (transform));
         generalTransformGroup.Children.Add (new TranslateTransform (this.Position.X, this.Position.Y));

         return generalTransformGroup;
      }
   }
}
