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
        Dictionary<string, object> _properties = new Dictionary<string, object>();
        #endregion

        #region Constructor
        public AdminAPIService(IHttpClientFactoryService http)
        {
            _http = http;
        }
        #endregion

        #region Reflection Methods
        private void GetProperties<TSource>()
        {
            _properties.Clear();
            var type = typeof(TSource);
            var id = type.GetProperty("Id");
            var moduleId = type.GetProperty("ModuleId");
            var courseId = type.GetProperty("CourseId");

            if (id != null) _properties.Add("id", 0);
            if (moduleId != null) _properties.Add("moduleId", 0);
            if (courseId != null) _properties.Add("courseId", 0);

        }
        private string FormatUriWithoutIds<TSource>()
        {
            string uri = "api";
            object moduleId, courseId;

            // Try to fetch the value for the courseId key in the collection
            // and append its path to the uri variable if it exists.
            bool succeeeded = _properties.TryGetValue("courseId", out courseId);
            if (succeeeded) uri = $"{uri}/courses/0";

            succeeeded = _properties.TryGetValue("moduleId", out moduleId);
            if (succeeeded) uri = $"{uri}/modules/0";

            uri = $"{uri}/{typeof(TSource).Name}s";

            return uri;
        }
        #endregion

        #region Methods
        public async Task<List<TDestination>> GetAsync<TSource, TDestination>(bool include = false) where TSource : class where TDestination : class
        {
            try
            {
                GetProperties<TSource>();
                string uri = FormatUriWithoutIds<TSource>();
                return await _http.GetListAsync<TDestination>($"{uri}?include={include.ToString()}", "AdminClient");
            }
            catch
            {
                throw;
            }
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

        public Task<bool> UpdateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
