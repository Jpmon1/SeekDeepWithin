using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class PassageEntry : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the passage entry.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the number of the passage entry.
      /// </summary>
      public int Number { get; set; }

      /// <summary>
      /// Gets or Sets the order of the passage entry.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the chapter of this entry.
      /// </summary>
      public virtual SubBookChapter Chapter { get; set; }

      /// <summary>
      /// Gets or Sets the passage of this entry.
      /// </summary>
      public virtual Passage Passage { get; set; }

      /// <summary>
      /// Gets the header for this passage.
      /// </summary>
      public virtual PassageHeader Header { get; set; }

      /// <summary>
      /// Gets the list of footers for this passage.
      /// </summary>
      public virtual ICollection<PassageFooter> Footers { get; set; }

      /// <summary>
      /// Gets the list of footers for this passage.
      /// </summary>
      public virtual ICollection<PassageStyle> Styles { get; set; }
   }
}
