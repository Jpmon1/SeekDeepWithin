using System.Collections.Generic;
using System.Collections.ObjectModel;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Renders content for sdw.
   /// </summary>
   public class SdwRenderer
   {
      private string m_Html;
      private readonly Dictionary<int, int> m_Insertions = new Dictionary<int, int> ();

      /// <summary>
      /// Initializes the renderer.
      /// </summary>
      public SdwRenderer ()
      {
         this.Links = new Collection<LinkViewModel> ();
         this.Styles = new Collection<StyleViewModel> ();
      }

      /// <summary>
      /// Gets or Sets the text ot render.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets the collection of links.
      /// </summary>
      public ICollection<LinkViewModel> Links { get; private set; }

      /// <summary>
      /// Gets the collection of styles.
      /// </summary>
      public ICollection<StyleViewModel> Styles { get; private set; }

      /// <summary>
      /// Renders the passage entry.
      /// </summary>
      /// <returns>The rendered html of the passage entry.</returns>
      public string Render ()
      {
         this.m_Insertions.Clear ();
         this.m_Html = this.Text;
         foreach (var link in this.Links)
         {
            var start = "<a href=\"" + link.Url + "\"";
            if (link.OpenInNewWindow)
               start += " target=\"_blank\"";
            start += ">";
            this.Insert (start, "</a>", link.StartIndex, link.EndIndex);
         }

         foreach (var style in this.Styles)
            this.Insert (style.Start, style.End, style.StartIndex, style.EndIndex);

         return this.m_Html;
      }

      /// <summary>
      /// Inserts the given tag information at the given indexes.
      /// </summary>
      /// <param name="start">Beginning tag information.</param>
      /// <param name="end">Ending tag information.</param>
      /// <param name="startIndex">Start insertion index.</param>
      /// <param name="endIndex">End insertion index.</param>
      private void Insert (string start, string end, int startIndex, int endIndex)
      {
         var index = this.GetInsertionIndex (startIndex);
         this.m_Html = this.m_Html.Insert (index, start);
         this.AddLength (startIndex, start.Length);

         index = this.GetInsertionIndex (endIndex);
         this.m_Html = this.m_Html.Insert (index, end);
         this.AddLength (endIndex, end.Length);
      }

      /// <summary>
      /// Adds length the the insertion dictionary.
      /// </summary>
      /// <param name="index">Index to add length for.</param>
      /// <param name="length">The amount of length to add.</param>
      private void AddLength (int index, int length)
      {
         if (this.m_Insertions.ContainsKey (index))
            this.m_Insertions[index] += length;
         else
            this.m_Insertions.Add (index, length);
      }

      /// <summary>
      /// Gets the insertion index for the given index based on previous insertions.
      /// </summary>
      /// <param name="index">Index to start.</param>
      /// <param name="doBefore">Index returned will come before a conflicting index.</param>
      /// <returns>The corrected start index.</returns>
      private int GetInsertionIndex (int index, bool doBefore = false)
      {
         int addIndex = index;
         foreach (var kvp in this.m_Insertions)
         {
            if (doBefore)
            {
               if (kvp.Key < index)
                  addIndex += kvp.Value;
            }
            else
            {
               if (kvp.Key <= index)
                  addIndex += kvp.Value;
            }
         }
         return addIndex;
      }
   }
}