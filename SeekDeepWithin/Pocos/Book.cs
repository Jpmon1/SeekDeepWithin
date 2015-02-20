using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a book.
   /// </summary>
   public class Book : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the book.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets if this book should be hidden or not.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets or Sets the title of the book.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or Sets a brief summary of the book.
      /// </summary>
      public string Summary { get; set; }

      /// <summary>
      /// Gets or Sets the list of verions.
      /// </summary>
       public virtual ICollection<Version> Versions { get; set; }

      /// <summary>
      /// Gets or Sets the list of tags for this book.
      /// </summary>
       public virtual ICollection<BookTag> BookTags { get; set; }
   }
}
