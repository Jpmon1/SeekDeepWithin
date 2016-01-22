using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   public class SdwStyle
   {
      /// <summary>
      /// Initializes a new style.
      /// </summary>
      /// <param name="style"></param>
      public SdwStyle (TruthStyle style)
      {
         this.StartIndex = style.StartIndex;
         this.EndIndex = style.EndIndex;
         this.Start = style.Style.Start;
         this.End = style.Style.End;
      }

      /// <summary>
      /// Gets the start style.
      /// </summary>
      public string Start { get; private set; }

      /// <summary>
      /// Gets the end style
      /// </summary>
      public string End { get; private set; }

      /// <summary>
      /// Gets the starting index.
      /// </summary>
      public int StartIndex { get; private set; }

      /// <summary>
      /// Gets the ending index.
      /// </summary>
      public int EndIndex { get; private set; }
   }
}