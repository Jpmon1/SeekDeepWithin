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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Peter.Common.Controls
{
   /// <summary>
   /// A control used to explore files and folders.
   /// </summary>
   public class Explorer : TreeViewEx
   {
      /// <summary>
      /// Initializes a new explorer.
      /// </summary>
      public Explorer ()
      {
         this.Loaded += this.OnLoad;
         this.FileSystem = new ObservableCollection <ExplorerItem> ();
         this.SetBinding (ItemsSourceProperty, new Binding ("FileSystem") { Source = this });
      }

      /*/// <summary>
      /// Gets the file system list.
      /// </summary>
      //public ObservableCollection <ExplorerItem> FileSystem { get; private set; }*/

      /// <summary>
      /// Dependency property for the file system.
      /// </summary>
      public static readonly DependencyProperty FileSystemProperty = DependencyProperty.Register (
         "FileSystem", typeof(IEnumerable<ExplorerItem>), typeof(Explorer),
         new PropertyMetadata (default(IEnumerable<ExplorerItem>)));

      /// <summary>
      /// Gets or Sets the file system.
      /// </summary>
      public IEnumerable<ExplorerItem> FileSystem
      {
         get { return (IEnumerable<ExplorerItem>) GetValue (FileSystemProperty); }
         set { SetValue (FileSystemProperty, value); }
      }

      /// <summary>
      /// Dependency property for the root directory.
      /// </summary>
      public static readonly DependencyProperty RootDirectoryProperty =
         DependencyProperty.Register ("RootDirectory", typeof (string), typeof (Explorer),
         new PropertyMetadata (default(string)));

      /// <summary>
      /// Gets or Sets the root directory.
      /// </summary>
      public string RootDirectory
      {
         get { return (string) GetValue (RootDirectoryProperty); }
         set { SetValue (RootDirectoryProperty, value); }
      }

      /// <summary>
      /// Dependency property for showing files.
      /// </summary>
      public static readonly DependencyProperty ShowFilesProperty =
         DependencyProperty.Register ("ShowFiles", typeof (bool), typeof (Explorer),
         new PropertyMetadata (true));

      /// <summary>
      /// Gets or Sets if the explorer should show files or not.
      /// </summary>
      public bool ShowFiles
      {
         get { return (bool) GetValue (ShowFilesProperty); }
         set { SetValue (ShowFilesProperty, value); }
      }

      /// <summary>
      /// Dependency property for the selected path.
      /// </summary>
      public static readonly DependencyProperty SelectedPathProperty =
         DependencyProperty.Register ("SelectedPath", typeof (string), typeof (Explorer),
         new PropertyMetadata (default(string), OnSelectedPathChanged));

      /// <summary>
      /// Gets or Sets the full path to the selected file/directory.
      /// </summary>
      public string SelectedPath
      {
         get { return (string) GetValue (SelectedPathProperty); }
         set { SetValue (SelectedPathProperty, value); }
      }

      /// <summary>
      /// Occurs when the selected path changes.
      /// </summary>
      /// <param name="d">An explorer object.</param>
      /// <param name="e">DependencyPropertyChangedEventArgs</param>
      private static void OnSelectedPathChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var explorer = d as Explorer;
         if (explorer != null)
         {
            var item = explorer.SelectedTreeItem as ExplorerItem;
            var file = e.NewValue.ToString ();
            if (item != null && item.File.FullName == file)
            {
               // TODO: this.SelectedNode.EnsureVisible();
               return;
            }
            if (!string.IsNullOrEmpty (file))
            {
               file = file.Replace ('/', '\\');
               var drive = file.Substring (0, file.IndexOf ('\\') + 1);
               foreach (var f in explorer.FileSystem)
               {
                  if (f.Text == drive)
                     f.SelectFile (file.Substring (file.IndexOf ('\\') + 1));
               }
            }
         }
      }

      /// <summary>
      /// Occurs when the selected tree item changes;
      /// </summary>
      protected override void OnSelectedTreeItemChanged ()
      {
         var item = this.SelectedTreeItem as ExplorerItem;
         if (item != null)
         {
            this.SelectedPath = item.File.FullName;
         }
      }

      /// <summary>
      /// Occurs when the control has been loaded.
      /// </summary>
      /// <param name="sender">This tree view.</param>
      /// <param name="e">RoutedEventArgs</param>
      private void OnLoad (object sender, RoutedEventArgs e)
      {
         this.Loaded -= this.OnLoad;
         var fileSystem = new ObservableCollection <ExplorerItem> ();
         if (string.IsNullOrEmpty (this.RootDirectory))
         {
            fileSystem.Add (new ExplorerItem (null, Environment.SpecialFolder.MyDocuments, this.ShowFiles));
            fileSystem.Add (new ExplorerItem (null, Environment.SpecialFolder.Desktop, this.ShowFiles));

            foreach (var drive in DriveInfo.GetDrives ().Where (drive => drive.DriveType == DriveType.Fixed))
            {
               fileSystem.Add (new ExplorerItem (drive, this.ShowFiles));
            }
            fileSystem.First ().IsSelected = true;
         }
         else
         {
            var root = new DirectoryInfo (this.RootDirectory);
            if (root.Exists)
            {
               var directories = root.GetDirectories ();
               foreach (var directoryInfo in directories)
                  fileSystem.Add (new ExplorerItem(null, directoryInfo, this.ShowFiles));

               if (this.ShowFiles)
               {
                  var files = root.GetFiles ();
                  foreach (var fileInfo in files)
                     fileSystem.Add (new ExplorerItem (null, fileInfo));
               }
               if (fileSystem.Count > 0)
                  fileSystem.First ().IsSelected = true;
            }
         }
         this.FileSystem = fileSystem;
      }

      /// <summary>
      /// Executes the selected item.
      /// </summary>
      protected override void Execute ()
      {
         if (this.SelectedItem != null)
         {
            var selectedItem = this.SelectedTreeItem as ExplorerItem;
            if (this.ExecuteCommand != null && selectedItem != null)
            {
               if (this.ExecuteCommand.CanExecute (selectedItem.File))
                  this.ExecuteCommand.Execute (selectedItem.File);
            }
         }
      }
   }
}
