using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Models;
using SeekDeepWithin.SdwSearch;

namespace SeekDeepWithin.Controllers
{
   public class SearchController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public SearchController () : base (new SdwDatabase()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public SearchController (ISdwDatabase db) : base (db) { }

      /// <summary>
      /// Gets the search index page.
      /// </summary>
      /// <returns>The search view.</returns>
      public ActionResult Index ()
      {
         return View ();
      }

      /// <summary>
      /// Optimizes indexed data.
      /// </summary>
      /// <param name="type">The type to optimize, null if all.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Administrator")]
      public ActionResult OptimizeIndex (string type)
      {
         try
         {
            type = type == null ? string.Empty : type.ToLower ();
            if (string.IsNullOrEmpty (type) || type == "book")
               BookSearch.Optimize();
            if (string.IsNullOrEmpty (type) || type == "passage")
               PassageSearch.Optimize ();
            if (string.IsNullOrEmpty (type) || type == "term")
               TermSearch.Optimize ();
            if (string.IsNullOrEmpty (type) || type == "glossary")
               GlossarySearch.Optimize ();
            return Json ("success");
         }
         catch (Exception ex)
         {
            return this.Fail (ex.Message);
         }
      }

      /// <summary>
      /// Deletes indexed data.
      /// </summary>
      /// <param name="type">The type to delete, null if all.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Administrator")]
      public ActionResult DeleteIndex (string type)
      {
         try
         {
            type = type == null ? string.Empty : type.ToLower ();
            if (string.IsNullOrEmpty (type) || type == "book")
               BookSearch.Clear ();
            if (string.IsNullOrEmpty (type) || type == "passage")
               PassageSearch.Clear ();
            if (string.IsNullOrEmpty (type) || type == "term")
               TermSearch.Clear ();
            if (string.IsNullOrEmpty (type) || type == "glossary")
               GlossarySearch.Clear ();
            return Json ("success");
         }
         catch (Exception ex)
         {
            return this.Fail (ex.Message);
         }
      }

      /// <summary>
      /// Adds index data to search.
      /// </summary>
      /// <param name="type">The type to add, null if adding all.</param>
      /// <param name="start">The begging number of object to index.</param>
      /// <param name="length">The number of objects to index.</param>
      /// <returns>Result</returns>
      [HttpPost]
      [ValidateAntiForgeryToken]
      [Authorize (Roles = "Administrator")]
      public ActionResult CreateIndex (string type, int? start, int? length)
      {
         try
         {
            if (string.IsNullOrEmpty (type))
               this.Fail ("Invalid search type.");
            int num = 0;
            type = type == null ? string.Empty : type.ToLower ();
            if (type == "book")
            {
               num = this.Database.Books.All ().Count ();
               if (start != null && start.Value != -1)
               {
                  if (length != null && length.Value != -1)
                  {
                     BookSearch.AddOrUpdateIndex (this.Database.Books.All ()
                        .Skip (start.Value)
                        .Take (length.Value));
                  }
                  else
                  {
                     BookSearch.AddOrUpdateIndex (this.Database.Books.All ()
                        .Skip (start.Value));
                  }
               }
               else
               {
                  BookSearch.AddOrUpdateIndex (this.Database.Books.All ());
               }
               BookSearch.Optimize ();
            }
            else if (type == "passage")
            {
               num = this.Database.PassageEntries.All ().Count ();
               if (start != null && start.Value != -1)
               {
                  if (length != null && length.Value != -1)
                  {
                     PassageSearch.AddOrUpdateIndex (this.Database.PassageEntries.All ()
                        .OrderBy (p => p.Chapter.SubBook.Version.Book.Title)
                        .ThenBy (p => p.Chapter.SubBook.Version.Title)
                        .ThenBy (p => p.Chapter.SubBook.Order)
                        .ThenBy (p => p.Chapter.Order)
                        .ThenBy (p => p.Number)
                        .Skip (start.Value)
                        .Take (length.Value));
                  }
                  else
                  {
                     PassageSearch.AddOrUpdateIndex (this.Database.PassageEntries.All ()
                        .OrderBy (p => p.Chapter.SubBook.Version.Book.Title)
                        .ThenBy (p => p.Chapter.SubBook.Version.Title)
                        .ThenBy (p => p.Chapter.SubBook.Term.Name)
                        .ThenBy (p => p.Chapter.Order)
                        .ThenBy (p => p.Number)
                        .Skip (start.Value));
                  }
               }
               else
               {
                  PassageSearch.AddOrUpdateIndex (this.Database.PassageEntries.All ()
                     .OrderBy (p => p.Chapter.SubBook.Version.Book.Title)
                     .ThenBy (p => p.Chapter.SubBook.Version.Title)
                     .ThenBy (p => p.Chapter.Order)
                     .ThenBy (p => p.Chapter.Chapter.Name)
                     .ThenBy (p => p.Number));
               }
               PassageSearch.Optimize ();
            }
            else if (type == "term")
            {
               num = this.Database.Terms.All ().Count ();
               if (start != null && start.Value != -1)
               {
                  if (length != null && length.Value != -1)
                  {
                     TermSearch.AddOrUpdateIndex (this.Database.Terms.All ()
                        .Skip (start.Value)
                        .Take (length.Value));
                  }
                  else
                  {
                     TermSearch.AddOrUpdateIndex (this.Database.Terms.All ()
                        .Skip (start.Value));
                  }
               }
               else
               {
                  TermSearch.AddOrUpdateIndex (this.Database.Terms.All ());
               }
               TermSearch.Optimize ();
            }
            else if (type == "glossary")
            {
               num = this.Database.TermItemEntries.All ().Count ();
               if (start != null && start.Value != -1)
               {
                  if (length != null && length.Value != -1)
                  {
                     GlossarySearch.AddOrUpdateIndex (this.Database.TermItemEntries.All ()
                        .OrderBy (e => e.Item.Term.Name)
                        .ThenBy (e => e.Order)
                        .Skip (start.Value)
                        .Take (length.Value));
                  }
                  else
                  {
                     GlossarySearch.AddOrUpdateIndex (this.Database.TermItemEntries.All ()
                        .OrderBy (e => e.Item.Term.Name)
                        .ThenBy (e => e.Order)
                        .Skip (start.Value));
                  }
               }
               else
               {
                  GlossarySearch.AddOrUpdateIndex (this.Database.TermItemEntries.All ()
                     .OrderBy (e => e.Item.Term.Name)
                     .ThenBy (e => e.Order));
               }
               GlossarySearch.Optimize ();
            }
            return Json (new {message="success", count=num});
         }
         catch (Exception ex)
         {
            return this.Fail (ex.Message);
         }
      }

      /// <summary>
      /// Adds index data to search.
      /// </summary>
      /// <returns>Index data view.</returns>
      [Authorize (Roles = "Administrator")]
      public ActionResult IndexData ()
      {
         return View ();
      }

      /// <summary>
      /// Attempts to parse the given query.
      /// </summary>
      /// <param name="search">Search query.</param>
      /// <returns>The results.</returns>
      public ActionResult Parse (SearchQueryViewModel search)
      {
         if (string.IsNullOrWhiteSpace (search.Query))
         {
            Response.StatusCode = 500;
            return Json ("Please specify a query for searching.");
         }

         var parser = new PassageParser (this.Database);
         parser.Parse (search.QDecoded);
         var results = new SearchResultsViewModel (search)
         {
            ParserLog = parser.Log,
            Title = "Passages",
            SearchType = SearchType.Parse,
            TotalHits = parser.PassageList.Count
         };
         if (parser.PassageList.Count > 0)
         {
            var host = Request.Url == null ?  string.Empty : Request.Url.AbsoluteUri.Replace (Request.Url.AbsolutePath, "");
            foreach (var passage in parser.PassageList)
            {
               results.Add (new SearchResult
               {
                  Id = passage.Id.ToString (CultureInfo.InvariantCulture),
                  Title = passage.GetTitle (),
                  Url = string.Format ("{0}/Read/{1}#num_{2}", host, passage.Chapter.Id, passage.Number),
                  Description = passage.Passage.Text
               });
            }
         }
         return PartialView ("Query", results);
      }

      /// <summary>
      /// Searches books for the query.
      /// </summary>
      /// <param name="search">Search query.</param>
      /// <returns>The results.</returns>
      public ActionResult Books (SearchQueryViewModel search)
      {
         if (string.IsNullOrWhiteSpace (search.Query))
         {
            Response.StatusCode = 500;
            return Json ("Please specify a query for searching.");
         }

         var host = Request.Url == null ? string.Empty : Request.Url.AbsoluteUri.Replace (Request.Url.AbsolutePath, "");
         var results = new SearchResultsViewModel (search);
         BookSearch.Query (search, results, host);
         return PartialView ("Query", results);
      }

      /// <summary>
      /// Searches passages for the query.
      /// </summary>
      /// <param name="search">Search query.</param>
      /// <returns>The results.</returns>
      public ActionResult Passages (SearchQueryViewModel search)
      {
         if (string.IsNullOrWhiteSpace (search.Query))
         {
            Response.StatusCode = 500;
            return Json ("Please specify a query for searching.");
         }

         var host = Request.Url == null ? string.Empty : Request.Url.AbsoluteUri.Replace (Request.Url.AbsolutePath, "");
         var results = new SearchResultsViewModel (search) { ShowEmpty = true };
         PassageSearch.Query (search, results, host);
         return PartialView ("Query", results);
      }

      /// <summary>
      /// Searches terms for the query.
      /// </summary>
      /// <param name="search">Search query.</param>
      /// <returns>The results.</returns>
      public ActionResult Terms (SearchQueryViewModel search)
      {
         if (string.IsNullOrWhiteSpace (search.Query))
         {
            Response.StatusCode = 500;
            return Json ("Please specify a query for searching.");
         }

         var host = Request.Url == null ? string.Empty : Request.Url.AbsoluteUri.Replace (Request.Url.AbsolutePath, "");
         var results = new SearchResultsViewModel (search);
         TermSearch.Query (search, results, host);
         return PartialView ("Query", results);
      }

      /// <summary>
      /// Searches the glossary for the query.
      /// </summary>
      /// <param name="search">Search query.</param>
      /// <returns>The results.</returns>
      public ActionResult Glossary (SearchQueryViewModel search)
      {
         if (string.IsNullOrWhiteSpace (search.Query))
         {
            Response.StatusCode = 500;
            return Json ("Please specify a query for searching.");
         }

         var host = Request.Url == null ? string.Empty : Request.Url.AbsoluteUri.Replace (Request.Url.AbsolutePath, "");
         var results = new SearchResultsViewModel (search) { ShowEmpty = true };
         GlossarySearch.Query (search, results, host);
         return PartialView ("Query", results);
      }
   }
}
