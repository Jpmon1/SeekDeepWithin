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
      }

      /// <summary>
      /// Initializes a new chapter view model.
      /// </summary>
      /// <param name="chapter">The chapter to copy data from.</param>
      /// <param name="subBook">The parent sub book.</param>
      public ChapterViewModel (SubBookChapter chapter, SubBookViewModel subBook = null)
      {
         this.Id = chapter.Id;
         this.Hide = chapter.Hide;
         this.Order = chapter.Order;
         this.Name = chapter.Chapter.Name;
         if (chapter.Header != null && !string.IsNullOrWhiteSpace(chapter.Header.Text))
            this.Header = new HeaderFooterViewModel (chapter.Header);
         this.DefaultToParagraph = chapter.DefaultToParagraph;
         if (subBook == null)
         {
            this.SubBook = new SubBookViewModel (chapter.SubBook);
            this.Passages = new Collection <PassageViewModel> ();

            var sb = chapter.SubBook;
            var version = chapter.SubBook.Version;
            if (chapter.Footer != null && !string.IsNullOrWhiteSpace (chapter.Footer.Text))
               this.Footer = new HeaderFooterViewModel (chapter.Footer);
            var renderer = new SdwRenderer ();
            foreach (var entry in chapter.Passages.OrderBy (pe => pe.Order))
               this.Passages.Add (new PassageViewModel (entry, chapter, sb, version) {Renderer = renderer});
            foreach (var passageViewModel in this.Passages)
               passageViewModel.DisplayType = DefaultToParagraph ? VerseDisplayType.Paragraph : VerseDisplayType.Number;
         }
         else
         {
            this.SubBook = subBook;
         }
      }

      /// <summary>
      /// Gets or Sets the order of the chapter.
      /// </summary>
      public int Order { get; set; }

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
      /// Gets or Sets the default reading style.
      /// </summary>
      public bool DefaultToParagraph { get; set; }

      /// <summary>
      /// Gets or Sets the sub book this chapter belongs to.
      /// </summary>
      public SubBookViewModel SubBook { get; set; }

      /// <summary>
      /// Get or Sets the header for this chapter.
      /// </summary>
      public HeaderFooterViewModel Header { get; set; }

      /// <summary>
      /// Get or Sets the footers for this chapter.
      /// </summary>
      public HeaderFooterViewModel Footer { get; set; }

      /// <summary>
      /// Gets or Sets the list of passages.
      /// </summary>
      public Collection<PassageViewModel> Passages { get; set; }
   }
}