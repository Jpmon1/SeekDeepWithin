/**
 * Peter
 * Created by: Peter Development Team
 *    http://peter.codeplex.com/
 * 
 * GNU General Public License version 2 (GPLv2)
 *    http://peter.codeplex.com/license
 *
 *  This code is provided on an AS IS basis, with no WARRANTIES,
 *  CONDITIONS or GUARANTEES of any kind.
 *  
 **/

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace Peter.Common.Utilities
{
   /// <summary>
   /// Interaction operations for files and the clipboard.
   /// </summary>
   public static class ClipboardFiles
   {
      /// <summary>
      /// Peforms a copy action on the given files.
      /// </summary>
      /// <param name="files">List of files to copy.</param>
      public static void Copy (string[] files)
      {
         Copy (files, false);
      }

      /// <summary>
      /// Peforms a cut action on the given files.
      /// </summary>
      /// <param name="files">List of files to cut.</param>
      public static void Cut (string[] files)
      {
         Copy (files, true);
      }

      /// <summary>
      /// Peforms the cut or copy command on the list of given files.
      /// </summary>
      /// <param name="files">List of files to cut or copy.</param>
      /// <param name="cut">True if we are cutting the files, otherwise false.</param>
      private static void Copy (string[] files, bool cut)
      {
         if (files != null)
         {
            var data = new DataObject (DataFormats.FileDrop, files, true);
            var memo = new MemoryStream (4);
            var bytes = new byte[] { (byte)(cut ? 2 : 5), 0, 0, 0 };
            memo.Write (bytes, 0, bytes.Length);
            data.SetData ("Preferred DropEffect", memo);
            Clipboard.SetDataObject (data);
         }
      }

      /// <summary>
      /// Checks to see if a paste files operation can be performed.
      /// </summary>
      /// <returns>True if there are files on the clipboard ready to be pasted, otherwise false.</returns>
      public static bool CanPaste ()
      {
         var data = Clipboard.GetDataObject ();
         if (data == null || !data.GetDataPresent (DataFormats.FileDrop)) return false;
         var files = (string[])data.GetData (DataFormats.FileDrop);
         var stream = (MemoryStream)data.GetData ("Preferred DropEffect", true);
         int flag = stream.ReadByte ();
         if (flag != 2 && flag != 5) return false;
         return files.Length > 0;
      }

      /// <summary>
      /// Pastes the files in the clipboard in the given folder.
      /// </summary>
      /// <param name="destination">The destination folder to paste files into.</param>
      /// <returns>The list of newly pasted files.</returns>
      public static Collection <string> Paste (string destination)
      {
         var data = Clipboard.GetDataObject ();
         if (data == null || !data.GetDataPresent (DataFormats.FileDrop))return null;
         var files = (string[]) data.GetData (DataFormats.FileDrop);
         var stream = (MemoryStream) data.GetData ("Preferred DropEffect", true);
         int flag = stream.ReadByte ();
         if (flag != 2 && flag != 5)return null;
         bool cut = (flag == 2);
         var destFiles = new Collection <string> ();
         foreach (var file in files)
         {
            var currDir = Path.GetDirectoryName (file);
            var fileName = Path.GetFileName (file);
            if (currDir != null && currDir.ToLower () == destination.ToLower ())
               fileName = "Copy of " + fileName;
            string dest = destination + "\\" + fileName;
            try
            {
               if (cut)
                  File.Move (file, dest);
               else
                  File.Copy (file, dest, false);
               destFiles.Add (dest);
            }
            catch (IOException ex)
            {
               Console.Error.WriteLine (ex.Message);
            }
         }
         return destFiles;
      }
   }
}
