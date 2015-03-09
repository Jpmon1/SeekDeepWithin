namespace SeekDeepWithin.Pocos
{
   public class Abbreviation : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of this item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the the abbreviation.
      /// </summary>
      public string Text { get; set; }

      /// <summary>
      /// Gets the linked sub book of this abbreviation.
      /// </summary>
      public virtual SubBook SubBook { get; set; }
   }
}