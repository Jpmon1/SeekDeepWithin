using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeekDeepWithin.Controllers;

namespace SeekDeepWithin.Tests.Controllers
{
   [TestClass]
   public class ConvertControllerTest
   {
      [TestMethod]
      public void TestRegexToPassages ()
      {
         var controller = new ConvertController ();
         controller.ModelState.AddModelError ("test", "test");
         var result = controller.RegexToPassages ("1:1 Text 1:2 Test text 1:3 blah blahs blhdidlfjk 1:4 ..fjkej.sejf;sdjf/.skdjf",
            @"(?<chapter>\d+):(?<number>\d+)(?<text>\D+)", 1, 1) as JsonResult;
         Assert.IsNotNull (result);
      }
   }
}
