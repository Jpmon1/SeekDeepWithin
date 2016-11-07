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

using System.IO;
using Peter.Common.Tree;

namespace Peter.Common.Tests.Peter
{
   /// <summary>
   /// Mock for a peter project.
   /// </summary>
   public class MockPeterProject : PeterProjectItem
   {
      /// <summary>
      /// Initializes a new peter project.
      /// </summary>
      /// <param name="parent">Parent item</param>
      public MockPeterProject (ModelTreeViewItem parent) : base (parent) {}

      /// <summary>
      /// Initializes a new peter project.
      /// </summary>
      /// <param name="parent">Parent item</param>
      /// <param name="file">The file for this item.</param>
      public MockPeterProject (ModelTreeViewItem parent, FileInfo file) : base (parent, file) {}

      /// <summary>
      /// Initializes a new peter project.
      /// </summary>
      /// <param name="parent">Parent item</param>
      /// <param name="directory">The directory for this item.</param>
      public MockPeterProject (ModelTreeViewItem parent, DirectoryInfo directory) : base (parent, directory) {}

      /// <summary>
      /// Sets the project name.
      /// </summary>
      /// <param name="name">The name of the project</param>
      public void SetName (string name)
      {
         this.Text = name;
      }
   }
}
