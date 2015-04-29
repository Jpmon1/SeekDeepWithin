using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Pocos;

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
         this.Headers = new Collection<ChapterHeaderViewModel> ();
         this.Footers = new Collection<ChapterFooterViewModel> ();
      }
      /// <summary>
      /// Initializes a new chapter view model.
      /// </summary>
      /// <param name="chapter">The chapter to copy data from.</param>
      public ChapterViewModel (SubBookChapter chapter)
      {
         this.Id = chapter.Id;
         this.Hide = chapter.Hide;
         this.Name = chapter.Chapter.Name;
         this.SubBookId = chapter.SubBook.Id;
         this.DefaultToParagraph = chapter.DefaultToParagraph;
         this.SubBook = new SubBookViewModel (chapter.SubBook);
         this.Passages = new Collection<PassageViewModel> ();
         this.Headers = new Collection<ChapterHeaderViewModel> ();
         this.Footers = new Collection<ChapterFooterViewModel> ();

         foreach (var header in chapter.Headers)
            this.Headers.Add (new ChapterHeaderViewModel (header));
         foreach (var footer in chapter.Footers)
            this.Footers.Add (new ChapterFooterViewModel (footer));
         var renderer = new SdwRenderer ();
         foreach (var entry in chapter.Passages.OrderBy (pe => pe.Order))
            this.Passages.Add (new PassageViewModel (entry) { Renderer = renderer });
         foreach (var passageViewModel in this.Passages)
            passageViewModel.DisplayType = DefaultToParagraph ? VerseDisplayType.Paragraph : VerseDisplayType.Number;
      }

      /// <summary>
      /// Gets or Sets the id of this chapter.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of this chapter.
      /// </summary>
      [Required]
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets if this is hidden.
      /// </summary>
      public bool Hide { get; set; }

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
      public Collection<ChapterHeaderViewModel> Headers { get; set; }

      /// <summary>
      /// Get or Sets the footers for this chapter.
      /// </summary>
      public Collection<ChapterFooterViewModel> Footers { get; set; }

      /// <summary>
      /// Gets or Sets the list of passages.
      /// </summary>
      public Collection<PassageViewModel> Passages { get; set; }
   }
}