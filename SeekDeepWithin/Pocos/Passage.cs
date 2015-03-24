using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class Passage : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the passage.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the text of the passage.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets the list of entries this passage is in.
      /// </summary>
      public virtual ICollection<PassageEntry> Entries { get; set; }

      /// <summary>
      /// Gets the list of links for this passage.
      /// </summary>
      public virtual ICollection<PassageLink> Links { get; set; }
   }
}
