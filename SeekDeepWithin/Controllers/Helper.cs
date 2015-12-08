using System;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;
using Version = SeekDeepWithin.Pocos.Version;

namespace SeekDeepWithin.Controllers
{
   public static class Helper
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
            version.SubBooks.OrderBy (sb => sb.Order).Select (
               sb =>
                  new {
                     name = String.IsNullOrWhiteSpace (sb.Alias) ? sb.Term.Name : sb.Alias,
                     id = sb.Id,
                     termId = sb.Term.Id,
                     hide = sb.Hide,
                     chapters = sb.Chapters == null ? Enumerable.Repeat (new { name = String.Empty, id = 0, hide = true }, 1) :
                     sb.Chapters.OrderBy (c => c.Order).Select (c =>
                        new {
                           name = c.Chapter.Name,
                           id = c.Id,
                           hide = c.Hide
                        })
                  });
         version.Modified = DateTime.Now;
         version.Contents = Json.Encode (contents);
         db.Save ();
      }

      /// <summary>
      /// Encodes the given string to a base 64 string.
      /// </summary>
      /// <param name="text">Text to encode.</param>
      /// <returns>A base 64 string.</returns>
      public static string Base64Encode (string text)
      {
         var bytes = Encoding.UTF8.GetBytes (text);
         return Convert.ToBase64String (bytes).TrimEnd ('=');
      }

      public static string Base64Decode (string encodedText)
      {
         var bytes = Convert.FromBase64String (encodedText);
         return Encoding.UTF8.GetString (bytes);
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
         if (chapter == null) {
            chapter = new Chapter { Name = name };
            db.Chapters.Insert (chapter);
         }
         return chapter;
      }
   }
}