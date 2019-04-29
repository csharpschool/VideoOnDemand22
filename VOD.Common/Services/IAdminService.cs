using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Common.Services
{
    public interface IAdminService
    {
        #region CRUD Methods
        Task<List<TDestination>> GetAsync<TSource, TDestination>(bool include = false) where TSource : class where TDestination : class;
        Task<List<TDestination>> GetAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false) where TSource : class where TDestination : class;
        Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false) where TSource : class where TDestination : class;
        Task<int> CreateAsync<TSource, TDestination>(TSource item) where TSource : class where TDestination : class;
        Task<bool> UpdateAsync<TSource, TDestination>(TSource item) where TSource : class where TDestination : class;
        Task<bool> DeleteAsync<TSource>(Expression<Func<TSource, bool>> expression) where TSource : class;
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
        #endregion
    }
}
