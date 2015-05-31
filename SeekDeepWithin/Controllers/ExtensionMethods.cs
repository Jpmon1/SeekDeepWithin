using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SeekDeepWithin.Models;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Controllers
{
   public static class ExtensionMethods
   {
      /// <summary>
      /// Highlights the words in the given query.
      /// </summary>
      /// <param name="text">Text to highlight.</param>
      /// <param name="query">Query to hightlight.</param>
      /// <returns>Html text with highlighted words.</returns>
      public static string Highlight (this string text, SearchQueryViewModel query)
      {
         if (query.SearchType == 3) return text;
         var html = string.Empty;
         var words = query.QDecoded.GetWords ();
         var indexes = new Dictionary <int, int> ();
         foreach (var word in words)
         {
            var hilite = query.Exact ? "\\b" + Regex.Escape (word) + "\\b" : Regex.Escape (word);
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

      /// <summary>
      /// Gets the full title of the passage.
      /// </summary>
      /// <param name="entry">Passage to get title for.</param>
      /// <param name="url">The url, if requesting a link, null if link is not required.</param>
      /// <returns>The full title of the passage.</returns>
      public static string GetTitle (this PassageEntry entry, Uri url = null)
      {
         if (entry == null) return string.Empty;
         var title = string.Empty;
         if (url != null)
         {
            var host = url.AbsoluteUri.Replace (url.AbsolutePath, "");
            title += string.Format ("<a href=\"{0}/Read/{1}#num_{2}\">", host, entry.ChapterId, entry.Number);
         }
         title += entry.Chapter.SubBook.Version.Title + " | ";
         if (!entry.Chapter.SubBook.Hide)
            title += entry.Chapter.SubBook.SubBook.Name + " | ";
         if (!entry.Chapter.Hide)
            title += entry.Chapter.Chapter.Name + ":";
         title += entry.Number;
         if (url != null)
            title += "</a>";
         return title;
      }
   }
}