using Newtonsoft.Json;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents a truth for importing.
   /// </summary>
   public class ImportTruth
   {
      /// <summary>
      /// Gets or Sets the number, if we have it.
      /// </summary>
      [JsonProperty("n")]
      public int? Number { get; set; }

      /// <summary>
      /// Gets if we should serialize the number.
      /// </summary>
      public bool ShouldSerializeNumber () { return this.Number.HasValue; }

      /// <summary>
      /// Gets or Sets the order, if we have it.
      /// </summary>
      [JsonProperty ("o")]
      public int? Order { get; set; }

      /// <summary>
      /// Gets if we should serialize the order.
      /// </summary>
      public bool ShouldSerializeOrder () { return this.Order.HasValue; }

      /// <summary>
      /// Gets or Sets the parent, if we have it.
      /// </summary>
      [JsonProperty ("p")]
      public int? Parent { get; set; }

      /// <summary>
      /// Gets if we should serialize the parent.
      /// </summary>
      public bool ShouldSerializeParent () { return this.Parent.HasValue; }

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
      /// Gets or Sets the text for the truth, if we have it.
      /// </summary>
      [JsonProperty ("t")]
      public string Text { get; set; }

      /// <summary>
      /// Gets if we should serialize the text.
      /// </summary>
      public bool ShouldSerializeText () { return !string.IsNullOrEmpty(this.Text); }
   }
}