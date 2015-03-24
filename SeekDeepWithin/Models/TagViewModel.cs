using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a view model for a tag.
   /// </summary>
   public class TagViewModel
   {
      private readonly Dictionary<int, string> m_Terms;
      private readonly Dictionary<int, string> m_Books;
      private readonly Dictionary <int, string> m_SubBooks;

      /// <summary>
      /// Initializes a new tag view model.
      /// </summary>
      public TagViewModel ()
      {
         this.m_Terms = new Dictionary <int, string> ();
         this.m_Books = new Dictionary<int, string> ();
         this.m_SubBooks = new Dictionary<int, string> ();
      }

      /// <summary>
      /// Gets or Sets the id of the tag.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the tag's item.
      /// </summary>
      public int ItemId { get; set; }

      /// <summary>
      /// Get or Sets name of this tag.
      /// </summary>
      [Required]
      public string Name { get; set; }

      /// <summary>
      /// Gets the collections of terms that belong to this tag.
      /// </summary>
      public Dictionary<int, string> Terms { get { return this.m_Terms; } }

      /// <summary>
      /// Gets the collection of books that belong to this tag.
      /// </summary>
      public Dictionary<int, string> Books { get { return this.m_Books; } }

      /// <summary>
      /// Gets the collection of sub books that belong to this tag.
      /// </summary>
      public Dictionary <int, string> SubBooks { get { return m_SubBooks; } }
   }
}