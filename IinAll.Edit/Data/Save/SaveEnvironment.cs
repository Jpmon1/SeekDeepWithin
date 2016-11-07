using IinAll.Edit.Logic;

namespace IinAll.Edit.Data.Save
{
   /// <summary>
   /// Represents the save environment.
   /// </summary>
   public class SaveEnvironment
   {
      /// <summary>
      /// Initializes a new save environment.
      /// </summary>
      public SaveEnvironment ()
      {
         this.LocalData = new SaveData();
         this.ProductionData = new SaveData();
      }

      /// <summary>
      /// Gets or Sets if we are in production mode or not.
      /// </summary>
      public bool UseProduction { get; set; }

      /// <summary>
      /// Gets or Sets the local data.
      /// </summary>
      public SaveData LocalData { get; set; }

      /// <summary>
      /// Gets or Sets the production data.
      /// </summary>
      public SaveData ProductionData { get; set; }

      /// <summary>
      /// Sets data.
      /// </summary>
      /// <param name="model">The view model with data..</param>
      public void SetData (MainViewModel model)
      {
         this.UseProduction = model.UseProduction;
         var data = this.UseProduction ? this.ProductionData : this.LocalData;
         data.User = model.UserName;
         data.SearchText = model.Light.SearchText;
         data.SearchLight.Clear ();
         foreach (var result in model.Light.SearchResults)
            data.SearchLight.Add (new SaveLight (result));
         data.StagedLight.Clear ();
         foreach (var light in model.Light.Light)
            data.StagedLight.Add (new SaveLight (light));
         data.Love.Clear ();
         foreach (var love in model.Love)
            data.Love.Add (new SaveLove (love));
      }
   }
}
