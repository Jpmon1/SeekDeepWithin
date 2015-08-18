using System;
using System.Linq;
using System.Web.Helpers;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using Version = SeekDeepWithin.Pocos.Version;

namespace SeekDeepWithin.Controllers
{
   public static class DbHelper
   {
      /// <summary>
      /// Creates the Table of Contents for the given version.
      /// </summary>
      /// <param name="db">Database object.</param>
      /// <param name="version">Version to Create TOC for.</param>
      /// <returns>The dynamic object for the contents.</returns>
      public static void CreateToc (ISdwDatabase db, Version version)
      {
         var contents =
            version.SubBooks.OrderBy(sb => sb.Order).Select (
               sb =>
                  new
                  {
                     name = string.IsNullOrWhiteSpace(sb.Alias) ? sb.Term.Name : sb.Alias,
                     id = sb.Id,
                     termId = sb.Term.Id,
                     hide = sb.Hide,
                     chapters = sb.Chapters == null ? Enumerable.Repeat(new {name = string.Empty, id=0, hide=true}, 1) :
                     sb.Chapters.OrderBy(c => c.Order).Select (c =>
                        new
                        {
                           name = c.Chapter.Name,
                           id = c.Id,
                           hide = c.Hide
                        })
                  });
         version.Modified = DateTime.Now;
         version.Contents = Json.Encode (contents);
         db.Save();
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
         }
         return chapter;
      }
   }
}