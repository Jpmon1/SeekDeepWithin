using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a source for a glossary entry.
   /// </summary>
   public class GlossaryItemSource : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the entry source.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the name of the source.
      /// </summary>
      public string Name { get; set; }

      /// <summary>
      /// Gets or Sets a url link for the source.
      /// </summary>
      public string Url { get; set; }

      /// <summary>
      /// Gets or Sets any data for the source.
      /// </summary>
      public string Data { get; set; }

      /// <summary>
      /// Gets or Sets the version.
      /// </summary>
      public virtual ICollection<GlossaryItem> GlossaryItems { get; set; }
   }
}
