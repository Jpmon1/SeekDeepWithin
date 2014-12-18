using SeekDeepWithin.DataAccess;
using System.Data.Entity.Migrations;

namespace SeekDeepWithin.Migrations
{
   internal sealed class Configuration : DbMigrationsConfiguration<SdwDbContext>
   {
      public Configuration ()
      {
         AutomaticMigrationsEnabled = true;
         AutomaticMigrationDataLossAllowed = true;
      }

      protected override void Seed (SdwDbContext context)
      {
      }
   }
}
