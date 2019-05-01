using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Common.Services
{
    public class AdminAPIService : IAdminService
    {
        #region Properties
        private readonly IHttpClientFactoryService _http;
        #endregion

        #region Constructor
        public AdminAPIService(IHttpClientFactoryService http)
        {
            _http = http;
        }
        #endregion

        #region Methods
        public Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync<TSource>(Expression<Func<TSource, bool>> expression) where TSource : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TDestination>> GetAsync<TSource, TDestination>(bool include = false)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TDestination>> GetAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
