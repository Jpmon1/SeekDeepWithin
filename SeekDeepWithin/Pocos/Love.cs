using System;
using System.Collections.Generic;

namespace SeekDeepWithin.Pocos
{
   /// <summary>
   /// Represents a link of love.
   /// </summary>
   public sealed class Love : IDbTable
   {
      public Love ()
      {
         this.Lights = new HashSet <Light> ();
         this.Truths = new HashSet <Truth> ();
      }

      /// <summary>
      /// Gets or Sets the id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the parent id string.
      /// </summary>
      public string ParentId { get; set; }

      /// <summary>
      /// Gets or Sets the last time this was modified.
      /// </summary>
      public DateTime Modified { get; set; }

      /// <summary>
      /// Gets or Sets the parent light.
      /// </summary>
      public ICollection<Light> Lights { get; set; }

      /// <summary>
      /// Gets or Sets the truths.
      /// </summary>
      public ICollection<Truth> Truths { get; set; }
   }
}