using System;
using System.Collections.Generic;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a paged view model.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public class PagedViewModel <T> : List <T>
   {
      /// <summary>
      /// Gets the total number of item.
      /// </summary>
      public int TotalHits { get; set; }

      /// <summary>
      /// Gets the current page number.
      /// </summary>
      public int PageNumber { get; set; }

      /// <summary>
      /// Gets the number of items on a page.
      /// </summary>
      public int ItemsOnPage { get; set; }

      /// <summary>
      /// Gets the starting item number we are displaying.
      /// </summary>
      public int Start
      {
         get { return (this.PageNumber - 1) * this.ItemsOnPage + 1; }
      }

      /// <summary>
      /// Gets the ending item number we are displaying.
      /// </summary>
      public int End
      {
         get
         {
            var end = this.PageNumber * this.ItemsOnPage;
            return (this.TotalHits < end) ? this.TotalHits : end;
         }
      }

      /// <summary>
      /// Get s the total number of pages.
      /// </summary>
      public int TotalPages { get { return Convert.ToInt32 (this.TotalHits / this.ItemsOnPage) + 1; } }
   }
}