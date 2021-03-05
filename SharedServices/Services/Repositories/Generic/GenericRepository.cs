using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using ServicesLib.Services.Database;
using ServicesLib.Services.Repositories.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ServicesLib.Services.Repository.Generic
{
    public abstract class GenericRepository<T> : IRepository<T> where T : class
    {
        protected readonly SchoolDbContext dbContext;
        public GenericRepository(SchoolDbContext schoolDb)
        {
            dbContext = schoolDb;
        }

        public async virtual Task AddAsync(T entity)
        {
            try
            {
                await dbContext.AddAsync<T>(entity);
            }
            catch (DbUpdateConcurrencyException e)
            {
                var errorMessage = ProcessException("Add db update concurrency error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (DbUpdateException e)
            {
                var errorMessage = ProcessException("Add db update error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (InvalidOperationException e)
            {
                var errorMessage = ProcessException("Add db invalid operation error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (Exception e)
            {
                var errorMessage = ProcessException("Add db general error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
        }

        public async virtual Task<IEnumerable<T>> AllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async virtual Task<T> FindByIdAsync(string Id)
        {
            return await dbContext.FindAsync<T>(Id);
        }

        public async virtual Task SaveChangesAsync()
        {
            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                var errorMessage = ProcessException("UpdateApplianceDetailsAsync: db Update concurrency error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (DbUpdateException e)
            {
                var errorMessage = ProcessException("UpdateApplianceDetailsAsync: db update error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (InvalidOperationException e)
            {
                var errorMessage = ProcessException("UpdateApplianceDetailsAsync: db invalid operation error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (Exception e)
            {
                var errorMessage = ProcessException("UpdateApplianceDetailsAsync: db general error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                return dbContext.Update<T>(entity).Entity;
            }
            catch (DbUpdateConcurrencyException e)
            {
                var errorMessage = ProcessException("UpdateApplianceDetailsAsync: db Update concurrency error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (DbUpdateException e)
            {
                var errorMessage = ProcessException("UpdateApplianceDetailsAsync: db update error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (InvalidOperationException e)
            {
                var errorMessage = ProcessException("UpdateApplianceDetailsAsync: db invalid operation error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (Exception e)
            {
                var errorMessage = ProcessException("UpdateApplianceDetailsAsync: db general error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                dbContext.Remove<T>(entity);
            }
            catch (DbUpdateConcurrencyException e)
            {
                var errorMessage = ProcessException("Delete db Update concurrency error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (DbUpdateException e)
            {
                var errorMessage = ProcessException("Delete db update error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (InvalidOperationException e)
            {
                var errorMessage = ProcessException("Delete db invalid operation error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
            catch (Exception e)
            {
                var errorMessage = ProcessException("Delete db general error: ",
                                                    e.Message,
                                                    e.InnerException,
                                                    ErrorType.Critical);
                throw new DataAccessException(errorMessage);
            }
        }

        private string ProcessException(string msg, string message, Exception innerException, ErrorType critical)
        {
            if (!string.IsNullOrEmpty(message))
                msg += message;
            if (!string.IsNullOrEmpty(innerException.Message))
                msg += message;
            msg += "Error type: " + critical;
            return msg;
        }

        public async virtual Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbContext.Set<T>()
                                  .AsQueryable()
                                  .Where(predicate)
                                  .FirstOrDefaultAsync();
        }
    }
}