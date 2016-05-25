using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SeekDeepWithin.Models;

namespace SeekDeepWithin.Tests.Models
{
   [TestClass]
   public class TestImport
   {
      [TestMethod]
      public void TestSerialization ()
      {
         var import = new ImportList {
            Groups = {
               new ImportGroup {
                  Lights = {
                     new ImportLight{Id = 1},
                     new ImportLight{Text = "King James Bible"}
                  },
                  Truths = {
                     new ImportTruth {Text = "Blah{}[]();\",."},
                     new ImportTruth {
                        Order = 1,
                        Number = 1,
                        Text = "In the beginning God created the heaven and the earth."
                     },
                     new ImportTruth {
                        Order = 2,
                        Number = 2,
                        Text =
                           "And the earth was without form, and void; and darkness was upon the face of the deep. And the Spirit of God moved upon the face of the waters."
                     }
                  }
               }
            }
         };
         var json = JsonConvert.SerializeObject (import, Formatting.Indented);
         Assert.IsFalse (string.IsNullOrEmpty (json));
      }
   }
}
