using System;
using System.Data.Entity;
using System.Threading;
using System.Web.Mvc;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Migrations;
using WebMatrix.WebData;

namespace SeekDeepWithin.Filters
{
   [AttributeUsage (AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
   public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
   {
      private static SimpleMembershipInitializer s_Initializer;
      private static object s_InitializerLock = new object ();
      private static bool s_IsInitialized;

      public override void OnActionExecuting (ActionExecutingContext filterContext)
      {
         // Ensure ASP.NET Simple Membership is initialized only once per app start
         LazyInitializer.EnsureInitialized (ref s_Initializer, ref s_IsInitialized, ref s_InitializerLock);
      }

      /// <summary>
      /// Simple memeber ship initializer used for a user options table.
      /// </summary>
      private class SimpleMembershipInitializer
      {
         public SimpleMembershipInitializer ()
         {
            Database.SetInitializer (new MigrateDatabaseToLatestVersion<UsersContext, UserConfiguration> ());

            try {
               using (var context = new UsersContext ()) {
                  if (!context.Database.Exists ()) {
                     context.Database.Create ();
                  }
                  context.UserProfiles.Find (1);
               }

               WebSecurity.InitializeDatabaseConnection ("UserConnection", "UserProfile", "UserId", "Email", autoCreateTables: true);
            } catch (Exception ex) {
               throw new InvalidOperationException ("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
            }
         }
      }
   }
}
