using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SeekDeepWithin.Controllers
{
   public static class ExtensionMethods
   {
      /// <summary>
      /// Highlights the words in the given query.
      /// </summary>
      /// <param name="text">Text to highlight.</param>
      /// <param name="words">Words to hightlight.</param>
      /// <returns>Html text with highlighted words.</returns>
      public static string Highlight (this string text, IEnumerable <string> words)
      {
         var html = string.Empty;
         var indexes = new Dictionary <int, int> ();
         foreach (var word in words)
         {
            var hilite = Regex.Escape (word);
            var matches = Regex.Matches (text, hilite, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
               if (indexes.ContainsKey (match.Index))
                  indexes [match.Index] = Math.Max (indexes [match.Index], match.Length);
               else
                  indexes.Add(match.Index, match.Length);
            }
         }
         var index = 0;
         foreach (var kvp in indexes.OrderBy(i => i.Key))
         {
            html += text.Substring (index, kvp.Key - index);
            html += "<span style=\"background-color:#A0D3E8\">";
            html += text.Substring (kvp.Key, kvp.Value);
            html += "</span>";
            index = kvp.Key + kvp.Value;
         }
         html += text.Substring (index);
         return html;
      }

      /// <summary>
      /// Chops the given text up into words, removing punctuation.
      /// </summary>
      /// <param name="text">Text to get words from.</param>
      /// <returns>The list of words in the given text.</returns>
      public static List<string> GetWords (this string text)
      {
         var punctuation = text.Where (Char.IsPunctuation).Distinct ().ToArray ();
         return text.Split ().Select (x => x.Trim (punctuation)).ToList ();
      }
   }
}