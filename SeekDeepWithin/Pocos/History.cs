namespace SeekDeepWithin.Pocos
{
   public class History : IDbTable
   {
      /// <summary>
      /// Gets or Sets the ID of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the json representing the history.
      /// </summary>
      public string Json { get; set; }

      /// <summary>
      /// Gets or Sets the level items to load for the history item.
      /// </summary>
      public string LevelItems { get; set; }
   }
}