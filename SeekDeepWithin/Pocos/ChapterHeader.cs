using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class ChapterHeader : IDbTable, IHeader
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
      /// Gets or Sets the chapter the header belongs to.
      /// </summary>
      public virtual SubBookChapter Chapter { get; set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public virtual ICollection<ChapterHeaderStyle> Styles { get; set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public IEnumerable<IStyle> StyleList { get { return this.Styles; } }
   }
}
