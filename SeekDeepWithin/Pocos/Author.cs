using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents an author or translator.
   /// </summary>
   public class Author : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the author.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of the author.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets information about this author.
      /// </summary>
      public string About { get; set; }

      /// <summary>
      /// Gets the book versions written by the author.
      /// </summary>
      public virtual ICollection<Writer> Writers { get; set; }
   }
}
