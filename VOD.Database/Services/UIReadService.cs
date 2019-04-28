﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<Course> GetCourse(string userId, int courseId)
        {
            _db.Include<Course, Module>();
            var userCourse = await _db.SingleAsync<UserCourse>(c => c.UserId.Equals(userId) && c.CourseId.Equals(courseId));
            if (userCourse == null) return default;
            return userCourse.Course;
        }

        public async Task<IEnumerable<Course>> GetCourses(string userId)
        {
            _db.Include<UserCourse>();
            var userCourses = await _db.GetAsync<UserCourse>(uc => uc.UserId.Equals(userId));
            return userCourses.Select(c => c.Course);

        }

        public async Task<Video> GetVideo(string userId, int videoId)
        {
            _db.Include<Course>();
            var video = await _db.SingleAsync<Video>(v => v.Id.Equals(videoId));
            if (video == null) return default;
            var userCourse = await _db.SingleAsync<UserCourse>(c => c.UserId.Equals(userId) && c.CourseId.Equals(video.CourseId));
            if (userCourse == null) return default;
            return video;
        }

        public async Task<IEnumerable<Video>> GetVideos(string userId, int moduleId = 0)
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
