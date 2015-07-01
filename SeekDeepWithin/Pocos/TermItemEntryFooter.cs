using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a footer for a glossary entry.
   /// </summary>
   public class TermItemEntryFooter : IDbTable, IFooter
   {
      /// <summary>
      /// Gets or Sets the id of the footer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the index of the footer.
      /// </summary>
      public int Index { get; set; }

      /// <summary>
      /// Gets or Sets the footer.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets or Sets the glossary entry the footer belongs to.
      /// </summary>
      public virtual TermItemEntry Entry { get; set; }

      /// <summary>
      /// Gets the list of links for this entry.
      /// </summary>
      public virtual ICollection<TermItemEntryFooterLink> Links { get; set; }

      /// <summary>
      /// Gets the list of styles for this entry.
      /// </summary>
      public virtual ICollection<TermItemEntryFooterStyle> Styles { get; set; }

      /// <summary>
      /// Gets the list of links.
      /// </summary>
      public IEnumerable<ILink> LinkList { get { return this.Links; } }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public IEnumerable<IStyle> StyleList { get { return this.Styles; } }
   }
}