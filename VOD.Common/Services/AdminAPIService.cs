using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
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
        private void GetProperties<TSource>(Expression<Func<TSource, bool>> expression)
        {
            try
            {
                var lambda = expression as LambdaExpression;
                _properties.Clear();

                ResolveExpression(lambda.Body);

                var typeName = typeof(TSource).Name;

                if (!typeName.Equals("Instructor") && 
                    !typeName.Equals("Course") && 
                    !_properties.ContainsKey("courseId"))
                        _properties.Add("courseId", 0);

            }
            catch 
            {

                throw;
            }
        }
        private void GetProperties<TSource>(TSource source)
        {
            try
            {
                _properties.Clear();

                var idProperty = source.GetType().GetProperty("Id");
                var moduleProperty = source.GetType().GetProperty("ModuleId");
                var courseProperty = source.GetType().GetProperty("CourseId");

                if (idProperty != null)
                {
                    var id = idProperty.GetValue(source);
                    if (id != null && (int)id > 0) _properties.Add("id", id);
                }

                if (moduleProperty != null)
                {
                    var moduleId = moduleProperty.GetValue(source);
                    if (moduleId != null && (int)moduleId > 0)
                        _properties.Add("moduleId", moduleId);
                }

                if (courseProperty != null)
                {
                    var courseId = courseProperty.GetValue(source);
                    if (courseId != null && (int)courseId > 0)
                        _properties.Add("courseId", courseId);
                }
            }
            catch
            {
                _properties.Clear();
                throw;
            }
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
        private string FormatUriWithIds<TSource>()
        {
            string uri = "api";
            object id, moduleId, courseId;

            bool succeeeded = _properties.TryGetValue("courseId", out courseId);
            if (succeeeded) uri = $"{uri}/courses/{courseId}";

            succeeeded = _properties.TryGetValue("moduleId", out moduleId);
            if (succeeeded) uri = $"{uri}/modules/{moduleId}";

            succeeeded = _properties.TryGetValue("id", out id);
            if (id != null)
                uri = $"{uri}/{typeof(TSource).Name}s/{id}";
            else
                uri = $"{uri}/{typeof(TSource).Name}s";

            return uri;
        }
        private void GetExpressionProperties(Expression expression)
        {
            var body = (MethodCallExpression)expression;
            var argument = body.Arguments[0];
            if (argument is MemberExpression)
            {
                var memberExpression = (MemberExpression)argument;
                var value = ((FieldInfo)memberExpression.Member).GetValue(((ConstantExpression)memberExpression.Expression).Value);

                _properties.Add(memberExpression.Member.Name, value);
            }
        }
        private void ResolveExpression(Expression expression)
        {
            try
            {
                if (expression.NodeType == ExpressionType.AndAlso)
                {
                    var binaryExpression = expression as BinaryExpression;

                    // Recursive calls to get at the MethodCallExpressions
                    ResolveExpression(binaryExpression.Left);
                    ResolveExpression(binaryExpression.Right);
                }
                else if (expression is MethodCallExpression)
                {
                    GetExpressionProperties(expression);
                }
            }
            catch
            {

                throw;
            }
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

        public async Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false) where TSource : class where TDestination : class
        {
            try
            {
                GetProperties(expression);
                string uri = FormatUriWithIds<TSource>();
                return await _http.GetAsync<TDestination>($"{uri}?include={include.ToString()}", "AdminClient");

            }
            catch
            {

                throw;
            }
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
