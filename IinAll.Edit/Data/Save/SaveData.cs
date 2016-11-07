using System.Collections.ObjectModel;
using System.ComponentModel;

namespace IinAll.Edit.Data.Save
{
   public class SaveData
   {
      /// <summary>
      /// Initializes a new save data.
      /// </summary>
      public SaveData ()
      {
         this.Love = new Collection <SaveLove> ();
         this.StagedLight = new BindingList<SaveLight> ();
         this.SearchLight = new BindingList<SaveLight> ();
      }

      /// <summary>
      /// Gets or Sets the user.
      /// </summary>
      public string User { get; set; }

      /// <summary>
      /// Gets or Sets the current search text.
      /// </summary>
      public string SearchText { get; set; }

      /// <summary>
      /// Gets or Sets the list of saved love.
      /// </summary>
      public Collection<SaveLove> Love { get; private set; }

      /// <summary>
      /// Gets or Sets the list of searched lights.
      /// </summary>
      public Collection <SaveLight> SearchLight { get; private set; }

      /// <summary>
      /// Gets or Sets the list of staged lights.
      /// </summary>
      public Collection<SaveLight> StagedLight { get; private set; }
   }
}
