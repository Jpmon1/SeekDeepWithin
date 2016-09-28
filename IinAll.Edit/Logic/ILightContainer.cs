using IinAll.Edit.Data;

namespace IinAll.Edit.Logic
{
   public interface ILightContainer
   {
      /// <summary>
      /// Removes the given light.
      /// </summary>
      /// <param name="light">Light to remove.</param>
      void RemoveLight (Light light);
   }
}
