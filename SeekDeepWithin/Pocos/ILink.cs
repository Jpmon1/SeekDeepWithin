namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Common interface for links.
   /// </summary>
   public interface ILink
   {
      /// <summary>
      /// Gets or Sets the id of the entry link.
      /// </summary>
      int Id { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the link.
      /// </summary>
      int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the link.
      /// </summary>
      int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets if the link should open in a new window.
      /// </summary>
      bool OpenInNewWindow { get; set; }

      /// <summary>
      /// Gets or Sets the link.
      /// </summary>
      Link Link { get; set; }
   }
}