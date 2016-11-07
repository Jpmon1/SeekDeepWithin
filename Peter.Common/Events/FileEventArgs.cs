using System;

namespace Peter.Common.Events
{
   /// <summary>
   /// Event handler for file event arguments.
   /// </summary>
   /// <param name="sender">Object that caused the event.</param>
   /// <param name="args">FileEventArgs</param>
   public delegate void FileEventHandler (object sender, FileEventArgs args);

   /// <summary>
   /// Represents event arguments for a file.
   /// </summary>
   public class FileEventArgs : EventArgs
   {
      /// <summary>
      /// Initializes a new text event argument.
      /// </summary>
      /// <param name="filePath">The path to the file that raised the event.</param>
      public FileEventArgs (string filePath)
      {
         this.FilePath = filePath;
      }

      /// <summary>
      /// Gets the text that raised the event.
      /// </summary>
      public string FilePath { get; private set; }
   }
}
