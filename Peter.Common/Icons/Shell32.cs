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
using System.Runtime.InteropServices;

namespace Peter.Common.Icons
{
   ///<summary>
   /// Provides exports from Shell32.dll
   /// </summary>
   public class Shell32
   {
      public const int MAX_PATH = 256;
      public const int NAME_SIZE = 80;

      [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 4)]
      public struct ShFileInfo
      {
         public IntPtr hIcon;
         public int iIcon;
         public uint dwAttributes;
         [MarshalAs (UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
         public string szDisplayName;
         [MarshalAs (UnmanagedType.ByValTStr, SizeConst = NAME_SIZE)]
         public string szTypeName;
      };

      public const uint SHGFI_ICON = 0x000000100;     // get icon
      public const uint SHGFI_DISPLAYNAME = 0x000000200;     // get display name
      public const uint SHGFI_TYPENAME = 0x000000400;     // get type name
      public const uint SHGFI_ATTRIBUTES = 0x000000800;     // get attributes
      public const uint SHGFI_ICONLOCATION = 0x000001000;     // get icon location
      public const uint SHGFI_EXETYPE = 0x000002000;     // return exe type
      public const uint SHGFI_SYSICONINDEX = 0x000004000;     // get system icon index
      public const uint SHGFI_LINKOVERLAY = 0x000008000;     // put a link overlay on icon
      public const uint SHGFI_SELECTED = 0x000010000;     // show icon in selected state
      public const uint SHGFI_ATTR_SPECIFIED = 0x000020000;     // get only specified attributes
      public const uint SHGFI_LARGEICON = 0x000000000;     // get large icon
      public const uint SHGFI_SMALLICON = 0x000000001;     // get small icon
      public const uint SHGFI_OPENICON = 0x000000002;     // get open icon
      public const uint SHGFI_SHELLICONSIZE = 0x000000004;     // get shell size icon
      public const uint SHGFI_PIDL = 0x000000008;     // pszPath is a pidl
      public const uint SHGFI_USEFILEATTRIBUTES = 0x000000010;     // use passed dwFileAttribute
      public const uint SHGFI_ADDOVERLAYS = 0x000000020;     // apply the appropriate overlays
      public const uint SHGFI_OVERLAYINDEX = 0x000000040;     // Get the index of the overlay

      public const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;
      public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

      ///<summary>See topic in Win32 SDK documentation</summary>
      [DllImport ("Shell32.dll", CharSet = CharSet.Auto)]
      public static extern IntPtr SHGetFileInfo (
          IntPtr pszPath,
          uint dwFileAttributes,
          ref ShFileInfo psfi,
          uint cbFileInfo,
          uint uFlags
          );

      [DllImport ("Shell32.dll", SetLastError = true)]
      public static extern int SHGetSpecialFolderLocation (
         IntPtr hwndOwner,
         Csidl nFolder,
         ref IntPtr ppidl);

      [DllImport ("shell32.dll", CharSet = CharSet.Auto)]
      public static extern bool ShellExecuteEx (ref SHELLEXECUTEINFO lpExecInfo);

      [StructLayout (LayoutKind.Sequential, CharSet = CharSet.Auto)]
      public struct SHELLEXECUTEINFO
      {
         public int cbSize;
         public uint fMask;
         public IntPtr hwnd;
         [MarshalAs (UnmanagedType.LPTStr)]
         public string lpVerb;
         [MarshalAs (UnmanagedType.LPTStr)]
         public string lpFile;
         [MarshalAs (UnmanagedType.LPTStr)]
         public string lpParameters;
         [MarshalAs (UnmanagedType.LPTStr)]
         public string lpDirectory;
         public int nShow;
         public IntPtr hInstApp;
         public IntPtr lpIDList;
         [MarshalAs (UnmanagedType.LPTStr)]
         public string lpClass;
         public IntPtr hkeyClass;
         public uint dwHotKey;
         public IntPtr hIcon;
         public IntPtr hProcess;
      }

      public enum Csidl
      {
         CSIDL_DESKTOP = 0x0000,    // <desktop>
         CSIDL_INTERNET = 0x0001,    // Internet Explorer (icon on desktop)
         CSIDL_PROGRAMS = 0x0002,    // Start Menu\Programs
         CSIDL_CONTROLS = 0x0003,    // My Computer\Control Panel
         CSIDL_PRINTERS = 0x0004,    // My Computer\Printers
         CSIDL_PERSONAL = 0x0005,    // My Documents
         CSIDL_FAVORITES = 0x0006,    // <user name>\Favorites
         CSIDL_STARTUP = 0x0007,    // Start Menu\Programs\Startup
         CSIDL_RECENT = 0x0008,    // <user name>\Recent
         CSIDL_SENDTO = 0x0009,    // <user name>\SendTo
         CSIDL_BITBUCKET = 0x000a,    // <desktop>\Recycle Bin
         CSIDL_STARTMENU = 0x000b,    // <user name>\Start Menu
         CSIDL_MYDOCUMENTS = 0x000c,    // logical "My Documents" desktop icon
         CSIDL_MYMUSIC = 0x000d,    // "My Music" folder
         CSIDL_MYVIDEO = 0x000e,    // "My Videos" folder
         CSIDL_DESKTOPDIRECTORY = 0x0010,    // <user name>\Desktop
         CSIDL_DRIVES = 0x0011,    // My Computer
         CSIDL_NETWORK = 0x0012,    // Network Neighborhood (My Network Places)
         CSIDL_NETHOOD = 0x0013,    // <user name>\nethood
         CSIDL_FONTS = 0x0014,    // windows\fonts
         CSIDL_TEMPLATES = 0x0015,
         CSIDL_COMMON_STARTMENU = 0x0016,    // All Users\Start Menu
         CSIDL_COMMON_PROGRAMS = 0X0017,    // All Users\Start Menu\Programs
         CSIDL_COMMON_STARTUP = 0x0018,    // All Users\Startup
         CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,    // All Users\Desktop
         CSIDL_APPDATA = 0x001a,    // <user name>\Application Data
         CSIDL_PRINTHOOD = 0x001b,    // <user name>\PrintHood
         CSIDL_LOCAL_APPDATA = 0x001c,    // <user name>\Local Settings\Applicaiton Data (non roaming)
         CSIDL_ALTSTARTUP = 0x001d,    // non localized startup
         CSIDL_COMMON_ALTSTARTUP = 0x001e,    // non localized common startup
         CSIDL_COMMON_FAVORITES = 0x001f,
         CSIDL_INTERNET_CACHE = 0x0020,
         CSIDL_COOKIES = 0x0021,
         CSIDL_HISTORY = 0x0022,
         CSIDL_COMMON_APPDATA = 0x0023,    // All Users\Application Data
         CSIDL_WINDOWS = 0x0024,    // GetWindowsDirectory()
         CSIDL_SYSTEM = 0x0025,    // GetSystemDirectory()
         CSIDL_PROGRAM_FILES = 0x0026,    // C:\Program Files
         CSIDL_MYPICTURES = 0x0027,    // C:\Program Files\My Pictures
         CSIDL_PROFILE = 0x0028,    // USERPROFILE
         CSIDL_SYSTEMX86 = 0x0029,    // x86 system directory on RISC
         CSIDL_PROGRAM_FILESX86 = 0x002a,    // x86 C:\Program Files on RISC
         CSIDL_PROGRAM_FILES_COMMON = 0x002b,    // C:\Program Files\Common
         CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,    // x86 Program Files\Common on RISC
         CSIDL_COMMON_TEMPLATES = 0x002d,    // All Users\Templates
         CSIDL_COMMON_DOCUMENTS = 0x002e,    // All Users\Documents
         CSIDL_COMMON_ADMINTOOLS = 0x002f,    // All Users\Start Menu\Programs\Administrative Tools
         CSIDL_ADMINTOOLS = 0x0030,    // <user name>\Start Menu\Programs\Administrative Tools
         CSIDL_CONNECTIONS = 0x0031,    // Network and Dial-up Connections
         CSIDL_COMMON_MUSIC = 0x0035,    // All Users\My Music
         CSIDL_COMMON_PICTURES = 0x0036,    // All Users\My Pictures
         CSIDL_COMMON_VIDEO = 0x0037,    // All Users\My Video
         CSIDL_CDBURN_AREA = 0x003b    // USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning
      }
   }
}
