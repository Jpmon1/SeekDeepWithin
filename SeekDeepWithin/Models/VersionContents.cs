using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents the table of contents for a version.
   /// </summary>
   public class VersionContents
   {
      private readonly List<SubBookContent> m_SubBooks = new List<SubBookContent> ();

      /// <summary>
      /// Initializes a new version content item.
      /// </summary>
      /// <param name="name">The name of the version.</param>
      /// <param name="contents">The contents to parse.</param>
      /// <param name="versionId">The id of the version.</param>
      public VersionContents (string name, string contents, int versionId)
      {
         this.Name = name;
         this.VersionId = versionId;
         this.SetContents (contents);
      }

      /// <summary>
      /// Gets or Sets the name of the version.
      /// </summary>
      public int VersionId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the version.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets the current sub book.
      /// </summary>
      public int CurrentSubBook { get; set; }

      /// <summary>
      /// Gets or Sets the current chapter
      /// </summary>
      public int CurrentChapter { get; set; }

      /// <summary>
      /// Gets the list of sub books.
      /// </summary>
      public List<SubBookContent> SubBooks { get { return this.m_SubBooks; } }

      /// <summary>
      /// Sets the json string of contents.
      /// </summary>
      /// <param name="contents"></param>
      public void SetContents (string contents)
      {
         this.SubBooks.Clear();
         if (!string.IsNullOrWhiteSpace (contents))
         {
            dynamic dContents = JArray.Parse (contents);
            if (dContents.Count > 0)
            {
               foreach (var subBook in dContents)
               {
                  var hideSb = subBook.hide ?? new JValue (false);
                  var subBookContent = new SubBookContent {Name = subBook.name, Hide = hideSb.Value, Id = subBook.id};
                  if (subBook.chapters.Count > 0)
                  {
                     foreach (var chapter in subBook.chapters)
                     {
                        var hideCh = chapter.hide ?? new JValue (false);
                        subBookContent.Chapters.Add (new ChapterContent
                        {
                           Id = chapter.id,
                           Name = chapter.name,
                           Hide = hideCh.Value
                        });
                     }
                  }
                  this.SubBooks.Add(subBookContent);
               }
            }
         }
      }
   }

   /// <summary>
   /// Represents a content (TOC) item for a sub book.
   /// </summary>
   public class SubBookContent
   {
      private readonly List<ChapterContent> m_Chapters = new List<ChapterContent> ();

      /// <summary>
      /// Gets or Sets the id of the sub book content.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the sub book.
      /// </summary>
      public int ItemId { get; set; }

      /// <summary>
      /// Gets or Sets if we should show the sub book or not.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets or Sets the name of the sub book.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets the list of chapters.
      /// </summary>
      public List<ChapterContent> Chapters { get { return this.m_Chapters; } }
   }

   /// <summary>
   /// Represents a content (TOC) item for a chapter.
   /// </summary>
   public class ChapterContent
   {
      /// <summary>
      /// Gets or Sets the id of the chapter content.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the chapter.
      /// </summary>
      public int ItemId { get; set; }

      /// <summary>
      /// Gets or Sets if we should show the chapter or not.
      /// </summary>
      public bool Hide { get; set; }

      /// <summary>
      /// Gets or Sets the name of the chapter.
      /// </summary>
      public string Name { get; set; }
   }
}