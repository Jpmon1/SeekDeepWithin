using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Tests.ViewModels
{
   [TestClass]
   public class BookViewModelTest
   {
      [TestMethod]
      public void TestValiditiyNoTitle ()
      {
         var bookViewModel = new BookViewModel ();
         var context = new ValidationContext (bookViewModel, null, null);
         var results = new List <ValidationResult> ();
         var isValid = Validator.TryValidateObject (bookViewModel, context, results, true);
         Assert.IsFalse (isValid);
         Assert.IsTrue (results.Any (r => r.ErrorMessage.Equals ("The Title field is required.")));
      }
   }
}
