using System;
using System.Text;
using SeekDeepWithin.DataAccess;

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

      public static string Base64Decode (string encodedText)
      {
         var bytes = Convert.FromBase64String (encodedText);
         return Encoding.UTF8.GetString (bytes);
      }
   }
}