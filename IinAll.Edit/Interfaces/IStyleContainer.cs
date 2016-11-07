using IinAll.Edit.Data;

namespace IinAll.Edit.Interfaces
{
   public interface IStyleContainer
   {
      /// <summary>
      /// Removes a style from the parent.
      /// </summary>
      /// <param name="style">The style to remove.</param>
      void RemoveStyle (Style style);
   }
}
