using System.Collections.Generic;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model used to display a love with truths.
   /// </summary>
   public class LoveViewModel
   {
      private readonly Dictionary<int, string> m_Tags = new Dictionary<int, string> ();
      private readonly Dictionary<int, string> m_Books = new Dictionary<int, string> ();
      private readonly Dictionary<int, string> m_Passages = new Dictionary<int, string> ();
      private readonly Dictionary<int, string> m_Summaries = new Dictionary<int, string> ();

      /// <summary>
      /// Gets the list of tags.
      /// </summary>
      public Dictionary<int, string> Tags { get { return this.m_Tags; } }

      /// <summary>
      /// Gets the list of books.
      /// </summary>
      public Dictionary<int, string> Books { get { return this.m_Books; } }

      /// <summary>
      /// Gets the list of passages.
      /// </summary>
      public Dictionary<int, string> Passages { get { return this.m_Passages; } }

      /// <summary>
      /// Gets the list of summaries.
      /// </summary>
      public Dictionary<int, string> Summaries { get { return this.m_Summaries; } }
   }
}