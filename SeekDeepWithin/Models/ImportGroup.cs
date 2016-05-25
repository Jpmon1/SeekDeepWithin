using System.Collections.ObjectModel;
using Newtonsoft.Json;

namespace SeekDeepWithin.Models
{
   /// <summary>
   /// Represents group of light and truths to import.
   /// </summary>
   public class ImportGroup
   {
      private readonly Collection<ImportTruth> m_Truths = new Collection<ImportTruth> ();
      private readonly Collection<ImportLight> m_Lights = new Collection<ImportLight> ();
      private readonly Collection<ImportTruth> m_TruthLinks = new Collection<ImportTruth> ();
      private readonly Collection<Collection<ImportTruth>> m_VersionLinks = new Collection<Collection<ImportTruth>> ();

      /// <summary>
      /// Gets or Sets the light.
      /// </summary>
      [JsonProperty ("l")]
      public Collection<ImportLight> Lights { get { return m_Lights; } }
      
      /// <summary>
      /// Gets or Sets the truths.
      /// </summary>
      [JsonProperty("t")]
      public Collection<ImportTruth> Truths { get { return m_Truths; } }

      /// <summary>
      /// Gets or Sets any links for the truths.
      /// </summary>
      [JsonProperty ("tl")]
      public Collection<ImportTruth> TruthLinks { get { return m_TruthLinks; } }

      /// <summary>
      /// Gets or Sets if the truth links should be serialized or not.
      /// </summary>
      /// <returns></returns>
      public bool ShouldSerializeTruthLinks () { return this.TruthLinks.Count > 0; }

      /// <summary>
      /// Gets or Sets any version links.
      /// </summary>
      [JsonProperty ("vl")]
      public Collection<Collection<ImportTruth>> VersionLinks { get { return m_VersionLinks; } }

      /// <summary>
      /// Gets or Sets if the version links should be serialized or not.
      /// </summary>
      /// <returns></returns>
      public bool ShouldSerializeVersionLinks () { return this.VersionLinks.Count > 0; }
   }
}