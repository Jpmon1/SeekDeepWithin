﻿namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a style for a peace.
   /// </summary>
   public class LightStyle
   {
      /// <summary>
      /// Gets or Sets the id.
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
      /// Gets or Sets the parent light.
      /// </summary>
      public virtual Light Light { get; set; }
   }
}