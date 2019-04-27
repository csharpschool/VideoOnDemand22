using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VOD.Database.Contexts;

namespace VOD.Database.Services
{
    public class DbReadService : IDbReadService
    {
        #region Properties
        private VODContext _db;
        #endregion

        #region Constructor
        public DbReadService(VODContext db)
        {
            _db = db;
        }
        #endregion

        #region Read Methods

        public Task<List<TEntity>> GetAsync<TEntity>() where TEntity : class
        {
            return _db.Set<TEntity>().ToListAsync();
        }
        #endregion
    }
}
