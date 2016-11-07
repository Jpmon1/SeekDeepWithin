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
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Peter.Common.Controls
{
   /// <summary>
   /// Represents a rich text editor.
   /// </summary>
   public class RichTextEditor : RichTextBox
   {
      /// <summary>
      /// Initializes a new Rich text editor.
      /// </summary>
      public RichTextEditor ()
      {
         this.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
         this.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
         this.AddHandler (Hyperlink.RequestNavigateEvent, new RoutedEventHandler (this.HyperlinkClicked));
      }

      /// <summary>
      /// Dependency property for opening hyper links.
      /// </summary>
      public static readonly DependencyProperty OpenHyperLinkCommandProperty = DependencyProperty.Register (
         "OpenHyperLinkCommand", typeof (ICommand), typeof (RichTextEditor), new PropertyMetadata (default(ICommand)));

      /// <summary>
      /// Gets or Sets the command to open a hyper link.
      /// </summary>
      public ICommand OpenHyperLinkCommand
      {
         get { return (ICommand) GetValue (OpenHyperLinkCommandProperty); }
         set { SetValue (OpenHyperLinkCommandProperty, value); }
      }

      /// <summary>
      /// Dependency property for file.
      /// </summary>
      public static DependencyProperty FileProperty = DependencyProperty.Register ("File", typeof (String), typeof (RichTextEditor),
          new PropertyMetadata (OnFileChanged));

      /// <summary>
      /// Gets or Sets the file for the rich text box.
      /// </summary>
      public String File
      {
         get { return (String)GetValue (FileProperty); }
         set { SetValue (FileProperty, value); }
      }

      /// <summary>
      /// Occurs when the file changes.
      /// </summary>
      /// <param name="d">RichTextEditor</param>
      /// <param name="e">DependencyPropertyChangedEventArgsa</param>
      private static void OnFileChanged (DependencyObject d, DependencyPropertyChangedEventArgs e)
      {
         var rtf = d as RichTextEditor;
         if (rtf == null)
            return;
         OpenFile (rtf.File, rtf.Document);
      }

      /// <summary>
      /// Opens the given file.
      /// </summary>
      /// <param name="fileName">The name of the file to open.</param>
      /// <param name="flowDoc">The RTF flow document.</param>
      private static void OpenFile (string fileName, FlowDocument flowDoc)
      {
         if (System.IO.File.Exists (fileName))
         {
            var range = new TextRange (flowDoc.ContentStart, flowDoc.ContentEnd);
            using (var fStream = new FileStream (fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
               range.Load (fStream, DataFormats.Rtf);
         }
      }

      /// <summary>
      /// Occurs when a hyper link is clicked.
      /// </summary>
      /// <param name="sender">RichTextEditor</param>
      /// <param name="args">RoutedEventArgs.</param>
      private void HyperlinkClicked (object sender, RoutedEventArgs args)
      {
         var link = args.Source as Hyperlink;
         if (link != null && this.OpenHyperLinkCommand != null &&
             this.OpenHyperLinkCommand.CanExecute (link.NavigateUri))
         {
            args.Handled = true;
            this.OpenHyperLinkCommand.Execute (link.NavigateUri);
         }
      }
   }
}
