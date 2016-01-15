using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Tests.Mocks;

namespace SeekDeepWithin.Tests.Controllers
{
   [TestClass]
   public class LightControllerTest
   {
      private int m_LightId;
      private int m_TruthId;
      private int m_LoveId;
      private MockDatabase m_MockDb;

      /// <summary>
      /// Intializes a new mock database.
      /// </summary>
      [TestInitialize]
      public void TestInitialize ()
      {
         this.m_MockDb = new MockDatabase ();
         var bg = new Light { Id = this.m_LightId++, Text = "Bhagavad Gita" };
         var bg1 = new Light { Id = this.m_LightId++, Text = "Bhagavad Gita with Commentaries" };
         var bg2 = new Light { Id = this.m_LightId++, Text = "Bhagavad Gita by so and so" };
         var bgCh1 = new Light { Id = this.m_LightId++, Text = "Conversation 1" };
         var bible = new Light { Id = this.m_LoveId++, Text = "Bible" };
         var kjv = new Light {Id = this.m_LightId++, Text = "King James Bible" };
         var drb = new Light {Id = this.m_LightId++, Text = "Douay-Rheims Bible" };
         var gen = new Light {Id = this.m_LightId++, Text = "Genesis" };
         var ch1 = new Light { Id = this.m_LightId++, Text = "Chapter 1" };
         var date2008 = new Light { Id = this.m_LightId++, Text = "2008" };
         var sourceName = new Light { Id = this.m_LightId++, Text = "Swami Center" };
         var sourceUrl = new Light { Id = this.m_LightId++, Text = "http://bhagavadgita.swami-center.org/" };
         var intro = new Light { Id = this.m_LightId++, Text = "Introduction" };
         var con1 = new Light { Id = this.m_LightId++, Text = "Conversation 1" };
         var introV1 = new Light { Id = this.m_LightId++, Text = "The Bhagavad Gita — or, in translation from Sanskrit, the Song of God — is the most important part of the Indian epic poem Mahabharata. The latter describes events that took place about 5000 years ago." };
         var wiki = new Light { Id = this.m_LightId++, Text = "Wikipedia" };
      }

      [TestMethod]
      public void TestCreateLight ()
      {
         /*var lights = this.m_MockDb.Lights.All ();
         for (int index = 0; index < lights.Count; index++) {
            this.m_MockDb.Lights.Delete (lights [index]);
         }
         var controller = new LightController (this.m_MockDb);
         var result = controller.Create ("Bhagavad Gita", true, true) as JsonResult;
         Assert.IsNotNull (result);
         Assert.AreEqual (1, this.m_MockDb.Light.All ().Count);*/
      }
   }
}
