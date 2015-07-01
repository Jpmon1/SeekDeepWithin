namespace SeekDeepWithin.Models
{
   /// <summary>
   /// View Model for the glossary index page.
   /// </summary>
   public class GlossaryIndexViewModel
   {
      /// <summary>
      /// Gets or Sets the source name we are displaying, if any.
      /// </summary>
      public string SourceName { get; set; }

      /// <summary>
      /// Gets or Sets the source Id, if any.
      /// </summary>
      public int? SourceId { get; set; }

      /// <summary>
      /// Gets or Sets the terms for the index page.
      /// </summary>
      public PagedViewModel <TermViewModel> Terms { get; set; }
   }
}