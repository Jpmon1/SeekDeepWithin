namespace SeekDeepWithin.Models
{
   public class SearchResult
   {

      /// <summary>
      /// Gets the id of the result.
      /// </summary>
      public int Id { get; set; }

      /// <summary>
      /// Gets or Sets the search type.
      /// </summary>
      public SearchType Type { get; set; }

      /// <summary>
      /// Gets or Sets the title of the search item.
      /// </summary>
      public string Title { get; set; }

      /// <summary>
      /// Gets or Sets the description of the search item.
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.
      /// </summary>
      /// <returns>
      /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise, false.
      /// </returns>
      /// <param name="obj">The object to compare with the current object. </param>
      public override bool Equals (object obj)
      {
         if (ReferenceEquals (null, obj)) return false;
         if (ReferenceEquals (this, obj)) return true;
         if (obj.GetType () != this.GetType ()) return false;
         return Equals ((SearchResult) obj);
      }
      protected bool Equals (SearchResult other)
      {
         return Id == other.Id && Type == other.Type;
      }

      /// <summary>
      /// Serves as a hash function for a particular type. 
      /// </summary>
      /// <returns>
      /// A hash code for the current <see cref="T:System.Object"/>.
      /// </returns>
      public override int GetHashCode ()
      {
         unchecked
         {
            return (Id * 397) ^ (int) Type;
         }
      }
   }
}