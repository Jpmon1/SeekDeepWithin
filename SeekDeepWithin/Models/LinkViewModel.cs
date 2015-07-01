using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a passage link.
   /// </summary>
   public class LinkViewModel
   {
      /// <summary>
      /// Initializes a new link view model.
      /// </summary>
      public LinkViewModel () { }

      /// <summary>
      /// Initializes a new link view model from the given model.
      /// </summary>
      /// <param name="link">The model to copy data from.</param>
      public LinkViewModel (ILink link)
      {
         this.Id = link.Id;
         this.LinkId = link.Link.Id;
         this.Url = link.Link.Url;
         this.StartIndex = link.StartIndex;
         this.EndIndex = link.EndIndex;
         this.OpenInNewWindow = link.OpenInNewWindow;
      }

      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the id of the link
      /// </summary>
      public int LinkId { get; set; }

      /// <summary>
      /// Gets or Sets the start index of the link.
      /// </summary>
      public int StartIndex { get; set; }

      /// <summary>
      /// Gets or Sets the end index of the link.
      /// </summary>
      public int EndIndex { get; set; }

      /// <summary>
      /// Gets or Sets the url of the link.
      /// </summary>
      public string Url { get; set; }

      /// <summary>
      /// Gets or Sets if this link should open in a new window or not.
      /// </summary>
      public bool OpenInNewWindow { get; set; }
   }
}