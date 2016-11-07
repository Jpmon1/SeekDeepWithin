namespace IinAll.Edit.Data.Save
{
   /// <summary>
   /// Represents a saved light.
   /// </summary>
   public class SaveLight
   {
      /// <summary>
      /// Initializes a new save light.
      /// </summary>
      public SaveLight () { }
      
      /// <summary>
      /// Initializes a new save light.
      /// </summary>
      public SaveLight (Light light)
      {
         this.Id = light.Id;
         this.Text = light.Text;
      }

      /// <summary>
      /// Gets or Sets the light's id.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the light's text.
      /// </summary>
      public string Text { get; set; }
   }
}
