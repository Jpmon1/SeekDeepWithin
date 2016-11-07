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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Peter.Common.Tests.Peter
{
   /// <summary>
   /// Test cases for the text event args.
   /// </summary>
   [TestClass]
   public class TextEventArgsTests
   {
      /// <summary>
      /// Tests the text event args
      /// </summary>
      [TestMethod]
      public void TestTextEventArgs ()
      {
         var args = new TextEventArgs ("test");
         Assert.AreEqual ("test", args.Text);
      }
   }
}
