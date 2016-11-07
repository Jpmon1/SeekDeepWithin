using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace Peter.Common.Controls
{
   /// <summary>
   /// The side insight bar for highlighting items in a document.
   /// </summary>
   public class InsightBar : Canvas
   {
      /// <summary>
      /// Initializes an insight bar with default values.
      /// </summary>
      public InsightBar ()
      {
         this.Width = 15;
      }

      /// <summary>
      /// Event for requesting navigation.
      /// </summary>
      public static readonly RoutedEvent NavigationRequestedEvent = EventManager.RegisterRoutedEvent (
         "NavigationRequested", RoutingStrategy.Bubble, typeof (RoutedEventHandler), typeof (InsightBar));

      /// <summary>
      /// Event Handler for navigation requested.
      /// </summary>
      public event RoutedEventHandler NavigationRequested
      {
         add { AddHandler (NavigationRequestedEvent, value); }
         remove { RemoveHandler (NavigationRequestedEvent, value); }
      }

      /// <summary>
      /// Dependency property for total lines.
      /// </summary>
      public static readonly DependencyProperty TotalLinesProperty = DependencyProperty.Register (
         "TotalLines", typeof (int), typeof (InsightBar),
         new PropertyMetadata (0, OnTotalLinesChanged));

      /// <summary>
      /// Gets or Sets the total number of lines in the attached document.
      /// </summary>
      public int TotalLines
      {
         get { return (int) GetValue (TotalLinesProperty); }
         set { SetValue (TotalLinesProperty, value); }
      }

      /// <summary>
      /// Occurs when the total number of lines changes in the attached document.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnTotalLinesChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var insight = d as InsightBar;
         if (insight != null)
         {
            insight.Redraw ();
         }
      }

      /// <summary>
      /// Dependency property for highlights.
      /// </summary>
      public static readonly DependencyProperty InsightHighlightsProperty = DependencyProperty.Register (
         "InsightHighlights", typeof (IEnumerable <InsightHighlight>), typeof (InsightBar),
         new PropertyMetadata (default(IEnumerable <InsightHighlight>), OnHighLightsChanged));

      /// <summary>
      /// Gets or Sets the insight highlights.
      /// </summary>
      public IEnumerable <InsightHighlight> InsightHighlights
      {
         get { return (IEnumerable <InsightHighlight>) GetValue (InsightHighlightsProperty); }
         set { SetValue (InsightHighlightsProperty, value); }
      }

      /// <summary>
      /// Occurs when the highlights change.
      /// </summary>
      /// <param name="d">DependencyObject</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnHighLightsChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var insight = d as InsightBar;
         if (insight != null)
         {
            if (e.OldValue != null)
            {
               foreach (InsightHighlight highlight in (IEnumerable <InsightHighlight>) e.OldValue)
               {
                  foreach (var lineShape in highlight.LineShapes)
                  {
                     lineShape.MouseUp -= insight.OnLineClick;
                     insight.Children.Remove (lineShape);
                  }
                  highlight.LineShapes.Clear ();
                  highlight.InsightBar = null;
               }
            }
            if (e.NewValue != null)
            {
               var enumerable = (IEnumerable <InsightHighlight>) e.NewValue;
               foreach (var highlight in enumerable)
                  highlight.InsightBar = insight;

               var coll = enumerable as ObservableCollection<InsightHighlight>;
               if (coll != null)
                  coll.CollectionChanged += insight.OnHighLightsCollectionChanged;
               insight.Redraw();
            }
         }
      }

      private void OnHighLightsCollectionChanged (object sender, NotifyCollectionChangedEventArgs e)
      {
         foreach (InsightHighlight highlight in e.OldItems)
            highlight.InsightBar = null;
         foreach (InsightHighlight highlight in e.NewItems)
            highlight.InsightBar = this;
      }

      /// <summary>
      /// Redraws all line highlight.
      /// </summary>
      public void Redraw ()
      {
         if (this.InsightHighlights != null)
         {
            foreach (var highlight in this.InsightHighlights)
               Redraw (highlight);
         }
      }

      /// <summary>
      /// Redraws the given highlight.
      /// </summary>
      /// <param name="insightHighlight">The highlight to redraw.</param>
      public void Redraw (InsightHighlight insightHighlight)
      {
         var lineHeight = this.ActualHeight / this.TotalLines;
         if (double.IsInfinity (lineHeight) || double.IsNaN (lineHeight))
            return;
         foreach (var lineShape in insightHighlight.LineShapes)
         {
            lineShape.MouseUp -= this.OnLineClick;
            this.Children.Remove(lineShape);
         }
         insightHighlight.LineShapes.Clear();
         foreach (var line in insightHighlight.Lines)
         {
            var top = lineHeight * (line.LineNumber - 1);
            var lineShape = new Line
            {
               X1 = 0,
               X2 = this.ActualWidth,
               Y1 = top,
               Y2 = top,
               Tag = line,
               Cursor = Cursors.Hand,
               Stroke = insightHighlight.Color,
               HorizontalAlignment = HorizontalAlignment.Left,
               VerticalAlignment = VerticalAlignment.Center,
               StrokeThickness = insightHighlight.Thickness
            };
            lineShape.MouseUp += this.OnLineClick;
            insightHighlight.LineShapes.Add (lineShape);
            this.Children.Add (lineShape);
         }
      }

      /// <summary>
      /// Occurs when a line highlight is clicked.
      /// </summary>
      /// <param name="sender">Line that was clicked.</param>
      /// <param name="e">MouseButtonEventArgs</param>
      private void OnLineClick (object sender, MouseButtonEventArgs e)
      {
         var line = sender as Line;
         if (line != null)
         {
            var data = line.Tag as InsightLine;
            this.RaiseNavigationEvent (data);
         }
      }

      void RaiseNavigationEvent (InsightLine line)
      {
         var newEventArgs = new RoutedEventArgs (InsightBar.NavigationRequestedEvent, line);
         RaiseEvent (newEventArgs);
      }
   }
}
