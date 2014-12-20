using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using SeekDeepWithin.Domain;

namespace SeekDeepWithin.DataAccess
{
   /// <summary>
   /// A repository of the given type.
   /// </summary>
   /// <typeparam name="T">Type of repository to use.</typeparam>
   public class Repository<T> : IRepository<T> where T : class, IDbTable
   {
      private readonly SdwDbContext m_Db;
      private readonly DbSet <T> m_Table;

      /// <summary>
      /// Initializes a new repository.
      /// </summary>
      /// <param name="db"></param>
      public Repository (SdwDbContext db)
      {
         this.m_Db = db;
         this.m_Table = db.Set <T> ();
      }

      /// <summary>
      /// Lists all of the items in the database.
      /// </summary>
      /// <returns>Returns all of the items in the table.</returns>
      public List<T> All (Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null)
      {
         IQueryable<T> query = this.m_Table;
         if (orderBy != null)
            return orderBy (query).ToList ();
         return query.ToList ();
      }

      /// <summary>
      /// Gets the requested entity from the database.
      /// </summary>
      /// <param name="filter">Filter to apply.</param>
      /// <param name="orderBy">Order clause.</param>
      /// <param name="includeProperties">Properties to include.</param>
      /// <returns>The list of items retrieved from the query.</returns>
      public virtual IEnumerable <T> Get (Expression <Func <T, bool>> filter = null,
         Func <IQueryable <T>, IOrderedQueryable <T>> orderBy = null, string includeProperties = "")
      {
         IQueryable <T> query = this.m_Table;
         if (filter != null)
            query = query.Where (filter);
         query = includeProperties.Split (new [] { ',' }, StringSplitOptions.RemoveEmptyEntries)
            .Aggregate (query, (current, includeProperty) => current.Include (includeProperty));
         if (orderBy != null)
            return orderBy (query).ToList ();
         return query.ToList ();
      }

      /// <summary>
      /// Gets the requested entity from the database.
      /// </summary>
      /// <param name="id">Id of Entity to get.</param>
      public virtual T Get (int id)
      {
         return this.m_Table.Find (id);
      }

      /// <summary>
      /// Inserts the given entity into the database.
      /// </summary>
      /// <param name="entity">Entity to insert.</param>
      public virtual void Insert (T entity)
      {
         this.m_Table.Add (entity);
      }

      /// <summary>
      /// Deletes the given entity from the database.
      /// </summary>
      /// <param name="id">Id of entity to delete.</param>
      public virtual void Delete (int id)
      {
         var entityToDelete = this.m_Table.Find (id);
         this.Delete (entityToDelete);
      }

      /// <summary>
      /// Deletes the given entity from the database.
      /// </summary>
      /// <param name="entity">Entity to delete.</param>
      public virtual void Delete (T entity)
      {
         if (this.m_Db.Entry (entity).State == EntityState.Detached)
            this.m_Table.Attach (entity);
         this.m_Table.Remove (entity);
      }

      /// <summary>
      /// Updates the given entity from the database.
      /// </summary>
      /// <param name="entity">Entity to update.</param>
      public virtual void Update (T entity)
      {
         this.m_Table.Attach (entity);
         this.m_Db.Entry (entity).State = EntityState.Modified;
      }
   }
}