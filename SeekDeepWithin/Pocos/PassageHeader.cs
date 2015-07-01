using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   public class PassageHeader : IDbTable, IHeader
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
      public virtual ICollection<PassageHeaderStyle> Styles { get; set; }

      /// <summary>
      /// Gets the list of styles.
      /// </summary>
      public IEnumerable<IStyle> StyleList { get { return this.Styles; } }
   }
}
