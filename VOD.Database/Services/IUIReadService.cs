using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.Entities;

namespace VOD.Database.Services
{
    public interface IUIReadService
    {
        Task<IEnumerable<Course>> GetCourses(string userId);
        Task<Course> GetCourse(string userId, int courseId);
        Task<Video> GetVideo(string userId, int videoId);
        Task<IEnumerable<Video>> GetVideos(string userId, int moduleId = default(int));
    }
}
