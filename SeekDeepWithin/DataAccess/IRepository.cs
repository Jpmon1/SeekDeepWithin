using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.DataAccess
{
   /// <summary>
   /// Interface for a repository.
   /// </summary>
   /// <typeparam name="T"></typeparam>
   public interface IRepository <T> where T : IDbTable
   {
      /// <summary>
      /// Lists all of the items in the database.
      /// </summary>
      /// <returns>Returns all of the items in the table.</returns>
      List <T> All ();

      /// <summary>
      /// Gets the requested entity from the database.
      /// </summary>
      /// <param name="filter">Filter to apply.</param>
      /// <param name="orderBy">Order clause.</param>
      /// <param name="includeProperties">Properties to include.</param>
      /// <returns>The list of items retrieved from the query.</returns>
      IEnumerable <T> Get (Expression <Func <T, bool>> filter = null, Func <IQueryable <T>,
         IOrderedQueryable <T>> orderBy = null, string includeProperties = "");

      /// <summary>
      /// Gets the requested entity from the database.
      /// </summary>
      /// <param name="id">Id of Entity to get.</param>
      T Get (int id);

      /// <summary>
      /// Inserts the given entity into the database.
      /// </summary>
      /// <param name="entity">Entity to insert.</param>
      void Insert (T entity);

      /// <summary>
      /// Deletes the given entity from the database.
      /// </summary>
      /// <param name="id">Id of entity to delete.</param>
      void Delete (int id);

      /// <summary>
      /// Deletes the given entity from the database.
      /// </summary>
      /// <param name="entity">Entity to delete.</param>
      void Delete (T entity);

      /// <summary>
      /// Updates the given entity from the database.
      /// </summary>
      /// <param name="entity">Entity to update.</param>
      void Update (T entity);
   }
}
