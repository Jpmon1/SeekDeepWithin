using System.Collections.Generic;
using System.Linq;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   public class LoveModel
   {
      private readonly List <Light> m_Lights;

      /// <summary>
      /// Initializes a new love model.
      /// </summary>
      public LoveModel (IEnumerable <Light> lights)
      {
         this.m_Lights = new List <Light> (lights);
         if (m_Lights.Count <= 0) return;
         /*if (Lights.First ().IsBook) {
            this.CreateContents ();
         }*/
      }

      /// <summary>
      /// Gets the list of lights.
      /// </summary>
      public List<Light> Lights { get { return this.m_Lights; } }

      /// <summary>
      /// Creates the table of contents.
      /// </summary>
      private void CreateContents ()
      {
      }
   }
}