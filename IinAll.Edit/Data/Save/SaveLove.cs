using System.Collections.ObjectModel;
using IinAll.Edit.Logic;

namespace IinAll.Edit.Data.Save
{
   /// <summary>
   /// Represents a saved love.
   /// </summary>
   public class SaveLove
   {
      /// <summary>
      /// Initializes a new save love.
      /// </summary>
      public SaveLove ()
      {
         this.Init ();
      }

      /// <summary>
      /// Initializes a new saved love.
      /// </summary>
      /// <param name="love">Love to save.</param>
      public SaveLove (LoveViewModel love)
      {
         this.Init();
         foreach (var light in love.Light)
            this.Light.Add(new SaveLight (light));
         foreach (var light in love.EditLight)
            this.EditLight.Add (new SaveLight (light));
         this.CurrentRegex = love.CurrentRegex;
         this.AddText = love.TextToFormat;
         this.IsExpanded = love.IsExpanded;
      }

      /// <summary>
      /// Gets or Sets the text in the add tab.
      /// </summary>
      public string AddText { get; set; }

      /// <summary>
      /// Gets or Sets the current regex.
      /// </summary>
      public string CurrentRegex { get; set; }

      /// <summary>
      /// Gets or Sets if the love is expanded or not.
      /// </summary>
      public bool IsExpanded { get; set; }

      /// <summary>
      /// Gets or Sets the light.
      /// </summary>
      public Collection <SaveLight> Light { get; private set; }

      /// <summary>
      /// Gets or Sets the edit light.
      /// </summary>
      public Collection<SaveLight> EditLight { get; private set; }

      /// <summary>
      /// Initializes the love.
      /// </summary>
      private void Init ()
      {
         this.Light = new Collection<SaveLight> ();
         this.EditLight = new Collection<SaveLight> ();
      }
   }
}
