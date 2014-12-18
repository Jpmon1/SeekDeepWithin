using System;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View model for an error.
   /// </summary>
   public class ErrorViewModel
   {
      /// <summary>
      /// Gets or Sets the error code.
      /// </summary>
      public int HttpStatusCode { get; set; }

      /// <summary>
      /// Gets or Sets the exception of the error.
      /// </summary>
      public Exception Exception { get; set; }
   }
}