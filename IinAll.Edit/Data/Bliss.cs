namespace IinAll.Edit.Data
{
   /// <summary>
   /// Model for bliss.
   /// </summary>
   public class Bliss
   {
      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the love of the bliss.
      /// </summary>
      public Love Love { get; set; }

      /// <summary>
      /// Gets or Sets the truth of the bliss.
      /// </summary>
      public Truth Truth { get; set; }

      /// <summary>
      /// Get or Sets the light header of the bliss.
      /// </summary>
      public Light Header { get; set; }

      /// <summary>
      /// Gets or Sets the order.
      /// </summary>
      public int Order { get; set; }

      /// <summary>
      /// Gets or Sets the number.
      /// </summary>
      public int? Number { get; set; }
   }
}
