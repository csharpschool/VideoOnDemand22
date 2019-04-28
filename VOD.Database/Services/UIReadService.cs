using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.Entities;

namespace VOD.Database.Services
{
    public class UIReadService : IUIReadService
    {
        #region Properties
        private readonly IDbReadService _db;
        #endregion

        #region Constructor
        public UIReadService(IDbReadService db)
        {
            _db = db;
        }
        #endregion

        #region Methods
        public Task<Course> GetCourse(string userId, int courseId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetCourses(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Video> GetVideo(string userId, int videoId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Video>> GetVideos(string userId, int moduleId = 0)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
