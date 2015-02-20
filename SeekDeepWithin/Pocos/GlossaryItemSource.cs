﻿namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a source for a glossary entry.
   /// </summary>
   public class GlossaryItemSource : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the entry source.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the version.
      /// </summary>
      public virtual GlossaryItem GlossaryItem { get; set; }

      /// <summary>
      /// Gets or Sets the source.
      /// </summary>
      public virtual Source Source { get; set; }
   }
}