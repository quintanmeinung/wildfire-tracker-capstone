using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Concrete
{
    /// <summary>
    /// Base class repository that implements common and CRUD operations.  Meant to be like an abstract 
    /// base class, but not actually made abstract because it is often useful to have a repository 
    /// that only supports these common operations.
    /// 
    /// This is only a minimal version. There is quite a lot we could add to this, including:
    ///    - add better error checking/recovery (i.e. throw exceptions when something goes wrong; write a 
    ///      custom exception class to handle errors)
    ///    - Write a base model class for the parameterized type, i.e. require TEntity : ModelBase, 
    ///      and have ModelBase define important things like the PK name and type so we can enforce that in
    ///      FindById for example.
    ///    - Async versions
    /// </summary>
    /// <typeparam name="TEntity">This is the entity for which we're making a repository</typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
      
        private readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext ctx)
        {
            _context = ctx;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual TEntity FindById(string identityId)
        {
            TEntity? entity = _dbSet.SingleOrDefault(e => EF.Property<string>(e, "IdentityId") == identityId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID: {identityId} not found.");
            }
            return entity;
        }

        public virtual bool Exists(string identityId)
        {
            return FindById(identityId) != null;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            // note, no includes here and we're returning it as an IQueryable, NOT a DbSet, on purpose
            // so the caller cannot do other things (which should go here or in a subclass)
            return _dbSet;
        }

        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            // Apply includes one by one
            IQueryable<TEntity> dbs = _dbSet;
            foreach (var item in includes)
            {
                dbs = dbs.Include(item);
            }
            return dbs;
        }

        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> dbs = _dbSet;
            return dbs.Where(predicate);
        }

        public virtual TEntity AddOrUpdate(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null to add or update");
            }
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("Entity to delete was not found or was null");
            }
            else
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
            return;
        }

        public virtual void DeleteById(string identityId)
        {
            Delete(FindById(identityId));
            return;
        }
    }
}