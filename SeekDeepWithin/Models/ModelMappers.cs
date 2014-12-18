using System.Collections.ObjectModel;
using System.Linq;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// A class of mappers for our models and view models.
   /// </summary>
   public static class ModelMappers
   {
      /// <summary>
      /// Maps the view model to a model.
      /// </summary>
      /// <param name="viewModel">The view model to map.</param>
      /// <returns>A new model based on the given view model.</returns>
      public static Book ToModel (this BookViewModel viewModel)
      {
         return new Book
         {
            Id = viewModel.Id,
            Title = viewModel.Title,
            Summary = viewModel.Summary
         };
      }

      /// <summary>
      /// Maps the model to a view model.
      /// </summary>
      /// <param name="book">The model to map.</param>
      /// <param name="deepCopy">Copies all properties.</param>
      /// <returns>A new view model based on the given model.</returns>
      public static BookViewModel ToViewModel (this Book book, bool deepCopy = true)
      {
         var viewModel = new BookViewModel
         {
            Id = book.Id,
            Title = book.Title,
            Summary = book.Summary,
            Versions = new Collection <VersionViewModel> ()
         };
         if (deepCopy)
         {
            foreach (var version in book.Versions)
               viewModel.Versions.Add (version.ToViewModel ());
         }
         return viewModel;
      }

      /// <summary>
      /// Maps the model to a view model.
      /// </summary>
      /// <param name="version">The model to map.</param>
      /// <param name="deepCopy">Copies all properties.</param>
      /// <returns>A new view model based on the given model.</returns>
      public static VersionViewModel ToViewModel (this Version version, bool deepCopy = true)
      {
         var source = version.VersionSources.FirstOrDefault ();
         var viewModel = new VersionViewModel
         {
            Id = version.Id,
            Name = version.Name,
            BookId = version.Book.Id,
            Abbreviation = version.Abbreviation,
            Summary = version.Summary,
            TitleFormat = version.TitleFormat,
            PublishDate = version.PublishDate,
            SubBooks = new Collection <SubBookViewModel> (),
            Writers = new Collection <WriterLink> (),
            Book = version.Book.ToViewModel (false),
            SourceName = source == null ? string.Empty : source.Source.Name,
            SourceUrl = source == null ? string.Empty : source.Source.Url
         };
         if (deepCopy)
         {
            foreach (var subBook in version.SubBooks.OrderBy (sb => sb.Order))
               viewModel.SubBooks.Add (subBook.ToViewModel ());
            foreach (var writer in version.Writers)
            {
               viewModel.Writers.Add (new WriterLink
               {
                  Author = new AuthorViewModel
                  {
                     Id = writer.Author.Id,
                     Name = writer.Author.Name
                  },
                  Version = writer.Version.ToViewModel (false)
               });
            }
         }
         return viewModel;
      }

      /// <summary>
      /// Maps the model to a view model.
      /// </summary>
      /// <param name="subBook">The model to map.</param>
      /// <param name="deepCopy">Copies all properties.</param>
      /// <returns>A new view model based on the given model.</returns>
      public static SubBookViewModel ToViewModel (this SubBook subBook, bool deepCopy = true)
      {
         var viewModel = new SubBookViewModel
         {
            Id = subBook.Id,
            Name = subBook.Name,
            Order = subBook.Order,
            VersionId = subBook.Version.Id,
            Chapters = new Collection <ChapterViewModel> (),
            Version = subBook.Version.ToViewModel (false)
         };
         if (deepCopy)
         {
            foreach (var chapter in subBook.Chapters.OrderBy(c => c.Order))
               viewModel.Chapters.Add (chapter.ToViewModel ());
         }
         return viewModel;
      }

      /// <summary>
      /// Maps the model to a view model.
      /// </summary>
      /// <param name="chapter">The model to map.</param>
      /// <param name="deepCopy">Copies all properties.</param>
      /// <returns>A new view model based on the given model.</returns>
      public static ChapterViewModel ToViewModel (this Chapter chapter, bool deepCopy = true)
      {
         var viewModel = new ChapterViewModel
         {
            Id =  chapter.Id,
            Name = chapter.Name,
            Order = chapter.Order,
            SubBookId = chapter.SubBook.Id,
            DefaultToParagraph = chapter.DefaultToParagraph,
            Passages = new Collection <PassageEntryViewModel> (),
            SubBook = chapter.SubBook.ToViewModel (false)
         };
         if (deepCopy)
         {
            foreach (var passageEntry in chapter.Passages.OrderBy (pe => pe.Order))
               viewModel.Passages.Add (passageEntry.ToViewModel ());
         }
         return viewModel;
      }

      /// <summary>
      /// Maps the model to a view model.
      /// </summary>
      /// <param name="passage">The model to map.</param>
      /// <param name="deepCopy">Copies all properties.</param>
      /// <returns>A new view model based on the given model.</returns>
      public static PassageViewModel ToViewModel (this Passage passage, bool deepCopy = true)
      {
         var viewModel = new PassageViewModel
         {
            Text = passage.Text,
            Id = passage.Id,
            PassageEntries = new Collection <PassageEntryViewModel> (),
            PassageLinks = new Collection <PassageLinkViewModel> ()
         };
         foreach (var passageLink in passage.PassageLinks)
         {
            viewModel.PassageLinks.Add (new PassageLinkViewModel
            {
               StartIndex = passageLink.StartIndex,
               EndIndex = passageLink.EndIndex,
               Url = passageLink.Link.Url,
               OpenInNewWindow = passageLink.Link.OpenInNewWindow
            });
         }
         if (deepCopy)
         {
            foreach (var passageEntry in passage.PassageEntries)
            {
               viewModel.PassageEntries.Add (new PassageEntryViewModel
               {
                  Id = passageEntry.Id,
                  ChapterId = passageEntry.ChapterId,
                  PassageId = passageEntry.PassageId,
                  Number = passageEntry.Number,
                  Order = passageEntry.Order,
                  Passage = viewModel,
                  Chapter = passageEntry.Chapter.ToViewModel (false)
               });
            }
         }
         return viewModel;
      }

      /// <summary>
      /// Maps the model to a view model.
      /// </summary>
      /// <param name="passageEntry">The model to map.</param>
      /// <returns>A new view model based on the given model.</returns>
      public static PassageEntryViewModel ToViewModel (this PassageEntry passageEntry)
      {
         return new PassageEntryViewModel
         {
            Chapter = passageEntry.Chapter.ToViewModel (false),
            ChapterId = passageEntry.ChapterId,
            Id = passageEntry.Id,
            Number = passageEntry.Number,
            Order = passageEntry.Order,
            Passage = passageEntry.Passage.ToViewModel (false),
            PassageId = passageEntry.PassageId
         };
      }

      /// <summary>
      /// Maps the model to a view model.
      /// </summary>
      /// <param name="glossaryTerm">The model to map.</param>
      /// <param name="deepCopy">Copies all properties.</param>
      /// <returns>A new view model based on the given model.</returns>
      public static GlossaryTermViewModel ToViewModel (this GlossaryTerm glossaryTerm, bool deepCopy)
      {
         var viewModel = new GlossaryTermViewModel
         {
            Id = glossaryTerm.Id,
            Name = glossaryTerm.Name,
            Entries = new Collection <GlossaryEntryViewModel> ()
         };
         if (deepCopy)
         {
            foreach (var entry in glossaryTerm.Entries)
               viewModel.Entries.Add (entry.ToViewModel ());
         }
         return viewModel;
      }

      /// <summary>
      /// Maps the model to a view model.
      /// </summary>
      /// <param name="glossaryEntry">The model to map.</param>
      /// <returns>A new view model based on the given model.</returns>
      public static GlossaryEntryViewModel ToViewModel (this GlossaryEntry glossaryEntry)
      {
         var source = glossaryEntry.GlossaryEntrySources.FirstOrDefault ();
         return new GlossaryEntryViewModel
         {
            Id = glossaryEntry.Id,
            Text = glossaryEntry.Text,
            TermId = glossaryEntry.GlossaryTerm.Id,
            SourceName = source == null ? string.Empty : source.Source.Name,
            SourceUrl = source == null ? string.Empty : source.Source.Url
         };
      }
   }
}