using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Tests.Mocks
{
   /// <summary>
   /// A mocked database implementation.
   /// </summary>
   public class MockDatabase : ISdwDatabase
   {
      private IRepository<Love> m_Loves;
      private IRepository<Soul> m_Souls;
      private IRepository<Light> m_Lights;
      private IRepository<Truth> m_Truths;
      private IRepository<Peace> m_Peaces;
      private IRepository<Style> m_Styles;
      private IRepository<TruthStyle> m_TruthStyles;
      private IRepository<FormatRegex> m_RegexFormats;

      /// <summary>
      /// Gets the repository for books.
      /// </summary>
      public IRepository<Light> Light
      {
         get { return this.m_Lights ?? (this.m_Lights = new MockRepository<Light> ()); }
      }

      /// <summary>
      /// Gets the repository for loves.
      /// </summary>
      public IRepository<Love> Love
      {
         get { return this.m_Loves ?? (this.m_Loves = new MockRepository<Love> ()); }
      }

      /// <summary>
      /// Gets the repository for soul.
      /// </summary>
      public IRepository<Soul> Soul
      {
         get { return this.m_Souls ?? (this.m_Souls = new MockRepository<Soul> ()); }
      }

      /// <summary>
      /// Gets the repository for truth.
      /// </summary>
      public IRepository<Truth> Truth
      {
         get { return this.m_Truths ?? (this.m_Truths = new MockRepository<Truth> ()); }
      }

      /// <summary>
      /// Gets the repository for peace.
      /// </summary>
      public IRepository<Peace> Peace
      {
         get { return this.m_Peaces ?? (this.m_Peaces = new MockRepository<Peace> ()); }
      }

      /// <summary>
      /// Gets the repository for formatting regular expressions.
      /// </summary>
      public IRepository<FormatRegex> RegexFormats
      {
         get { return this.m_RegexFormats ?? (this.m_RegexFormats = new MockRepository<FormatRegex> ()); }
      }

      /// <summary>
      /// Gets the repository for styles.
      /// </summary>
      public IRepository<Style> Styles
      {
         get { return this.m_Styles ?? (this.m_Styles = new MockRepository<Style> ()); }
      }

      /// <summary>
      /// Gets the repository for truth styles.
      /// </summary>
      public IRepository<TruthStyle> TruthStyles
      {
         get { return this.m_TruthStyles ?? (this.m_TruthStyles = new MockRepository<TruthStyle> ()); }
      }

      /// <summary>
      /// Saves all changes.
      /// </summary>
      public void Save () {}

      /// <summary>
      /// Sets the values of the given object, with the given values.
      /// </summary>
      /// <param name="item">The database item to set the values for.</param>
      /// <param name="values">The values to update the item with.</param>
      public void SetValues (object item, object values) {}

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose ()
      {
      }
   }
}
