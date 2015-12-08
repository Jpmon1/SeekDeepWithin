using System.Collections.Generic;
using System.Linq;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   public class LightModel
   {
      /// <summary>
      /// Initializes a new light model.
      /// </summary>
      /// <param name="light">The list of light.</param>
      public LightModel (IList <Light> light)
      {
         if (!light.Any ()) return;
         this.Light = light.First ();
         light.RemoveAt (0);
         if (light.Count > 0)
            this.Child = new LightModel (light);
      }

      /// <summary>
      /// Gets the light.
      /// </summary>
      public Light Light { get; private set; }

      /// <summary>
      /// Gets the love.
      /// </summary>
      public LightModel Child { get; private set; }
   }
}