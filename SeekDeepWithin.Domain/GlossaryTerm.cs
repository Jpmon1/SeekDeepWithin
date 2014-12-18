using System.Collections.Generic;

namespace SeekDeepWithin.Domain
{
   public class GlossaryTerm : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Get or Sets name of this item.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Get or Sets the list of entries.
      /// </summary>
      public virtual ICollection<GlossaryEntry> Entries { get; set; }

      /// <summary>
      /// Get or Sets the list of tags.
      /// </summary>
      public virtual ICollection<GlossaryTermTag> GlossaryTermTags { get; set; }
   }
}
