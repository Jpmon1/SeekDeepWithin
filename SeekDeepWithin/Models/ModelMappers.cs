using System.Collections.ObjectModel;
using System.Linq;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// A class of mappers for our models and view models.
   /// </summary>
   public static class ModelMappers
   {
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
               viewModel.Versions.Add (version.ToViewModel (false));
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
            About = version.About,
            TitleFormat = version.TitleFormat,
            PublishDate = version.PublishDate,
            SubBooks = new Collection <SubBookViewModel> (),
            Writers = new Collection <WriterLink> (),
            VersionAboutLinks = new Collection<LinkViewModel> (),
            VersionAboutStyles = new Collection<StyleViewModel> (),
            Book = version.Book.ToViewModel (false),
            SourceName = source == null ? string.Empty : source.Source.Name,
            SourceUrl = source == null ? string.Empty : source.Source.Url
         };
         if (deepCopy)
         {
            foreach (var subBook in version.SubBooks.OrderBy (sb => sb.Order))
               viewModel.SubBooks.Add (subBook.ToViewModel ());
            foreach (var link in version.VersionAboutLinks)
            {
               viewModel.VersionAboutLinks.Add (new LinkViewModel
               {
                  StartIndex = link.StartIndex,
                  EndIndex = link.EndIndex,
                  Url = link.Link.Url,
                  OpenInNewWindow = link.OpenInNewWindow,
               });
            }
            foreach (var style in version.VersionAboutStyles)
            {
               viewModel.VersionAboutStyles.Add (new StyleViewModel
               {
                  StartIndex = style.StartIndex,
                  EndIndex = style.EndIndex,
                  Start = style.Style.Start,
                  End = style.Style.End,
                  Id = style.Id
               });
            }
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
            Passages = new Collection <PassageViewModel> (),
            SubBook = chapter.SubBook.ToViewModel (false)
         };
         if (deepCopy)
         {
            foreach (var entry in chapter.Passages.OrderBy (pe => pe.Order))
               viewModel.Passages.Add (PassageController.GetViewModel (entry.Passage, entry));
         }
         return viewModel;
      }
   }
}