using System.Linq;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public static class DbHelper
   {
      /// <summary>
      /// Gets a sub book with the given name.
      /// </summary>
      /// <param name="db">Database object.</param>
      /// <param name="name">Name of sub book.</param>
      /// <param name="bookId"></param>
      /// <returns>A sub book.</returns>
      public static SubBook GetSubBook (ISdwDatabase db, string name, int bookId)
      {
         var subBook = db.SubBooks.Get (sb => sb.Name == name && sb.Book.Id == bookId).FirstOrDefault();
         if (subBook == null)
         {
            var book = db.Books.Get (bookId);
            subBook = new SubBook { Name = name, Book = book };
            db.SubBooks.Insert(subBook);
            db.Save();
         }
         return subBook;
      }

      /// <summary>
      /// Gets a chapter with the given name.
      /// </summary>
      /// <param name="db">Database object.</param>
      /// <param name="name">Name of chapter.</param>
      /// <returns>A chapter.</returns>
      public static Chapter GetChapter (ISdwDatabase db, string name)
      {
         var chapter = db.Chapters.Get (c => c.Name == name).FirstOrDefault ();
         if (chapter == null)
         {
            chapter = new Chapter { Name = name };
            db.Chapters.Insert (chapter);
            db.Save ();
         }
         return chapter;
      }
   }
}