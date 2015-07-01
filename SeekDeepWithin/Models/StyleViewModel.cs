using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for a style
   /// </summary>
   public class StyleViewModel
   {
      public StyleViewModel (IStyle style)
      {
         this.Id = style.Id;
         this.Start = style.Style.Start;
         this.StartIndex = style.StartIndex;
         this.End = style.Style.End;
         this.EndIndex = style.EndIndex;
         this.SpansMultiple = style.Style.SpansMultiple;
         this.StyleId = style.Style.Id;
      }

      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the start of the style.
      /// </summary>
      public string Start { get; set; }

      /// <summary>
      /// Gets or Sets if this style spans multiple items (entries/passages).
      /// </summary>
      public bool SpansMultiple { get; set; }

      /// <summary>
      /// Gets or Sets the end of the style.
      /// </summary>
      public string End { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the style.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the style.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets the id of the style
      /// </summary>
      public int StyleId { get; set; }
   }
}