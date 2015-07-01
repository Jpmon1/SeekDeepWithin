using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a header for a glossary entry.
   /// </summary>
   public class TermItemEntryHeader : IDbTable, IHeader
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
      /// Gets the list of styles.
      /// </summary>
      public virtual ICollection<TermItemEntryHeaderStyle> Styles { get; set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public IEnumerable<IStyle> StyleList { get { return this.Styles; } }
   }
}