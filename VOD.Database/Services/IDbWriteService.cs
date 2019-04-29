using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Database.Services
{
    public interface IDbWriteService
    {
        Task<bool> SaveChangesAsync();
        void Add<TEntity>(TEntity item) where TEntity : class;
    }
}
