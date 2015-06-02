using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a link between a version and a sub book.
   /// </summary>
   public class VersionSubBook : IDbTable
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets if this sub book should be hidden or not.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets or Sets the order of the sub book.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets an alias for this sub book.
      /// </summary>
      public string Alias { get; set; }

      /// <summary>
      /// Gets or Sets the associated version
      /// </summary>
      public virtual Version Version { get; set; }

      /// <summary>
      /// Gets or Sets the associated sub book
      /// </summary>
      public virtual SubBook SubBook { get; set; }

      /// <summary>
      /// Gets or Sets the list of chapters.
      /// </summary>
      public virtual ICollection<SubBookChapter> Chapters { get; set; }
   }
}