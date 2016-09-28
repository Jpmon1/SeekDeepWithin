/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 **/

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace IinAll.Edit.DragAndDrop.Adorners
{
   /// <summary>
   /// Basic adorner for a line in-between two objects.
   /// </summary>
   public class InsertAdorner : DragNDropAdorner
   {
      static PathGeometry s_Triangle;
      private readonly Grid m_Adornment;
      private readonly Orientation m_Orientation = Orientation.Vertical;

      /// <summary>
      /// Static constructor to create the pen and triangle.
      /// </summary>
      static InsertAdorner ()
      {
         CreateTriangle ();
      }

      /// <summary>
      /// Initializes a new drop target insert adorner.
      /// </summary>
      /// <param name="adornedElement">Element being adorned.</param>
      public InsertAdorner (UIElement adornedElement)
         : base (adornedElement)
      {
         var itemsControl = adornedElement as ItemsControl;
         if (itemsControl != null)
         {
            this.m_Orientation = itemsControl.GetItemsPanelOrientation ();
         }
         this.m_Adornment = new Grid { Opacity = 0.5 };
         this.CreateAdornment ();
      }

      /// <summary>
      /// Crates the adornment.
      /// </summary>
      private void CreateAdornment ()
      {
         this.m_Adornment.Children.Clear ();
         if (this.m_Orientation == Orientation.Vertical)
            this.m_Adornment.Width = this.Width;
         else
            this.m_Adornment.Height = this.Height;

         var startTriangle = new Path
         {
            Fill = Brushes.Gray,
            Data = s_Triangle,
            SnapsToDevicePixels = true,
            Margin = new Thickness (0)
         };
         if (this.m_Orientation == Orientation.Vertical)
            startTriangle.HorizontalAlignment = HorizontalAlignment.Left;
         else
         {
            startTriangle.VerticalAlignment = VerticalAlignment.Top;
            startTriangle.RenderTransform = new RotateTransform (90);
         }
         this.m_Adornment.Children.Add (startTriangle);

         var endTriangle = new Path
         {
            Fill = Brushes.Gray,
            Data = s_Triangle,
            SnapsToDevicePixels = true
         };

         if (m_Orientation == Orientation.Vertical)
         {
            endTriangle.HorizontalAlignment = HorizontalAlignment.Right;
            endTriangle.RenderTransformOrigin = new Point (0.5, 0);
            endTriangle.RenderTransform = new RotateTransform (180);
         }
         else
         {
            endTriangle.VerticalAlignment = VerticalAlignment.Bottom;
            endTriangle.RenderTransformOrigin = new Point (0.5, 0.5);
            endTriangle.RenderTransform = new RotateTransform (-90);
         }
         this.m_Adornment.Children.Add (endTriangle);
      }

      /// <summary>
      /// Gets the adornment to display.
      /// </summary>
      protected override UIElement Adornment
      {
         get { return this.m_Adornment; }
      }

      /// <summary>
      /// Creates the triangle's geometry.
      /// </summary>
      private static void CreateTriangle ()
      {
         const int triangleSize = 5;
         var firstLine = new LineSegment (new Point (0, -triangleSize), false);
         firstLine.Freeze ();
         var secondLine = new LineSegment (new Point (0, triangleSize), false);
         secondLine.Freeze ();
         var figure = new PathFigure {StartPoint = new Point (triangleSize, 0)};
         figure.Segments.Add (firstLine);
         figure.Segments.Add (secondLine);
         figure.Freeze ();
         s_Triangle = new PathGeometry ();
         s_Triangle.Figures.Add (figure);
         s_Triangle.Freeze ();
      }
   }
}
