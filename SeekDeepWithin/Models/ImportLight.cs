using Newtonsoft.Json;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a light for importing.
   /// </summary>
   public class ImportLight
   {
      /// <summary>
      /// Gets or Sets the id of the light, if we have it.
      /// </summary>
      [JsonProperty ("i")]
      public int? Id { get; set; }

      /// <summary>
      /// Gets if we should serialize the id.
      /// </summary>
      public bool ShouldSerializeId () { return this.Id.HasValue; }

      /// <summary>
      /// Gets or Sets the text for the light, if we have it.
      /// </summary>
      [JsonProperty ("t")]
      public string Text { get; set; }

      /// <summary>
      /// Gets if we should serialize the text.
      /// </summary>
      public bool ShouldSerializeText () { return !string.IsNullOrEmpty (this.Text); }
   }
}