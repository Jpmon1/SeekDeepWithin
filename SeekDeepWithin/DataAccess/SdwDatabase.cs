using System;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.DataAccess
{
   /// <summary>
   /// Access to the seek deep within database.
   /// </summary>
   public class SdwDatabase : ISdwDatabase
   {
      private bool m_Disposed;
      private IRepository<Light> m_Lights;
      private IRepository<Love> m_Loves;
      private IRepository<Truth> m_Truths;
      private IRepository<Style> m_Styles;
      private IRepository<FormatRegex> m_RegexFormats;
      private readonly SdwDbContext m_Db = new SdwDbContext ();

      /// <summary>
      /// Gets the repository for light.
      /// </summary>
      public IRepository<Light> Light
      {
         get { return this.m_Lights ?? (this.m_Lights = new Repository<Light> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for love.
      /// </summary>
      public IRepository<Love> Love
      {
         get { return this.m_Loves ?? (this.m_Loves = new Repository<Love> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for truth.
      /// </summary>
      public IRepository<Truth> Truth
      {
         get { return this.m_Truths ?? (this.m_Truths = new Repository<Truth> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for formatting regular expressions.
      /// </summary>
      public IRepository<FormatRegex> RegexFormats
      {
         get { return this.m_RegexFormats ?? (this.m_RegexFormats = new Repository<FormatRegex> (m_Db)); }
      }

      /// <summary>
      /// Gets the repository for styles.
      /// </summary>
      public IRepository<Style> Styles
      {
         get { return this.m_Styles ?? (this.m_Styles = new Repository<Style> (m_Db)); }
      }

      /// <summary>
      /// Sets the values of the given object, with the given values.
      /// </summary>
      /// <param name="item">The database item to set the values for.</param>
      /// <param name="values">The values to update the item with.</param>
      public void SetValues (object item, object values)
      {
         this.m_Db.Entry (item).CurrentValues.SetValues (values);
      }

      /// <summary>
      /// Saves all changes.
      /// </summary>
      public void Save ()
      {
         this.m_Db.SaveChanges ();
      }

      /// <summary>
      /// Disposes of any objects.
      /// </summary>
      public void Dispose ()
      {
         this.Dispose (true);
         GC.SuppressFinalize (this);
      }

      /// <summary>
      /// Disploses of the object, if not already disposed.
      /// </summary>
      /// <param name="disposing">True to dispose.</param>
      protected virtual void Dispose (bool disposing)
      {
         if (!this.m_Disposed)
         {
            if (disposing)
            {
               this.m_Db.Dispose ();
            }
         }
         this.m_Disposed = true;
      }
   }
}
