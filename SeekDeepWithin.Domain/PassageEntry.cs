using System.Collections.Generic;

namespace SeekDeepWithin.Domain
{
   public class PassageEntry : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the passage entry.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the passage id.
      /// </summary>
      public int PassageId { get; set; }

      /// <summary>
      /// Gets or Sets the chapter id.
      /// </summary>
      public int ChapterId { get; set; }

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
      public virtual Chapter Chapter { get; set; }

      /// <summary>
      /// Gets or Sets the passage of this entry.
      /// </summary>
      public virtual Passage Passage { get; set; }

      /// <summary>
      /// Gets the list of headers for this passage.
      /// </summary>
      public virtual ICollection<PassageHeader> Headers { get; set; }

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
