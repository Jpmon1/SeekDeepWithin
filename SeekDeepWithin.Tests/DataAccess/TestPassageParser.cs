using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Tests.Mocks;

namespace SeekDeepWithin.Tests.DataAccess
{
   [TestClass]
   public class TestPassageParser
   {
      /// <summary>
      /// Tests the passage parser for bible parsing.
      /// </summary>
      [TestMethod]
      public void TestBible ()
      {
         //var parser = new PassageParser (new MockDatabase());
         //parser.Parse("John3:16");
         //Assert.AreEqual ("16", parser.PassageList["Bible"]["King James Bible"]["John"]["3"].First());
         //parser.PassageList.Clear();
         //parser.Parse ("gen1:1");
         //Assert.AreEqual ("1", parser.PassageList["Bible"]["King James Bible"]["Genesis"]["1"].First ());
      }
   }
}
