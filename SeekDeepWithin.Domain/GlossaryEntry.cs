using System.Collections.Generic;

namespace SeekDeepWithin.Domain
{
   public class GlossaryEntry : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the number of votes this entry has.
      /// </summary>
      public int Votes { get; set; }

      /// <summary>
      /// Gets or Sets the text of this entry.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Get or Sets the parent glossary term.
      /// </summary>
      public virtual GlossaryTerm GlossaryTerm { get; set; }

      /// <summary>
      /// Get or Sets the source of this entry.
      /// </summary>
      public virtual ICollection<GlossaryEntrySource> GlossaryEntrySources { get; set; }
   }
}
