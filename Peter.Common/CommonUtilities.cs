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
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using Peter.Common.Icons;

namespace Peter.Common
{
   /// <summary>
   /// Common utility methods.
   /// </summary>
   public static class CommonUtilities
   {
      private static bool? s_IsInDesignMode;
      private static readonly Random s_Random;

      /// <summary>
      /// Static constructor
      /// </summary>
      static CommonUtilities ()
      {
         s_Random = new Random ((int)DateTime.Now.Ticks);
      }

      /// <summary>
      /// Gets if we are in design mode or not, needed to disable certain operations.
      /// </summary>
      public static bool IsInDesignMode
      {
         get
         {
            if (!s_IsInDesignMode.HasValue)
            {
               s_IsInDesignMode = (bool)DependencyPropertyDescriptor.FromProperty (DesignerProperties.IsInDesignModeProperty, typeof (FrameworkElement))
                  .Metadata.DefaultValue;
            }

            return s_IsInDesignMode.Value;
         }
      }

      /// <summary>
      /// Gets the directory the assembly is executing in.
      /// </summary>
      /// <param name="assem">Assembly to get executing directory for.
      /// If null, the currently executing assembly will be uses.</param>
      /// <returns>The requested directory.</returns>
      public static string GetAssemblyDirectory (Assembly assem = null)
      {
         if (assem == null)
            assem = Assembly.GetExecutingAssembly ();

         var path = assem.Location;
         return path.Substring (0, path.LastIndexOf ("\\", StringComparison.Ordinal));
      }

      /// <summary>
      /// Generates a random string.
      /// </summary>
      /// <param name="length">The length of the string.</param>
      /// <returns>A randomly generated string.</returns>
      public static string RandomString (int length)
      {
         var builder = new StringBuilder ();
         for (int i = 0; i < length; i++)
         {
            var ch = Convert.ToChar (Convert.ToInt32 (Math.Floor (26 * s_Random.NextDouble () + 65)));
            builder.Append (ch);
         }

         return builder.ToString ();
      }

      private const int SW_SHOW = 5;
      private const uint SEE_MASK_INVOKEIDLIST = 12;
      /// <summary>
      /// Shows the windows file properties dialog.
      /// </summary>
      /// <param name="filename">Full path to file to show properties for.</param>
      /// <returns>True if successful, otherwise false.</returns>
      public static bool ShowFileProperties (string filename)
      {
         var info = new Shell32.SHELLEXECUTEINFO ();
         info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf (info);
         info.lpVerb = "properties";
         info.lpFile = filename;
         info.nShow = SW_SHOW;
         info.fMask = SEE_MASK_INVOKEIDLIST;
         return Shell32.ShellExecuteEx (ref info);
      }

      /// <summary>
      /// Gets if the given extension is xml or not.
      /// </summary>
      /// <param name="extension">The extension of the file (xml or .xml).</param>
      /// <returns>True if  the extension is a known xml file, otherwise false.</returns>
      public static bool IsXml (string extension)
      {
         extension = extension.ToLower ();
         if (extension.StartsWith ("."))
            extension = extension.Substring (1);
         if (extension == "xml" ||
             extension == "xsl" ||
             extension == "xslt" ||
             extension == "xsd" ||
             extension == "manifest" ||
             extension == "config" ||
             extension == "addin" ||
             extension == "xshd" ||
             extension == "wxs" ||
             extension == "wxi" ||
             extension == "wxl" ||
             extension == "proj" ||
             extension == "csproj" ||
             extension == "vbproj" ||
             extension == "ilproj" ||
             extension == "booproj" ||
             extension == "build" ||
             extension == "xfrm" ||
             extension == "targets" ||
             extension == "xaml" ||
             extension == "xpt" ||
             extension == "xft" ||
             extension == "map" ||
             extension == "wsdl" ||
             extension == "disco" ||
             extension == "ps1xml" ||
             extension == "nuspec" ||
             extension == "vcxproj")
            return true;
         return false;
      }

      /// <summary>
      /// Makes the given file name valid, stripping faulty characters.
      /// </summary>
      /// <param name="name">File name to make valid.</param>
      /// <returns>A valid file name.</returns>
      public static string MakeValidFileName (string name)
      {
         string invalidChars = System.Text.RegularExpressions.Regex.Escape (new string (Path.GetInvalidFileNameChars ()));
         string invalidRegStr = string.Format (@"([{0}]*\.+$)|([{0}]+)", invalidChars);

         return System.Text.RegularExpressions.Regex.Replace (name, invalidRegStr, "");
      }
   }
}
