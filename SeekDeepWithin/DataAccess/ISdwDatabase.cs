using System;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.DataAccess
{
   public interface ISdwDatabase : IDisposable
   {
      /// <summary>
      /// Gets the repository for lights.
      /// </summary>
      IRepository<Light> Light { get; }

      /// <summary>
      /// Gets the repository for loves.
      /// </summary>
      IRepository<Love> Love { get; }

      /// <summary>
      /// Gets the repository for truth.
      /// </summary>
      IRepository<Truth> Truth { get; }

      /// <summary>
      /// Gets the repository for formatting regular expressions.
      /// </summary>
      IRepository<FormatRegex> RegexFormats { get; }

      /// <summary>
      /// Gets the repository for styles.
      /// </summary>
      IRepository<Style> Styles { get; }

      /// <summary>
      /// Saves all changes.
      /// </summary>
      void Save ();

      /// <summary>
      /// Sets the values of the given object, with the given values.
      /// </summary>
      /// <param name="item">The database item to set the values for.</param>
      /// <param name="values">The values to update the item with.</param>
      void SetValues (object item, object values);
   }
}
