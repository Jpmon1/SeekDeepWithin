using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class SubBook : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this sub book.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of this sub book.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the book this sub book belongs to.
      /// </summary>
      public virtual Book Book { get; set; }

      /// <summary>
      /// Gets or Sets the list of writers.
      /// </summary>
      public virtual ICollection<SubBookWriter> Writers { get; set; }

      /// <summary>
      /// Gets or Sets the list of versions.
      /// </summary>
      public virtual ICollection<VersionSubBook> Versions { get; set; }

      /// <summary>
      /// Gets or Sets the list of know abbreviations.
      /// </summary>
      public virtual ICollection<Abbreviation> Abbreviations { get; set; }
   }
}
