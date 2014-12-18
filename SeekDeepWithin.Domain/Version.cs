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
      /// Gets or Sets an abbreviation to use for this version.
      /// </summary>
      public string Abbreviation { get; set; }

      /// <summary>
      /// Gets or Sets the name of the table.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the format the title should be displayed in.
      /// </summary>
      public string TitleFormat { get; set; }

      /// <summary>
      /// Gets or Sets the date the version was published.
      /// </summary>
      public string PublishDate { get; set; }

      /// <summary>
      /// Gets or Sets the about summary.
      /// </summary>
      public string About { get; set; }

      /// <summary>
      /// Gets or Sets the book of the version
      /// </summary>
      public virtual Book Book { get; set; }

      /// <summary>
      /// Gets or Sets the source of this version.
      /// </summary>
      public virtual ICollection<VersionSource> VersionSources { get; set; }

      /// <summary>
      /// Gets or Sets the links for this version's about.
      /// </summary>
      public virtual ICollection<VersionAboutLink> VersionAboutLinks { get; set; }

      /// <summary>
      /// Gets or Sets the styles for this version's about.
      /// </summary>
      public virtual ICollection<VersionAboutStyle> VersionAboutStyles { get; set; }

      /// <summary>
      /// Gets or Sets the list of writers.
      /// </summary>
      public virtual ICollection<Writer> Writers { get; set; }

      /// <summary>
      /// Gets or Sets the list of subbooks.
      /// </summary>
      public virtual ICollection<SubBook> SubBooks { get; set; }
   }
}
