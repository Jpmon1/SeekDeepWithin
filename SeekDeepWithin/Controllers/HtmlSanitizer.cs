using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace SeekDeepWithin.Controllers
{
   public static class HtmlSanitizer
   {
      private static readonly IDictionary<string, string[]> s_Whitelist;
      private static readonly List<string> s_DeletableNodesXpath = new List<string> ();

      static HtmlSanitizer ()
      {
         s_Whitelist = new Dictionary<string, string[]> {
                { "a", new[] { "href" } },
                { "strong", null },
                { "em", null },
                { "blockquote", null },
                { "b", null},
                { "p", null},
                { "ul", null},
                { "ol", null},
                { "li", null},
                { "div", new[] { "align" } },
                { "strike", null},
                { "u", null},                
                { "sub", null},
                { "sup", null},
                { "table", null },
                { "tr", null },
                { "td", null },
                { "th", null }
                };
      }

      public static string Sanitize (string input)
      {
         if (string.IsNullOrEmpty (input))
            return string.Empty;
         if (input.Trim ().Length < 1)
            return string.Empty;
         var htmlDocument = new HtmlDocument ();

         htmlDocument.LoadHtml (input);
         SanitizeNode (htmlDocument.DocumentNode);
         string xPath = CreateXPath ();

         return StripHtml (htmlDocument.DocumentNode.WriteTo ().Trim (), xPath);
      }

      private static void SanitizeChildren (HtmlNode parentNode)
      {
         for (int i = parentNode.ChildNodes.Count - 1; i >= 0; i--)
         {
            SanitizeNode (parentNode.ChildNodes[i]);
         }
      }

      private static void SanitizeNode (HtmlNode node)
      {
         if (node.NodeType == HtmlNodeType.Element)
         {
            if (!s_Whitelist.ContainsKey (node.Name))
            {
               if (!s_DeletableNodesXpath.Contains (node.Name))
               {
                  //DeletableNodesXpath.Add(node.Name.Replace("?",""));
                  node.Name = "removeableNode";
                  s_DeletableNodesXpath.Add (node.Name);
               }
               if (node.HasChildNodes)
               {
                  SanitizeChildren (node);
               }

               return;
            }

            if (node.HasAttributes)
            {
               for (int i = node.Attributes.Count - 1; i >= 0; i--)
               {
                  HtmlAttribute currentAttribute = node.Attributes[i];
                  string[] allowedAttributes = s_Whitelist[node.Name];
                  if (allowedAttributes != null)
                  {
                     if (!allowedAttributes.Contains (currentAttribute.Name))
                     {
                        node.Attributes.Remove (currentAttribute);
                     }
                  }
                  else
                  {
                     node.Attributes.Remove (currentAttribute);
                  }
               }
            }
         }

         if (node.HasChildNodes)
         {
            SanitizeChildren (node);
         }
      }

      private static string StripHtml (string html, string xPath)
      {
         var htmlDoc = new HtmlDocument ();
         htmlDoc.LoadHtml (html);
         if (xPath.Length > 0)
         {
            HtmlNodeCollection invalidNodes = htmlDoc.DocumentNode.SelectNodes (@xPath);
            if (invalidNodes != null)
            {
               foreach (HtmlNode node in invalidNodes)
                  node.ParentNode.RemoveChild (node, true);
            }
         }
         return htmlDoc.DocumentNode.WriteContentTo ();
      }

      private static string CreateXPath ()
      {
         string xPath = string.Empty;
         for (int i = 0; i < s_DeletableNodesXpath.Count; i++)
         {
            if (i != s_DeletableNodesXpath.Count - 1)
            {
               xPath += string.Format ("//{0}|", s_DeletableNodesXpath[i]);
            }
            else xPath += string.Format ("//{0}", s_DeletableNodesXpath[i]);
         }
         return xPath;
      }
   }
}