using System.Collections.Generic;
using Newtonsoft.Json;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a list of imports.
   /// </summary>
   public class ImportList
   {
      private readonly List <ImportGroup> m_Groups = new List <ImportGroup> ();

      [JsonProperty ("g")]
      public List<ImportGroup> Groups { get { return this.m_Groups; } }
   }
}