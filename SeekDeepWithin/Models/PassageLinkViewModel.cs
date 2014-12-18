﻿namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a passage link.
   /// </summary>
   public class PassageLinkViewModel
   {
      /// <summary>
      /// Gets or Sets the start index of the link.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the link.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets the url of the link.
      /// </summary>
      public string Url { get; set; }

      /// <summary>
      /// Gets or Sets if this link should open in a new window or not.
      /// </summary>
      public bool OpenInNewWindow { get; set; }
   }
}