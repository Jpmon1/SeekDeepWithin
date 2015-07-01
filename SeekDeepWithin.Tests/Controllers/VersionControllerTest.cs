using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Tests.Mocks;

namespace SeekDeepWithin.Tests.Controllers
{
   /// <summary>
   /// Test cases for the version controller.
   /// </summary>
   [TestClass]
   public class VersionControllerTest
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
            Title = "Bible",
            Summary = "No info"
         });
         this.m_MockDb.Versions.Insert (new Version
         {
            Id = 0,
            Title = "King James",
            Book = this.m_MockDb.Books.Get (0),
            PublishDate = "1611"
         });
      }
   }
}
