using System.Collections.ObjectModel;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a level on SDW.
   /// </summary>
   public class LoveModel
   {
      /// <summary>
      /// Initializes a new level.
      /// </summary>
      public LoveModel ()
      {
         this.ToAdd = new Collection<SdwItem> ();
         this.ToRemove = new Collection<string> ();
      }

      /// <summary>
      /// Gets the items to add.
      /// </summary>
      public Collection<SdwItem> ToAdd { get; private set; }

      /// <summary>
      /// Gets the items ro remove.
      /// </summary>
      public Collection<string> ToRemove { get; private set; }
   }
}