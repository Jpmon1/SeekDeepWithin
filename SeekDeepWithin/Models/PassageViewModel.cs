using System;
using System.Collections.ObjectModel;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using Version = SeekDeepWithin.Pocos.Version;

namespace SeekDeepWithin.Models
{
   public class PassageViewModel : IRenderable
   {
      /// <summary>
      /// Initializes a new passage view model.
      /// </summary>
      public PassageViewModel ()
      {
         this.Links = new Collection <LinkViewModel> ();
         this.Styles = new Collection <StyleViewModel> ();
         this.Footers = new Collection <HeaderFooterViewModel> ();
      }

      /// <summary>
      /// Initializes a new passage view model.
      /// </summary>
      /// <param name="entry">The passage entry to copy data from.</param>
      /// <param name="chapter">The chapter this entry belongs to.</param>
      /// <param name="subBook">The subbook this entry belongs to.</param>
      /// <param name="version">The version this entry belongs to.</param>
      public PassageViewModel (PassageEntry entry, SubBookChapter chapter = null, VersionSubBook subBook = null, Version version = null)
      {
         if (entry == null) return;
         this.EntryId = entry.Id;
         this.Id = entry.Passage.Id;
         this.Number = entry.Number;
         this.Text = entry.Passage.Text;
         if (chapter == null)
            chapter = entry.Chapter;
         if (subBook == null)
            subBook = chapter.SubBook;
         if (version == null)
            version = subBook.Version;
         this.ChapterId = chapter.Id;
         this.ChapterName = chapter.Chapter.Name;
         this.SubBookId = subBook.Id;
         this.SubBookName = subBook.Term.Name;
         this.VersionId = version.Id;
         this.VersionName = version.Title;
         if (entry.Header != null && !string.IsNullOrWhiteSpace(entry.Header.Text))
            this.Header = new HeaderFooterViewModel (entry.Header);

         var title = this.VersionName + " | ";
         if (!chapter.SubBook.Hide)
            title += this.SubBookName + " | ";
         if (!chapter.Hide)
            title += this.ChapterName + ":";
         title += this.Number;
         this.Title = title;

         this.Links = new Collection<LinkViewModel> ();
         this.Styles = new Collection<StyleViewModel> ();
         this.Footers = new Collection<HeaderFooterViewModel> ();

         foreach (var link in entry.Passage.Links)
            this.Links.Add (new LinkViewModel (link));
         foreach (var style in entry.Styles)
            this.Styles.Add (new StyleViewModel (style));
         foreach (var footer in entry.Footers)
            this.Footers.Add (new HeaderFooterViewModel (footer));
      }

      /// <summary>
      /// Gets or Sets how to display this verse.
      /// </summary>
      public VerseDisplayType DisplayType { get; set; }

      /// <summary>
      /// Gets or Sets the title of the page.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets the title without the verse number.
      /// </summary>
      /// <returns></returns>
      public string GetTitleNoVerse ()
      {
         var titleSplit = this.Title.Split(':');
         if (titleSplit.Length > 1)
            return titleSplit [0];
         return Title;
      }

      /// <summary>
      /// Gets or Sets the id of the passage.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the entry id.
      /// </summary>
      public int EntryId { get; set; }

      /// <summary>
      /// Gets or Sets the text of the passage.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the number of the passage.
      /// </summary>
      public int Number { get; set; }

      /// <summary>
      /// Gets or Sets the id of the chapter.
      /// </summary>
      public int ChapterId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the chapter.
      /// </summary>
      public string ChapterName { get; set; }

      /// <summary>
      /// Gets or Sets the id of the sub book.
      /// </summary>
      public int SubBookId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the sub book.
      /// </summary>
      public string SubBookName { get; set; }

      /// <summary>
      /// Gets or Sets the id of the version.
      /// </summary>
      public int VersionId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the version.
      /// </summary>
      public string VersionName { get; set; }

      /// <summary>
      /// Get or Sets the headers for this passage.
      /// </summary>
      public HeaderFooterViewModel Header { get; set; }

      /// <summary>
      /// Gets the list of links for this passage.
      /// </summary>
      public Collection<LinkViewModel> Links { get; set; }

      /// <summary>
      /// Get or Sets the styles for this passage.
      /// </summary>
      public Collection<StyleViewModel> Styles { get; set; }

      /// <summary>
      /// Get or Sets the footers for this passage.
      /// </summary>
      public Collection<HeaderFooterViewModel> Footers { get; set; }

      /// <summary>
      /// Gets or Sets the renderer.
      /// </summary>
      public SdwRenderer Renderer { get; set; }

      /// <summary>
      /// Renders the passage.
      /// </summary>
      /// <param name="url"></param>
      /// <returns>The html to display for the passage.</returns>
      public string Render (Uri url)
      {
         if (string.IsNullOrWhiteSpace (this.Text)) return string.Empty;
         if (this.Text.StartsWith ("|PARSE|"))
         {
            var parser = new PassageParser (new SdwDatabase ());
            parser.Parse (this.Text.Substring (7));
            return parser.BuildHtmlOutput (url);
         }
         if (this.Renderer == null)
            this.Renderer = new SdwRenderer ();
         return Renderer.Render (this);
      }
   }
}