using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Peter.Common.Controls
{
   /// <summary>
   /// Represents a highlighted item for the insight bar.
   /// </summary>
   public class InsightHighlight
   {
      private readonly List <Line> m_LineShapes;
      private readonly ObservableCollection<InsightLine> m_Lines;

      /// <summary>
      /// Initializes a new insight highlight.
      /// </summary>
      public InsightHighlight ()
      {
         this.Thickness = 1;
         this.Color = Brushes.Blue;
         this.m_LineShapes = new List <Line> ();
         this.m_Lines = new ObservableCollection<InsightLine> ();
      }

      /// <summary>
      /// Gets or Sets the color of the highlight.
      /// </summary>
      public Brush Color { get; set; }

      /// <summary>
      /// The thickness of the line to draw.
      /// </summary>
      public double Thickness { get; set; }

      /// <summary>
      /// Gets the collection of lines to highlight.
      /// </summary>
      public ObservableCollection<InsightLine> Lines { get { return this.m_Lines; } }

      /// <summary>
      /// Gets the internal list of line shapes.
      /// </summary>
      internal List<Line> LineShapes { get { return this.m_LineShapes; } }

      /// <summary>
      /// Gets the insight bar.
      /// </summary>
      internal InsightBar InsightBar { get; set; }

      /// <summary>
      /// Redraws the lines in this group.
      /// </summary>
      public void Redraw ()
      {
         this.InsightBar.Redraw (this);
      }
   }

   /// <summary>
   /// Represents a insight line.
   /// </summary>
   public class InsightLine
   {
      /// <summary>
      /// Initializes a new insight line.
      /// </summary>
      /// <param name="line"></param>
      /// <param name="column"></param>
      public InsightLine (int line, int column)
      {
         this.LineNumber = line;
         this.Column = column;
      }

      /// <summary>
      /// The number of the line.
      /// </summary>
      public int LineNumber { get; set; }

      /// <summary>
      /// The column on the line.
      /// </summary>
      public int Column { get; set; }
   }
}
