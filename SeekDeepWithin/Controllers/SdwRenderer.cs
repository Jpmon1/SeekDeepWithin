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
      private readonly Dictionary<int, int> m_Insertions = new Dictionary<int, int> ();

      /// <summary>
      /// Initializes the renderer.
      /// </summary>
      public SdwRenderer ()
      {
         this.Links = new Collection <LinkViewModel> ();
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
      /// Renders the passage entry.
      /// </summary>
      /// <returns>The rendered html of the passage entry.</returns>
      public string Render ()
      {
         this.m_Insertions.Clear ();
         var html = this.Text;
         foreach (var link in this.Links)
         {
            var index = this.GetInsertionIndex (link.StartIndex);
            var insertion = "<a href=\"" + link.Url + "\"";
            if (link.OpenInNewWindow)
               insertion += " target=\"_blank\"";
            insertion += ">";
            html = html.Insert (index, insertion);
            this.AddLength (link.StartIndex, insertion.Length);

            insertion = "</a>";
            index = this.GetInsertionIndex (link.EndIndex);
            html = html.Insert (index, insertion);
            this.AddLength (link.EndIndex, insertion.Length);
         }
         return html;
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