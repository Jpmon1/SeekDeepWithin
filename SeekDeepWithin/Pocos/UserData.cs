﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeekDeepWithin.Pocos
{
   public class UserData
   {
      [Key]
      [ForeignKey ("UserProfile")]
      public int UserProfileId { get; set; }

      /// <summary>
      /// Gets or Sets if we should show the edit actions for the user, if he/she is an editor.
      /// </summary>
      public bool? ShowEditActions { get; set; }

      /// <summary>
      /// Gets or Sets if we should highlight alernating rows while reading.
      /// </summary>
      public bool? HighlightAlternatingRows { get; set; }

      /// <summary>
      /// Get or Sets the associated user profile.
      /// </summary>
      public virtual UserProfile UserProfile { get; set; }
   }
}