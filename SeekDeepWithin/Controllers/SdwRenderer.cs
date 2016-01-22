using System.Collections.Generic;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Controllers
{
   /// <summary>
   /// Renders content for sdw.
   /// </summary>
   public class SdwRenderer
   {
      private string m_Html;
      private int m_FooterNumber = 1;
      private readonly Dictionary<int, int> m_Insertions = new Dictionary<int, int> ();

      /// <summary>
      /// Gets or Sets the searched text.
      /// </summary>
      public string SearchText { get; set; }

      /// <summary>
      /// Renders the passage entry.
      /// </summary>
      /// <returns>The rendered html of the passage entry.</returns>
      public string Render (SdwItem item)
      {
         this.m_Insertions.Clear ();
         this.m_Html = item.Text;

         //ViewBag.Search == null ? item.Text : item.Text.Highlight((IEnumerable <string>)ViewBag.Search);
         foreach (var style in item.Styles)
            this.Insert (style.Start, style.StartIndex, style.End, style.EndIndex);

         foreach (var footer in item.Footers) {
            if (footer.Order.HasValue) {
               footer.Number = this.m_FooterNumber;
               this.Insert (string.Format ("<sup>({0})</sup>", this.m_FooterNumber), -footer.Order.Value);
               this.m_FooterNumber++;
            }
         }

         return this.m_Html;
      }

      /// <summary>
      /// Inserts the given tag information at the given indexes.
      /// </summary>
      /// <param name="start">Beginning tag information.</param>
      /// <param name="end">Ending tag information.</param>
      /// <param name="startIndex">Start insertion index.</param>
      /// <param name="endIndex">End insertion index.</param>
      private void Insert (string start, int startIndex, string end = "", int endIndex = -1)
      {
         var index = this.GetInsertionIndex (startIndex);
         if (index > this.m_Html.Length) index = this.m_Html.Length;
         this.m_Html = this.m_Html.Insert (index, start);
         this.AddLength (startIndex, start.Length);

         if (!string.IsNullOrWhiteSpace (end) && endIndex > 0) {
            index = this.GetInsertionIndex (endIndex, true);
            if (index > this.m_Html.Length) index = this.m_Html.Length;
            this.m_Html = this.m_Html.Insert (index, end);
            this.AddLength (endIndex, end.Length);
         }
      }

      /// <summary>
      /// Adds length the the insertion dictionary.
      /// </summary>
      /// <param name="index">Index to add length for.</param>
      /// <param name="length">The amount of length to add.</param>
      private void AddLength (int index, int length)
      {
         if (this.m_Insertions.ContainsKey (index))
            this.m_Insertions [index] += length;
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
         foreach (var kvp in this.m_Insertions) {
            if (doBefore) {
               if (kvp.Key < index)
                  addIndex += kvp.Value;
            } else {
               if (kvp.Key <= index)
                  addIndex += kvp.Value;
            }
         }
         return addIndex;
      }
   }
}
