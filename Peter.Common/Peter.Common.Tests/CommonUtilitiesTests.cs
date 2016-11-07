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

namespace Peter.Common.Tests
{
   /// <summary>
   /// Test cases for the common utilities class.
   /// </summary>
   [TestClass]
   public class CommonUtilitiesTests
   {
      /// <summary>
      /// Tests the IsInDesignMode property.
      /// </summary>
      [TestMethod]
      public void TestIsInDesignMode ()
      {
         Assert.IsFalse (CommonUtilities.IsInDesignMode);
      }

      /// <summary>
      /// Tests the random string.
      /// </summary>
      [TestMethod]
      public void TestRandomString ()
      {
         var str = CommonUtilities.RandomString (4);
         Assert.AreEqual (4, str.Length);
         var str2 = CommonUtilities.RandomString (4);
         Assert.AreNotEqual (str, str2);
      }
   }
}
