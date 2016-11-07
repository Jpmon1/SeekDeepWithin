using IinAll.Edit.Data;

namespace IinAll.Edit.Interfaces
{
   /// <summary>
   /// Represents a body container.
   /// </summary>
   public interface IBodyContainer
   {
      /// <summary>
      /// Removes a body from the bodies.
      /// </summary>
      /// <param name="body">The body to remove.</param>
      void RemoveBody (Body body);
   }
}
