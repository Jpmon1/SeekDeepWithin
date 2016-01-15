using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SeekDeepWithin.Pocos;
using SeekDeepWithin.Migrations;

namespace SeekDeepWithin.DataAccess
{
   public class SdwDbContext : DbContext
   {
      /// <summary>
      /// Initializes the seek deep within database context.
      /// </summary>
      public SdwDbContext () : base ("SdwConnection") { }

      /// <summary>
      /// Gets or sets the lights.
      /// </summary>
      public DbSet<Light> Lights { get; set; }

      /// <summary>
      /// Gets or sets the loves.
      /// </summary>
      public DbSet<Love> Loves { get; set; }

      /// <summary>
      /// Gets or sets the souls.
      /// </summary>
      public DbSet<Soul> Souls { get; set; }

      /// <summary>
      /// Gets or sets the truths.
      /// </summary>
      public DbSet<Truth> Truths { get; set; }

      /// <summary>
      /// Gets or sets the peace.
      /// </summary>
      public DbSet<Peace> Peaces { get; set; }

      /// <summary>
      /// Gets or sets the styles.
      /// </summary>
      public DbSet<Style> Styles { get; set; }

      /// <summary>
      /// Gets or sets the regexes used for formatting.
      /// </summary>
      public DbSet<FormatRegex> FormatRegexes { get; set; }

      /// <summary>
      /// This method is called when the model for a derived context has been initialized, but
      ///                 before the model has been locked down and used to initialize the context.  The default
      ///                 implementation of this method does nothing, but it can be overridden in a derived class
      ///                 such that the model can be further configured before it is locked down.
      /// </summary>
      /// <remarks>
      /// Typically, this method is called only once when the first instance of a derived context
      ///                 is created.  The model for that context is then cached and is for all further instances of
      ///                 the context in the app domain.  This caching can be disabled by setting the ModelCaching
      ///                 property on the given ModelBuidler, but note that this can seriously degrade performance.
      ///                 More control over caching is provided through use of the DbModelBuilder and DbContextFactory
      ///                 classes directly.
      /// </remarks>
      /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
      protected override void OnModelCreating (DbModelBuilder modelBuilder)
      {
         Database.SetInitializer (new MigrateDatabaseToLatestVersion<SdwDbContext, Configuration> ());
         modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention> ();
      }
   }
}
