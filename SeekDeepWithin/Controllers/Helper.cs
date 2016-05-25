using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public static class Helper
   {

      /// <summary>
      /// Encodes the given string to a base 64 string.
      /// </summary>
      /// <param name="text">Text to encode.</param>
      /// <returns>A base 64 string.</returns>
      public static string Base64Encode (string text)
      {
         var bytes = Encoding.UTF8.GetBytes (text);
         return Convert.ToBase64String (bytes);
      }

      /// <summary>
      /// Decodes the given string from base 64 to text.
      /// </summary>
      /// <param name="encodedText">The encoded string.</param>
      /// <returns>The decoded string.</returns>
      public static string Base64Decode (string encodedText)
      {
         var bytes = Convert.FromBase64String (encodedText);
         return Encoding.UTF8.GetString (bytes);
      }

      /// <summary>
      /// Finds or creates the love with the given peace.
      /// </summary>
      /// <param name="db">The database connection.</param>
      /// <param name="light">The id of the light.</param>
      /// <param name="create">True (default) to create a new love if non-existant, otherwise false.</param>
      /// <returns>The love.</returns>
      public static Love FindLove (ISdwDatabase db, int light, bool create = true)
      {
         return FindLove (db, new [] { light }, create);
      }

      /// <summary>
      /// Finds or creates the love with the given peaces.
      /// </summary>
      /// <param name="db">The database connection.</param>
      /// <param name="lights">The list of light ids.</param>
      /// <param name="create">True (default) to create a new love if non-existant, otherwise false.</param>
      /// <returns>The love.</returns>
      public static Love FindLove (ISdwDatabase db, IEnumerable<int> lights, bool create = true)
      {
         var hash = new Hashids ("GodisLove");
         return FindLove (db, hash.Encode (lights), create);
      }

      /// <summary>
      /// Finds or creates the love with the given peaces.
      /// </summary>
      /// <param name="db">The database connection.</param>
      /// <param name="light">The hash list of lights.</param>
      /// <param name="create">True (default) to create a new love if non-existant, otherwise false.</param>
      /// <returns>The love.</returns>
      public static Love FindLove (ISdwDatabase db, string light, bool create = true)
      {
         var hash = new Hashids ("GodisLove");
         var peaces = hash.Decode (light);
         if (peaces.Length <= 0) return null;
         hash.Order = true;
         var orderPeace = hash.Encode (peaces);
         var love = db.Love.Get (l => l.PeaceId == orderPeace).FirstOrDefault ();
         if (love == null && create) {
            love = new Love { Modified = DateTime.Now, PeaceId = orderPeace, Truths = new HashSet<Truth> (), Peaces = new HashSet<Peace> () };
            int index = 0;
            foreach (var p in peaces) {
               love.Peaces.Add (new Peace { Order = index, Light = db.Light.Get (p) });
               index++;
            }
            db.Love.Insert (love);
            db.Save ();
         }
         if (love != null && love.Truths == null) love.Truths = new HashSet<Truth> ();
         return love;
      }
   }
}