using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Database.Contexts;

namespace VOD.Database.Services
{
    public class DbWriteService : IDbWriteService
    {
        #region Properties
        private readonly VODContext _db;
        #endregion

        #region Constructor
        public DbWriteService(VODContext db)
        {
            _db = db;
        }
        #endregion

        #region Methods
        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                return await _db.SaveChangesAsync() >= 0;
            }
            catch
            {
                return false;
            }
        }
        public void Add<TEntity>(TEntity item) where TEntity : class
        {
            try
            {
                _db.Add(item);
            }
            catch
            {
                throw;
            }
        }
        public void Delete<TEntity>(TEntity item) where TEntity : class
        {
            try
            {
                _db.Set<TEntity>().Remove(item);
            }
            catch
            {
                throw;
            }
        }

        #endregion
    }
}
