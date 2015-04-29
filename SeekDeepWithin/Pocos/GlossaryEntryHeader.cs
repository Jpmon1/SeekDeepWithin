using System.Collections.Generic;
using System.Linq;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a header for a glossary entry.
   /// </summary>
   public class GlossaryEntryHeader : IDbTable, IHeader
   {
      /// <summary>
      /// Gets or Sets the id of the header.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the header.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the glossary entry the header belongs to.
      /// </summary>
      public virtual GlossaryEntry Entry { get; set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public virtual ICollection<GlossaryHeaderStyle> Styles { get; set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public IEnumerable<IStyle> StyleList { get { return this.Styles; } }
   }
}