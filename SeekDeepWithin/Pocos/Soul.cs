namespace SeekDeepWithin.Pocos
{
   public class Soul : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id of the soul.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the data of the soul.
      /// </summary>
      public string Data { get; set; }
   }
}