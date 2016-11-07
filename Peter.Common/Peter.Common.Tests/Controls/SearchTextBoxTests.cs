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
using Peter.Common.Controls;

namespace Peter.Common.Tests.Controls
{
   /// <summary>
   /// Test cases for the search text box.
   /// </summary>
   [TestClass]
   public class SearchTextBoxTests
   {
      /// <summary>
      /// Tests the constructor.
      /// </summary>
      [TestMethod]
      public void TestConstructor ()
      {
         var search = new CommandTextBox ();
         Assert.AreEqual ("Find...", search.PlaceHolderText);
      }
   }
}
