using SeekDeepWithin.Controllers;

namespace SeekDeepWithin.Pocos
{
   public class TermLink : IDbTable
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the link Type.
      /// </summary>
      public int LinkType { get; set; }

      /// <summary>
      /// Gets or Sets the referenced id.
      /// </summary>
      public int RefId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the link.
      /// </summary>
      public string Name { get; set; }
   }
}