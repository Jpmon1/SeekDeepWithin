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

using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Peter.Common.Tests.Peter
{
   /// <summary>
   /// Test cases for a peter project.
   /// </summary>
   [TestClass]
   public class PeterProjectTests
   {
      private string m_PropertyChange;

      /// <summary>
      /// Tests the constructor.
      /// </summary>
      [TestMethod]
      public void TestConstructor ()
      {
         var project = new MockPeterProject (null);
         Assert.IsNotNull (project.Children);
      }

      /// <summary>
      /// Tests the properties.
      /// </summary>
      [TestMethod]
      public void TestProperties ()
      {
         var project = new MockPeterProject (null);
         project.PropertyChanged += this.OnProjectPropertyChanged;

         project.SetName ("TestName");
         Assert.AreEqual ("TestName", project.Text);
         Assert.AreEqual ("ProjectName", this.m_PropertyChange);

         project.IsCurrentProject = true;
         Assert.IsTrue (project.IsCurrentProject);
         Assert.AreEqual ("IsCurrentProject", this.m_PropertyChange);
         this.m_PropertyChange = string.Empty;
      }

      private void OnProjectPropertyChanged (object sender, PropertyChangedEventArgs e)
      {
         this.m_PropertyChange = e.PropertyName;
      }
   }
}
