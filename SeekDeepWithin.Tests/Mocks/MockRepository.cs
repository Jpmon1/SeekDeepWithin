using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SeekDeepWithin.DataAccess;
using SeekDeepWithin.Pocos;

namespace SeekDeepWithin.Tests.Mocks
{
   /// <summary>
   /// A mocked repository.
   /// </summary>
   /// <typeparam name="T">Type of mocked repository.</typeparam>
   public class MockRepository<T> : IRepository<T> where T : IDbTable
   {
      private readonly List <T> m_MockedObects;

      /// <summary>
      /// Initializes a new mock repository.
      /// </summary>
      public MockRepository ()
      {
         this.m_MockedObects = new List <T> ();
      }

      /// <summary>
      /// Gets the list of mocked objects for this repository.
      /// </summary>
      public List<T> MockedObjects { get { return this.m_MockedObects; } }

      /// <summary>
      /// Lists all of the items in the database.
      /// </summary>
      /// <returns>Returns all of the items in the table.</returns>
      public List<T> All (Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
      {
         return this.m_MockedObects;
      }

      /// <summary>
      /// Gets the requested entity from the database.
      /// </summary>
      /// <param name="filter">Filter to apply.</param>
      /// <param name="orderBy">Order clause.</param>
      /// <param name="includeProperties">Properties to include.</param>
      /// <returns>The list of items retrieved from the query.</returns>
      public IEnumerable <T> Get (Expression <Func <T, bool>> filter = null, Func <IQueryable <T>,
         IOrderedQueryable <T>> orderBy = null, string includeProperties = "")
      {
         yield break;
      }

      /// <summary>
      /// Gets the requested entity from the database.
      /// </summary>
      /// <param name="id">Id of Entity to get.</param>
      public T Get (int id)
      {
         return this.m_MockedObects.FirstOrDefault (mo => mo.Id == id);
      }

      /// <summary>
      /// Inserts the given entity into the database.
      /// </summary>
      /// <param name="entity">Entity to insert.</param>
      public void Insert (T entity)
      {
         this.m_MockedObects.Add (entity);
      }

      /// <summary>
      /// Deletes the given entity from the database.
      /// </summary>
      /// <param name="id">Id of entity to delete.</param>
      public void Delete (int id)
      {
         this.Delete (this.m_MockedObects.FirstOrDefault (mo => mo.Id == id));
      }

      /// <summary>
      /// Deletes the given entity from the database.
      /// </summary>
      /// <param name="entity">Entity to delete.</param>
      public void Delete (T entity)
      {
         if (entity != null)
            this.m_MockedObects.Remove (entity);
      }

      /// <summary>
      /// Updates the given entity from the database.
      /// </summary>
      /// <param name="entity">Entity to update.</param>
      public void Update (T entity)
      {
      }
   }
}
