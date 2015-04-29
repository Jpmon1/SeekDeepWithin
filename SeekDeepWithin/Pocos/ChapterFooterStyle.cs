﻿namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Style for a chapter footer.
   /// </summary>
   public class ChapterFooterStyle : IStyle
   {
      /// <summary>
      /// Gets or Sets the id of the entry style.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the style.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the style.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets the style.
      /// </summary>
      public virtual Style Style { get; set; }

      /// <summary>
      /// Gets or Sets the footer.
      /// </summary>
      public virtual ChapterFooter Footer { get; set; }
   }
}