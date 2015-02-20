using System;

namespace SeekDeepWithin.Pocos
{
   public class Comment : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the comment.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets the user id that created this comment.
      /// </summary>
      public int UserId { get; set; }

      /// <summary>
      /// Gets or Sets the text of the comment.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the date and time of the comment's creation.
      /// </summary>
      public DateTime CreationDateTime { get; set; }

      /// <summary>
      /// Gets or Sets the date and time of the comment's modification.
      /// </summary>
      public DateTime ModifiedDateTime { get; set; }
   }
}