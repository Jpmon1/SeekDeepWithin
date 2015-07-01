using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Models;
using SeekDeepWithin.Tests.Mocks;

namespace SeekDeepWithin.Tests.Controllers
{
   /// <summary>
   /// Test cases for the book controller.
   /// </summary>
   [TestClass]
   public class BookControllerTest
   {
      private MockDatabase m_MockDb;

      /// <summary>
      /// Intializes a new mock database.
      /// </summary>
      [TestInitialize]
      public void TestInitialize ()
      {
         this.m_MockDb = new MockDatabase ();
         this.m_MockDb.Books.Insert (new Book
         {
            Id = 0,
            Title = "Test Book",
            Summary = "No info"
         });
      }

      /// <summary>
      /// Tests the book index page.
      /// </summary>
      [TestMethod]
      public void TestIndex ()
      {
         var controller = new BookController (this.m_MockDb);

         var result = controller.Index (null) as ViewResult;
         Assert.IsNotNull (result);

         var books = result.Model as List <BookViewModel>;
         Assert.IsNotNull (books);
         Assert.AreEqual (1, books.Count);
      }

      /// <summary>
      /// Tests the create get page.
      /// </summary>
      [TestMethod]
      public void TestCreateGet ()
      {
         var controller = new BookController (this.m_MockDb);

         var result = controller.Create() as ViewResult;
         Assert.IsNotNull (result);

         var book = result.Model as BookViewModel;
         Assert.IsNotNull (book);
      }

      /// <summary>
      /// Tests the create post page.
      /// </summary>
      [TestMethod]
      public void TestCreate ()
      {
         var controller = new BookController (this.m_MockDb);
         var result = controller.Create ("Book Title", "", "Summary", 0) as JsonResult;
         Assert.AreEqual (2, this.m_MockDb.Books.All ().Count);
         Assert.IsNotNull (result);
      }

      /// <summary>
      /// Tests the create post page when given information is bad.
      /// </summary>
      [TestMethod]
      public void TestCreateNoTitle ()
      {
         var controller = new BookController (this.m_MockDb);
         var result = controller.Create ("", "", "", -1) as JsonResult;
         Assert.IsNotNull (result);
      }
   }
}
