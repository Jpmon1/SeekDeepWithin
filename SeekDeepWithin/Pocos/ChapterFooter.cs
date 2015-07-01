using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class ChapterFooter : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the footer.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the footer.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets the list of links for this entry.
      /// </summary>
      public virtual ICollection<ChapterFooterLink> Links { get; set; }

      /// <summary>
      /// Gets the list of styles for this entry.
      /// </summary>
      public virtual ICollection<ChapterFooterStyle> Styles { get; set; }

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
