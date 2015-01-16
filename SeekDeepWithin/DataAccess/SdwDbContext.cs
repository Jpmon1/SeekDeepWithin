using System.Data.Entity;
using SeekDeepWithin.Domain;
using SeekDeepWithin.Migrations;

namespace SeekDeepWithin.DataAccess
{
   public class SdwDbContext : DbContext
   {
      /// <summary>
      /// Initializes the seek deep within database context.
      /// </summary>
      public SdwDbContext () : base ("SdwConnection") { }

      public DbSet<Book> Books { get; set; }

      public DbSet<Version> Versions { get; set; }

      public DbSet<SubBook> SubBooks { get; set; }

      public DbSet<Chapter> Chapters { get; set; }

      public DbSet<Passage> Passages { get; set; }

      public DbSet<Author> Authors { get; set; }

      public DbSet<Link> Links { get; set; }

      public DbSet<Tag> Tags { get; set; }

      public DbSet<Style> Styles { get; set; }

      public DbSet<Source> Sources { get; set; }

      public DbSet<ChapterHeader> ChapterHeaders { get; set; }

      public DbSet<ChapterFooter> ChapterFooters { get; set; }

      public DbSet<PassageHeader> PassageHeaders { get; set; }

      public DbSet<PassageFooter> PassageFooters { get; set; }

      public DbSet<GlossaryTerm> GlossaryItems { get; set; }

      public DbSet<GlossaryEntry> GlossaryEntries { get; set; }

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
         Database.SetInitializer (new MigrateDatabaseToLatestVersion <SdwDbContext, Configuration> ());
      }
   }
}
