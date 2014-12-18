using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeekDeepWithin.Controllers;
using SeekDeepWithin.Domain;
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
         this.m_MockDb.Authors.Insert (new Author { Id = 0, Name = "Auth0" });
         this.m_MockDb.Authors.Insert (new Author { Id = 1, Name = "Auth1" });
         this.m_MockDb.Authors.Insert (new Author { Id = 2, Name = "Auth2" });
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

         var result = controller.Index () as ViewResult;
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
      public void TestCreatePost ()
      {
         var controller = new BookController (this.m_MockDb);
         var viewModel = new BookViewModel { Id = 1, Title = "A book", Summary = "Nothing" };
         var result = controller.Create (viewModel) as RedirectToRouteResult;
         Assert.AreEqual (2, this.m_MockDb.Books.All ().Count);
         Assert.IsNotNull (result);
      }

      /// <summary>
      /// Tests the create post page when given information is bad.
      /// </summary>
      [TestMethod]
      public void TestCreatePostFail ()
      {
         var controller = new BookController (this.m_MockDb);
         controller.ModelState.AddModelError ("test", "test");
         var viewModel = new BookViewModel ();
         var result = controller.Create (viewModel) as ViewResult;
         Assert.IsNotNull (result);
         var book = result.Model as BookViewModel;
         Assert.IsNotNull (book);
         Assert.AreEqual (viewModel, book);
      }
   }
}
