﻿using System.Collections.Generic;
using DotNetOpenAuth.Messaging;
using SeekDeepWithin.Controllers;

namespace SeekDeepWithin.Models
{
   public class PassageEntryViewModel
   {
      private readonly Dictionary<int, int> m_Insertions = new Dictionary<int, int> ();

      /// <summary>
      /// Gets or Sets the id of the passage entry.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the passage id.
      /// </summary>
      public int PassageId { get; set; }

      /// <summary>
      /// Gets or Sets the chapter id.
      /// </summary>
      public int ChapterId { get; set; }

      /// <summary>
      /// Gets or Sets the number of the passage entry.
      /// </summary>
      public int Number { get; set; }

      /// <summary>
      /// Gets or Sets the order of the passage entry.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the chapter of this entry.
      /// </summary>
      public ChapterViewModel Chapter { get; set; }

      /// <summary>
      /// Gets or Sets the passage of this entry.
      /// </summary>
      public PassageViewModel Passage { get; set; }

      /// <summary>
      /// Renders the passage entry.
      /// </summary>
      /// <returns>The rendered html of the passage entry.</returns>
      public string Render ()
      {
         var renderer = new SdwRenderer {Text = this.Passage.Text};
         renderer.Links.AddRange(this.Passage.PassageLinks);
         return renderer.Render();
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