using SeekDeepWithin.Controllers;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Models
{
   public class TermLinkViewModel
   {
      /// <summary>
      /// Initializes a new term link view model.
      /// </summary>
      public TermLinkViewModel () { }

      /// <summary>
      /// Initializes a new term link view model.
      /// </summary>
      /// <param name="link">Link to copy data from.</param>
      public TermLinkViewModel (TermLink link)
      {
         this.Id = link.Id;
         this.Name = link.Name;
         this.RefId = link.RefId;
         this.LinkType = (TermLinkType) link.LinkType;
      }

      /// <summary>
      /// Gets the id of the item.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the link Type.
      /// </summary>
      public TermLinkType LinkType { get; set; }

      /// <summary>
      /// Gets or Sets the referenced id.
      /// </summary>
      public int RefId { get; set; }

      /// <summary>
      /// Gets or Sets the name of the link.
      /// </summary>
      public string Name { get; set; }
   }
}