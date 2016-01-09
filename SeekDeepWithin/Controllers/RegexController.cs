using SeekDeepWithin.DataAccess;

namespace SeekDeepWithin.Controllers
{
   public class RegexController : SdwController
   {
      /// <summary>
      /// Initializes a new controller.
      /// </summary>
      public RegexController () : base (new SdwDatabase ()) { }

      /// <summary>
      /// Initializes a new controller with the given db info.
      /// </summary>
      /// <param name="db">Database object.</param>
      public RegexController (ISdwDatabase db) : base (db) { }
   }
}