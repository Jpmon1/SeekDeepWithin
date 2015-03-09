using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class Chapter : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this chapter.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of this chapter.
      /// </summary>
      public string Name { get; set; }
   }
}
