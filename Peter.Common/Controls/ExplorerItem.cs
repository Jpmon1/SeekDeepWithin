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
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;
using Peter.Common.MainMenu;
using Peter.Common.Tree;

namespace Peter.Common.Controls
{
   /// <summary>
   /// Represents an item in the explorer.
   /// </summary>
   public class ExplorerItem : FileTreeViewItem
   {
      private string m_SelectedChildPath;
      private readonly bool m_ShowFiles;

      /// <summary>
      /// Initializes a file item from a given path.
      /// </summary>
      /// <param name="parent">Parent tree item.</param>
      /// <param name="file">The file item.</param>
      public ExplorerItem (ExplorerItem parent, FileInfo file)
         : base (parent, file)
      {
      }

      /// <summary>
      /// Initializes a new dummy explorer item.
      /// </summary>
      /// <param name="drive">The drive item.</param>
      /// <param name="showFiles">True if we want to show file when expanded.</param>
      public ExplorerItem (DriveInfo drive, bool showFiles)
         : base (null, drive.RootDirectory, true)
      {
         this.m_ShowFiles = showFiles;
         this.Icon = MainMenuIcon.Hdd;
         this.IconBrush = Brushes.SlateGray;
      }

      /// <summary>
      /// Initializes a file item from a given path.
      /// </summary>
      /// <param name="parent">Parent tree item.</param>
      /// <param name="directory">The directory item.</param>
      /// <param name="showFiles">True if we want to show file when expanded.</param>
      public ExplorerItem (ExplorerItem parent, DirectoryInfo directory, bool showFiles)
         : base (parent, directory, true)
      {
         this.m_ShowFiles = showFiles;
         this.Icon = MainMenuIcon.FolderClose;
      }

      /// <summary>
      /// Initializes a file item for the given special folder.
      /// </summary>
      /// <param name="parent">Parent tree item.</param>
      /// <param name="folder">Special folder to use.</param>
      /// <param name="showFiles">True if we want to show file when expanded.</param>
      public ExplorerItem (ExplorerItem parent, Environment.SpecialFolder folder, bool showFiles)
         : base (parent, new DirectoryInfo (Environment.GetFolderPath (folder)), true)
      {
         this.m_ShowFiles = showFiles;
         switch (folder)
         {
            case Environment.SpecialFolder.Desktop:
               this.IconBrush = Brushes.DodgerBlue;
               this.Icon = MainMenuIcon.Monitor;
               break;
            case Environment.SpecialFolder.MyDocuments:
               this.Icon = MainMenuIcon.Favoritefolder;
               break;
            case Environment.SpecialFolder.MyVideos:
               this.IconBrush = Brushes.DarkRed;
               this.Icon = MainMenuIcon.Myvideos;
               break;
            case Environment.SpecialFolder.MyMusic:
               this.IconBrush = Brushes.DarkOrange;
               this.Icon = MainMenuIcon.Mymusic;
               break;
            case Environment.SpecialFolder.MyPictures:
               this.IconBrush = Brushes.CornflowerBlue;
               this.Icon = MainMenuIcon.Mypictures;
               break;
            default:
               this.Icon = MainMenuIcon.FolderClose;
               break;
         }
      }

      /// <summary>
      /// Gets or Sets items to expand after dynamic load.
      /// </summary>
      public XElement Expanded { get; set; }

      /// <summary>
      /// Occurs when this item is expanded.
      /// </summary>
      protected override async void LoadChildren ()
      {
         var dirInfo = this.File as DirectoryInfo;
         if (!this.IsDirectory || dirInfo == null)
            return;
         this.Children.Clear ();
         var loader = new ModelTreeViewItem (this, false)
         { Text = "Loading...", Icon = MainMenuIcon.LoadingHourglass };
         this.Children.Add (loader);

         var fileItems = new Collection <ExplorerItem> ();

         var getDirs = Task.Run (() =>
         {
            var directories = dirInfo.GetDirectories ();
            foreach (var directoryInfo in directories)
            {
               var info = directoryInfo;
               fileItems.Add (new ExplorerItem (this, info, this.m_ShowFiles));
            }
         });
         await getDirs;
         if (this.m_ShowFiles)
         {
            var getFiles = Task.Run (() =>
            {
               var fileInfos = dirInfo.GetFiles ();
               foreach (var fileInfo in fileInfos)
               {
                  var info = fileInfo;
                  fileItems.Add (new ExplorerItem (this, info));
               }
            });
            await getFiles;
         }
         this.Children.Clear ();
         int count = 0;
         foreach (var explorerItem in fileItems)
         {
            count++;
            this.Children.Add (explorerItem);
            if (count == 100)
            {
               // This keeps the UI responsive when adding lots of items...
               await Task.Delay (500);
               count = 0;
            }
         }
         fileItems.Clear ();
         this.Loaded ();
      }

      /// <summary>
      /// Occurs when the children are loaded.
      /// </summary>
      private void Loaded ()
      {
         if (this.Expanded != null) {
            foreach (var expanded in this.Expanded.Elements ()) {
               var path = expanded.Attribute ("path").Value;
               var child = this.Children.Cast <ExplorerItem> ().FirstOrDefault (e => e.File.FullName == path);
               if (child != null) {
                  child.Expanded = expanded;
                  child.IsExpanded = true;
               }
            }
            this.Expanded = null;
         }
         if (!string.IsNullOrEmpty (this.m_SelectedChildPath))
         {
            this.SelectFile (this.m_SelectedChildPath);
         }
      }

      /// <summary>
      /// Selects the file with the given child path.
      /// </summary>
      /// <param name="childPath"></param>
      public void SelectFile (string childPath)
      {
         var item = childPath.Contains ("\\")
            ? childPath.Substring (0, childPath.IndexOf ('\\'))
            : childPath;

         var node = this.Children.FirstOrDefault (c => c.Text == item) as ExplorerItem;
         if (node == null)
         {
            if (string.IsNullOrEmpty (this.m_SelectedChildPath))
            {
               this.m_SelectedChildPath = childPath;
               // Refresh the list...
               this.IsExpanded = false;
               this.IsExpanded = true;
            }
            else
            {
               this.m_SelectedChildPath = string.Empty;
               this.IsSelected = true;
            }
         }
         else if (!node.IsDirectory)
         {
            node.IsSelected = true;
            this.m_SelectedChildPath = string.Empty;
         }
         else
         {
            node.IsSelected = true;
            this.m_SelectedChildPath = string.Empty;
            node.SelectFile (childPath.Substring (childPath.IndexOf ('\\') + 1));
         }
      }
   }
}
