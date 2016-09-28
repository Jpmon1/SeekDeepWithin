namespace IinAll.Edit.Data
{
   /// <summary>
   /// Represents a truth.
   /// </summary>
   public class Truth
   {
      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the corresponding light.
      /// </summary>
      public Light Light { get; set; }

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
