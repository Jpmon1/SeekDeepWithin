using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
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
      public virtual ICollection<GlossaryItem> Items { get; set; }

      /// <summary>
      /// Get or Sets the list of tags.
      /// </summary>
      public virtual ICollection<GlossaryTermTag> Tags { get; set; }

      /// <summary>
      /// Get or Sets the list of see also.
      /// </summary>
      public virtual ICollection<GlossarySeeAlso> SeeAlsos { get; set; }

      /// <summary>
      /// Get or Sets the list of see also.
      /// </summary>
      public virtual ICollection<GlossaryAmazonItem> AmazonItems { get; set; }
   }
}
