using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// A view model for a chapter.
   /// </summary>
   public class ChapterViewModel
   {
      /// <summary>
      /// Initializes a new chapter view model.
      /// </summary>
      public ChapterViewModel ()
      {
         this.Passages = new Collection <PassageViewModel> ();
         this.Headers = new Collection <HeaderFooterViewModel> ();
         this.Footers = new Collection <HeaderFooterViewModel> ();
      }

      /// <summary>
      /// Gets or Sets the id of this chapter.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the order of this chapter.
      /// </summary>
      [Required]
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the name of this chapter.
      /// </summary>
      [Required]
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the id of the parent sub book.
      /// </summary>
      public int SubBookId { get; set; }

      /// <summary>
      /// Gets or Sets the default reading style.
      /// </summary>
      public bool DefaultToParagraph { get; set; }

      /// <summary>
      /// Gets or Sets the sub book this chapter belongs to.
      /// </summary>
      public SubBookViewModel SubBook { get; set; }

      /// <summary>
      /// Get or Sets the headers for this chapter.
      /// </summary>
      public Collection<HeaderFooterViewModel> Headers { get; set; }

      /// <summary>
      /// Get or Sets the footers for this chapter.
      /// </summary>
      public Collection<HeaderFooterViewModel> Footers { get; set; }

      /// <summary>
      /// Gets or Sets the list of passages.
      /// </summary>
      public Collection<PassageViewModel> Passages { get; set; }
   }
}