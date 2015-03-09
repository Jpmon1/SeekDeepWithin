namespace SeekDeepWithin.Pocos
{
   public class GlossaryTermTag
   {
      /// <summary>
      /// Gets or Sets the id of the glossary term tag.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the tag.
      /// </summary>
      public virtual Tag Tag { get; set; }

      /// <summary>
      /// Gets or Sets the glossary term.
      /// </summary>
      public virtual GlossaryTerm Term { get; set; }
   }
}
