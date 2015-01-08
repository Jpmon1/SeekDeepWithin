using System.Collections.Generic;

namespace SeekDeepWithin.Domain
{
   public class SubBook : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this sub book.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the order of this sub book.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets if this sub book should be hidden or not.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets or Sets the name of this sub book.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the version of this sub book.
      /// </summary>
      public virtual Version Version { get; set; }

      /// <summary>
      /// Gets or Sets the list of writers.
      /// </summary>
      public virtual ICollection<Writer> Writers { get; set; }

      /// <summary>
      /// Gets or Sets the list of chapters.
      /// </summary>
      public virtual ICollection<Chapter> Chapters { get; set; }
   }
}
