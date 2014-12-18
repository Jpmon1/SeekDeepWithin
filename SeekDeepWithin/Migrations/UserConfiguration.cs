using System.Data.Entity.Migrations;
using SeekDeepWithin.DataAccess;

namespace SeekDeepWithin.Migrations
{
   internal class UserConfiguration : DbMigrationsConfiguration<UsersContext>
   {
      public UserConfiguration ()
      {
         AutomaticMigrationsEnabled = true;
         AutomaticMigrationDataLossAllowed = true;
      }

      protected override void Seed (UsersContext context)
      {
      }
   }
}