using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeekDeepWithin.Controllers;

namespace SeekDeepWithin.Tests.Controllers
{
   [TestClass]
   public class HomeControllerTest
   {
      [TestMethod]
      public void Index ()
      {
         // Arrange
         var controller = new HomeController ();
         // Act
         var result = controller.Index () as ViewResult;
         // Assert
         Assert.IsNotNull (result);
         Assert.AreEqual ("Study Spiritual Texts from all over the World.", result.ViewBag.Message);
      }

      [TestMethod]
      public void About ()
      {
         // Arrange
         var controller = new HomeController ();
         // Act
         var result = controller.About () as ViewResult;
         // Assert
         Assert.IsNotNull (result);
      }
   }
}
