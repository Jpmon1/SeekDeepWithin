using System.Text.RegularExpressions;

namespace SeekDeepWithin.Controllers
{
   public static class ExtensionMethods
   {
      public static string Highlight (this string text, string token)
      {
         var index = 0;
         var html = string.Empty;
         var matches = Regex.Matches (Regex.Unescape(text), token, RegexOptions.IgnoreCase);
         foreach (Match match in matches)
         {
            html += text.Substring (index, match.Index - index);
            html += "<span style=\"background-color:#A0D3E8\">";
            html += text.Substring (match.Index, match.Length);
            html += "</span>";
            index = match.Index + match.Length;
         }
         html += text.Substring (index);
         return html;
      }
   }
}