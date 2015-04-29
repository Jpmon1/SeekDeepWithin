using System;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Enumeration for item types in SDW.
   /// </summary>
   [Flags]
   public enum ItemType
   {
      Passage,
      Chapter,
      Header,
      Footer,
      Entry,
      Item,
      Term,
      SeeAlso,
      Redirect
   }
}