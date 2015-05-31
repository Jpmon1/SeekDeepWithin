using System;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   public class SearchController : Controller
   {
      private readonly ISdwDatabase m_Db;

      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public SearchController ()
      {
         this.m_Db = new SdwDatabase ();
      }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public SearchController (ISdwDatabase db)
      {
         this.m_Db = db;
      }

      /// <summary>
      /// Gets the search index page.
      /// </summary>
      /// <returns>The search view.</returns>
      public ActionResult Index ()
      {
         ViewBag.Tags = new SelectList (this.m_Db.Tags.All (q => q.OrderBy (t => t.Name)), "Id", "Name");
         return View ();
      }

      [Authorize (Roles = "Administrator")]
      public ActionResult OptimizeIndexData (string type)
      {
         Search.Optimize (Search.GetSearchType (type));
         return View ("IndexData");
      }

      [Authorize (Roles = "Administrator")]
      public ActionResult ClearIndexedData (string type)
      {
         Search.Clear (Search.GetSearchType (type));
         return View ("IndexData");
      }

      /// <summary>
      /// Adds index data to search.
      /// </summary>
      /// <param name="type">The type to add, null if adding all.</param>
      /// <returns></returns>
      [Authorize (Roles = "Administrator")]
      public ActionResult IndexData (string type)
      {
         try
         {
            var searchType = Search.GetSearchType (type);
            if (searchType == SearchType.All || searchType == SearchType.Book)
               Search.AddOrUpdateIndex (this.m_Db.Books.All (), SearchType.Book);
            if (searchType == SearchType.All || searchType == SearchType.Version)
               Search.AddOrUpdateIndex (this.m_Db.Versions.All (), SearchType.Version);
            if (searchType == SearchType.All || searchType == SearchType.SubBook)
               Search.AddOrUpdateIndex (this.m_Db.VersionSubBooks.All (), SearchType.SubBook);
            if (searchType == SearchType.All || searchType == SearchType.Chapter)
               Search.AddOrUpdateIndex (this.m_Db.Chapters.All (), SearchType.Chapter);
            if (searchType == SearchType.All || searchType == SearchType.Passage)
               Search.AddOrUpdateIndex (this.m_Db.PassageEntries.All (), SearchType.Passage);
            if (searchType == SearchType.All || searchType == SearchType.Term)
               Search.AddOrUpdateIndex (this.m_Db.GlossaryTerms.All (), SearchType.Term);
            if (searchType == SearchType.All || searchType == SearchType.Glossary)
               Search.AddOrUpdateIndex (this.m_Db.GlossaryEntries.All (), SearchType.Glossary);
            if (searchType == SearchType.All || searchType == SearchType.Writer)
               Search.AddOrUpdateIndex (this.m_Db.Writers.All (), SearchType.Writer);
            if (searchType == SearchType.All || searchType == SearchType.Tag)
               Search.AddOrUpdateIndex (this.m_Db.Tags.All (), SearchType.Tag);
            Search.Optimize (searchType);
         }
         catch (Exception ex)
         {
            ViewBag.Error = ex.Message;
         }
         return View ();
      }

      /// <summary>
      /// Gets the search page.
      /// </summary>
      /// <param name="search">The string to search for.</param>
      /// <returns>The search view.</returns>
      public ActionResult Query (SearchQueryViewModel search)
      {
         if (string.IsNullOrWhiteSpace (search.Query))
         {
            Response.StatusCode = 500;
            return Json ("Please specify a query for searching.");
         }

         search.BuildQuery ();
         var parser = new PassageParser (this.m_Db);
         parser.Parse(search.Query);
         var viewModel = new SearchViewModel (search) { ParserLog = parser.Log };
         foreach (var passage in parser.PassageList)
         {
            viewModel.Results.Add (new SearchResult
            {
               Id = passage.Id,
               Type = SearchType.Passage,
               Title = passage.GetTitle (Request.Url),
               Description = passage.Passage.Text
            });
         }

         var results = Search.Query (search);
         var host = Request.Url == null ? string.Empty : Request.Url.AbsoluteUri.Replace (Request.Url.AbsolutePath, "");
         foreach (var result in results)
         {
            var ids = result.Value;
            if (result.Key == SearchType.Passage)
            {
               var passages = this.m_Db.PassageEntries.Get (p => ids.Contains (p.Id));
               foreach (var passage in passages)
               {
                  viewModel.Results.Add (new SearchResult
                  {
                     Id = passage.Id,
                     Type = result.Key,
                     Title = passage.GetTitle (Request.Url),
                     Description = passage.Passage.Text.Highlight (search)
                  });
               }
            }
            else if (result.Key == SearchType.Book)
            {
               var books = this.m_Db.Books.Get (b => ids.Contains (b.Id));
               foreach (var book in books)
               {
                  var res = new SearchResult
                  {
                     Id = book.Id,
                     Type = SearchType.Book,
                     Title = string.Format ("<a href=\"{0}/Book/Details/{1}\">{2}</a>", host, book.Id, book.Title.Highlight(search))
                  };
                  if (!viewModel.Results.Contains (res))
                     viewModel.Results.Add (res);
               }
            }
            else if (result.Key == SearchType.Version)
            {
               var versions = this.m_Db.Versions.Get (v => ids.Contains (v.Id));
               foreach (var book in versions.Select (v => v.Book))
               {
                  var res = new SearchResult
                  {
                     Id = book.Id,
                     Type = SearchType.Book,
                     Title = string.Format ("<a href=\"{0}/Book/Details/{1}\">{2}</a>", host, book.Id, book.Title.Highlight(search))
                  };
                  if (!viewModel.Results.Contains (res))
                     viewModel.Results.Add (res);
               }
            }
            /*else if (result.Key == SearchType.SubBook)
            {
               var subBooks = this.m_Db.VersionSubBooks.Get (vs => ids.Contains (vs.Id) && !vs.Hide);
               foreach (var subBook in subBooks)
               {
                  viewModel.Results.Add (new SearchResult { Type = SearchType.Book, Title = subBook.Version.Title });
               }
            }*/
            else if (result.Key == SearchType.Chapter)
            {
               var chapters = this.m_Db.Chapters.Get (c => ids.Contains (c.Id));
               foreach (var chapter in chapters)
               {
                  viewModel.Results.Add (new SearchResult
                  {
                     Id = chapter.Id,
                     Type = SearchType.Book,
                     Title = chapter.Name.Highlight(search)
                  });
               }
            }
            else if (result.Key == SearchType.Term)
            {
               var terms = this.m_Db.GlossaryTerms.Get (t => ids.Contains (t.Id));
               foreach (var term in terms)
               {
                  viewModel.Results.Add (new SearchResult
                  {
                     Id = term.Id,
                     Type = result.Key,
                     Title = string.Format ("<a href=\"{0}/Term/{1}\">{2}</a>", host, term.Id, term.Name.Highlight(search))
                  });
               }
            }
            else if (result.Key == SearchType.Glossary)
            {
               var entries = this.m_Db.GlossaryEntries.Get (e => ids.Contains (e.Id));
               foreach (var entry in entries)
               {
                  viewModel.Results.Add (new SearchResult
                  {
                     Id = entry.Id,
                     Type = result.Key,
                     Title = string.Format ("<a href=\"{0}/Term/{1}\">{2}</a>", host, entry.Item.Term.Id, entry.Item.Term.Name.Highlight (search)),
                     Description = entry.Text.Highlight(search)
                  });
               }
            }
            else if (result.Key == SearchType.Writer)
            {
               var writers = this.m_Db.Writers.Get (w => ids.Contains (w.Id));
               foreach (var writer in writers)
               {
                  viewModel.Results.Add (new SearchResult
                  {
                     Id = writer.Id,
                     Type = result.Key,
                     Title = string.Format ("<a href=\"{0}/Writer/Details/{1}\">{2}</a>", host, writer.Id, writer.Name.Highlight(search))
                  });
               }
            }
            else if (result.Key == SearchType.Tag)
            {
               var tags = this.m_Db.Tags.Get (t => ids.Contains (t.Id));
               foreach (var tag in tags)
               {
                  viewModel.Results.Add (new SearchResult
                  {
                     Id = tag.Id,
                     Type = result.Key,
                     Title = string.Format ("<a href=\"{0}/Tag/Details/{1}\">{2}</a>", host, tag.Id, tag.Name.Highlight(search))
                  });
               }
            }
         }
         return PartialView (viewModel);
      }
   }
}
