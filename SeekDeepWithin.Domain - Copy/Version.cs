using System.Collections.Generic;

namespace SeekDeepWithin.Domain
{
   /// <summary>
   /// Represents a version.
   /// </summary>
   public class Version : IDbTable
   {
      /// <summary>
      /// Gets the id of the table.
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
      /// Gets or Sets an abbreviation to use for this version.
      /// </summary>
      public string Abbreviation { get; set; }

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
      /// Gets or Sets the book of the version
      /// </summary>
      public virtual Book Book { get; set; }

      /// <summary>
      /// Gets or Sets the list of subbooks.
      /// </summary>
      public virtual ICollection<SubBook> SubBooks { get; set; }

      /// <summary>
      /// Gets or Sets the source of this version.
      /// </summary>
      public virtual ICollection<VersionSource> VersionSources { get; set; }
   }
}
