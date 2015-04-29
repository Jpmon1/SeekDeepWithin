﻿using PagedList;

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
      /// Gets or Sets the terms for the index page.
      /// </summary>
      public IPagedList <GlossaryTermViewModel> Terms { get; set; }
   }
}