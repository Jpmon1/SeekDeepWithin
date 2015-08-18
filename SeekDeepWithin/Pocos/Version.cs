using System;
using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a version.
   /// </summary>
   public class Version : IDbTable
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets if this version should be hidden or not.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets or Sets the contents of this version.
      /// </summary>
      public string Contents { get; set; }

      /// <summary>
      /// Gets or Sets the title of the version.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or Sets the date the version was published.
      /// </summary>
      public string PublishDate { get; set; }

      /// <summary>
      /// Gets or Sets the chapter id to read when opeing a version.
      /// </summary>
      public int DefaultReadChapter { get; set; }

      /// <summary>
      /// Gets or Sets the last time this was modified.
      /// </summary>
      public DateTime Modified { get; set; }

      /// <summary>
      /// Gets or Sets the book of the version
      /// </summary>
      public virtual Book Book { get; set; }

      /// <summary>
      /// Gets or Sets the associated term.
      /// </summary>
      public virtual Term Term { get; set; }

      /// <summary>
      /// Gets or Sets the name of this source.
      /// </summary>
      public string SourceName { get; set; }

      /// <summary>
      /// Gets or Sets the url for the source.
      /// </summary>
      public string SourceUrl { get; set; }

      /// <summary>
      /// Gets or Sets the list of subbooks.
      /// </summary>
      public virtual ICollection<VersionSubBook> SubBooks { get; set; }
   }
}
