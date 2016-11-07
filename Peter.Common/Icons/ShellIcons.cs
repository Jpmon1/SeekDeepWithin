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
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Peter.Common.Icons
{
   /// <summary>
   /// Class used to the sytem icons.
   /// </summary>
   public static class ShellIcons
   {
      /// <summary>
      /// Gets the icon's image source for the given file.
      /// </summary>
      /// <param name="file">File object to get icon for.</param>
      /// <param name="smallIcon">True to get the small icon, false for large icon.</param>
      /// <returns>The image source of the icon for the requested file.</returns>
      public static ImageSource GetFileIcon (FileInfo file, bool smallIcon)
      {
         return GetFileIcon (file.FullName, smallIcon);
      }
      /// <summary>
      /// Gets the icon's image source for the given file.
      /// </summary>
      /// <param name="file">File name or path to get icon for.</param>
      /// <param name="smallIcon">True to get the small icon, false for large icon.</param>
      /// <returns>The image source of the icon for the requested file.</returns>
      public static ImageSource GetFileIcon (string file, bool smallIcon)
      {
         ImageSource image = null;
         using (var icon = GetIcon (file, smallIcon, false, false))
         {
            if (icon != null)
            {
               image = Imaging.CreateBitmapSourceFromHIcon (icon.Handle,
                  new Int32Rect (0, 0, icon.Width, icon.Height),
                  BitmapSizeOptions.FromEmptyOptions ());
            }
         }
         return image ??
                new BitmapImage (new Uri ("pack://application:,,,/Peter.Common;Component/Images/blank_file.png"));
      }

      /// <summary>
      /// Gets the icon's image source for the given directory.
      /// </summary>
      /// <param name="directory">Directory object to get icon for, if null the standard folder icon is retrieved.</param>
      /// <param name="smallIcon">True to get the small icon, false for large icon.</param>
      /// <param name="isOpen">True if path id directory and the open directory icon is wanted.</param>
      /// <returns>The image source of the icon for the requested directory.</returns>
      public static ImageSource GetDirectoryIcon (DirectoryInfo directory, bool smallIcon, bool isOpen)
      {
         ImageSource image = null;
         using (var icon = GetIcon (directory == null ? "dummy" : directory.FullName, smallIcon, true, isOpen))
         {
            if (icon != null)
            {
               image = Imaging.CreateBitmapSourceFromHIcon (icon.Handle,
                  new Int32Rect (0, 0, icon.Width, icon.Height),
                  BitmapSizeOptions.FromEmptyOptions ());
            }
         }
         return image ??
                new BitmapImage (new Uri ("pack://application:,,,/Peter.Common;Component/Images/blank_file.png"));
      }

      /// <summary>
      /// Gets the icon for the given path (file or folder).
      /// </summary>
      /// <param name="path">Path to file or folder.</param>
      /// <param name="smallIcon">True to get the small icon, false for large icon.</param>
      /// <param name="isDirectory">True if path is directory, otherwise false.</param>
      /// <param name="isOpen">True if path id directory and the open directory icon is wanted.</param>
      /// <returns>The icon for the given path.</returns>
      public static Icon GetIcon (string path, bool smallIcon, bool isDirectory, bool isOpen)
      {
         var flags = Shell32.SHGFI_ICON | Shell32.SHGFI_USEFILEATTRIBUTES;
         var attribute = isDirectory ? Shell32.FILE_ATTRIBUTE_DIRECTORY : Shell32.SHGFI_ICONLOCATION;
         flags += (smallIcon) ? Shell32.SHGFI_SMALLICON : Shell32.SHGFI_LARGEICON;
         if (isDirectory && isOpen)
            flags += Shell32.SHGFI_OPENICON;

         var shfi = new Shell32.ShFileInfo { szDisplayName = string.Empty, szTypeName = string.Empty };
         var res = Shell32.SHGetFileInfo (Marshal.StringToBSTR (path), attribute, ref shfi, (uint)Marshal.SizeOf (shfi), flags);
         if (Equals (res, IntPtr.Zero)) throw Marshal.GetExceptionForHR (Marshal.GetHRForLastWin32Error ());
         try
         {
            Icon.FromHandle (shfi.hIcon);
            return (Icon)Icon.FromHandle (shfi.hIcon).Clone ();
         }
         catch
         {
            return null;
         }
         finally
         {
            User32.DestroyIcon (shfi.hIcon);
         }
      }

      /// <summary>
      /// Gets an image source for the given special folder.
      /// </summary>
      /// <param name="folder">Special folder.</param>
      /// <param name="smallIcon">True for small icon, otherwise false.</param>
      /// <returns>The image source of the icon for the requested special folder.</returns>
      public static ImageSource GetSpecialFolderIcon (Environment.SpecialFolder folder, bool smallIcon)
      {
         var shfi = new Shell32.ShFileInfo { szDisplayName = string.Empty, szTypeName = string.Empty };
         var ptrDir = IntPtr.Zero;
         Shell32.SHGetSpecialFolderLocation (IntPtr.Zero, GetSpecialFolderCsidl (folder), ref ptrDir);
         var flags = Shell32.SHGFI_ICON | Shell32.SHGFI_PIDL;
         flags += (smallIcon) ? Shell32.SHGFI_SMALLICON : Shell32.SHGFI_LARGEICON;
         var res = Shell32.SHGetFileInfo (ptrDir, 0, ref shfi, (uint)Marshal.SizeOf (shfi), flags);
         if (Equals (res, IntPtr.Zero)) throw Marshal.GetExceptionForHR (Marshal.GetHRForLastWin32Error ());
         Marshal.FreeCoTaskMem (ptrDir);
         var icon = (Icon)Icon.FromHandle (shfi.hIcon).Clone ();
         User32.DestroyIcon (shfi.hIcon);
         var image = Imaging.CreateBitmapSourceFromHIcon (icon.Handle,
            new Int32Rect (0, 0, icon.Width, icon.Height),
            BitmapSizeOptions.FromEmptyOptions ());
         icon.Dispose ();

         return image;
      }

      /// <summary>
      /// Gets the csidl for the given special folder.
      /// </summary>
      /// <param name="folder">Special folder.</param>
      /// <returns>Csidl for the given special folder.</returns>
      private static Shell32.Csidl GetSpecialFolderCsidl (Environment.SpecialFolder folder)
      {
         switch (folder)
         {
            case Environment.SpecialFolder.Desktop:
               return Shell32.Csidl.CSIDL_DESKTOP;
            default:
               return Shell32.Csidl.CSIDL_COMMON_DOCUMENTS;
         }
      }
   }
}
