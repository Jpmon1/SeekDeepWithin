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

using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Media;
using Peter.Common.MainMenu;

namespace Peter.Common.Tree
{
   /// <summary>
   /// Represents a tree view item that houses a file.
   /// </summary>
   public class FileTreeViewItem : EditableTreeViewItem
   {
      /// <summary>
      /// Initializes a new editable tree view item.
      /// </summary>
      /// <param name="parent">Parent of item.</param>
      /// <param name="file">The file for this tree itme.</param>
      public FileTreeViewItem (ModelTreeViewItem parent, FileInfo file)
         : base (parent, false)
      {
         this.File = file;
         this.Text = file.Name;
         this.IsDirectory = false;
         this.Icon = MainMenuIconConverter.GetIconForFile (file);
         if ((file.Attributes & FileAttributes.Hidden) != 0) {
            this.IconBrush = MainMenuIconConverter.GetIconBrushForFile (file).Clone ();
            this.IconBrush.Opacity = 0.7;
            this.IconBrush.Freeze ();
         } else {
            this.IconBrush = MainMenuIconConverter.GetIconBrushForFile (file);
         }
      }

      /// <summary>
      /// Initializes a new editable tree view item.
      /// </summary>
      /// <param name="parent">Parent of item.</param>
      /// <param name="directory">The directory for this tree itme.</param>
      /// <param name="dynamicLoad">True to load dynamically</param>
      public FileTreeViewItem (ModelTreeViewItem parent, DirectoryInfo directory, bool dynamicLoad)
         : base (parent, dynamicLoad)
      {
         this.File = directory;
         this.Text = directory.Name;
         this.IsDirectory = true;
         this.Icon = MainMenuIcon.FolderClose;
         if ((directory.Attributes & FileAttributes.Hidden) != 0) {
            this.IconBrush = Brushes.BurlyWood.Clone();
            this.IconBrush.Opacity = 0.7;
            this.IconBrush.Freeze ();
         } else {
            this.IconBrush = Brushes.BurlyWood;
         }
      }

      /// <summary>
      /// Initializes a new editable tree view item.
      /// </summary>
      /// <param name="parent">Parent of item.</param>
      /// <param name="file">The file system object for this tree itme.</param>
      /// <param name="dynamicLoad">True to load dynamically</param>
      public FileTreeViewItem (ModelTreeViewItem parent, FileSystemInfo file, bool dynamicLoad)
         : base (parent, dynamicLoad)
      {
         this.File = file;
         this.Text = file.Name;
         this.IsDirectory = file is DirectoryInfo;
      }

      /// <summary>
      /// Initializes a new editable tree view item.
      /// </summary>
      /// <param name="parent">Parent of item.</param>
      /// <param name="dynamicLoad">True to load dynamically</param>
      public FileTreeViewItem (ModelTreeViewItem parent, bool dynamicLoad)
         : base (parent, dynamicLoad)
      {
      }

      /// <summary>
      /// Gets if this item is a directory or not.
      /// </summary>
      public bool IsDirectory { get; private set; }

      /// <summary>
      /// Gets or Sets the file associated with the tree item.
      /// </summary>
      public FileSystemInfo File { get; private set; }

      /// <summary>
      /// Checks whether an edit actions can be performed or not.
      /// </summary>
      /// <param name="obj">Edit action</param>
      /// <returns>True if action can be performed, otherwise false.</returns>
      protected override bool CanEdit (object obj)
      {
         if (this.File == null) return false;
         var editAction = (EditAction)obj;
         switch (editAction)
         {
            case EditAction.Copy:
               return true;
            case EditAction.Cut:
               return false;
            case EditAction.Paste:
               return this.IsDirectory;
            default:
               return false;
         }
      }

      /// <summary>
      /// Performs the edit action.
      /// </summary>
      /// <param name="obj">Edit action.</param>
      protected override void OnEdit (object obj)
      {
         var editAction = (EditAction)obj;
         switch (editAction)
         {
            case EditAction.Copy:
               Clipboard.SetFileDropList (new StringCollection { this.File.FullName });
               break;
            case EditAction.Cut:
               break;
            case EditAction.Paste:
               break;
         }
      }
   }
}
