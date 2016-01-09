namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// A regular expression used for formatting.
   /// </summary>
   public class FormatRegex : IDbTable
   {
      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the regular expression.
      /// </summary>
      public string Regex { get; set; }
   }
}