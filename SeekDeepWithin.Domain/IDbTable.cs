namespace SeekDeepWithin.Domain
{
   /// <summary>
   /// Common properties for a database table.
   /// </summary>
   public interface IDbTable
   {
      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      int Id { get; }
   }
}