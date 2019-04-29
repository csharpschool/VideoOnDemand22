using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Database.Services;

namespace VOD.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        #region Properties
        private readonly IDbReadService _dbRead;
        private readonly IDbWriteService _dbWrite;
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
        public SelectList AvailableCourses { get; set; }
        [BindProperty, Display(Name = "Available Courses")] public int CourseId { get; set; }
        public UserDTO Customer { get; set; }
        #endregion

        #region Constructor
        public DetailsModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbRead = dbReadService;
            _dbWrite = dbWriteService;
        }
        #endregion

        #region Helper Methods
        private async Task FillViewData(string userId)
        {
            // Fetch the user/customer
            var user = await _dbRead.SingleAsync<VODUser>(u => u.Id.Equals(userId));
            Customer = new UserDTO { Id = user.Id, Email = user.Email };

            // Fetch the user's courses and course ids
            _dbRead.Include<UserCourse>();
            var userCourses = await _dbRead.GetAsync<UserCourse>(uc => uc.UserId.Equals(userId));
            var usersCourseIds = userCourses.Select(c => c.CourseId).ToList();
            Courses = userCourses.Select(c => c.Course).ToList();

            // Fetch courses that the user doesn't already have access to
            var availableCourses = await _dbRead.GetAsync<Course>(uc => !usersCourseIds.Contains(uc.Id));
            AvailableCourses = availableCourses.ToSelectList("Id", "Title");
        }
        #endregion

        #region Actions
        public async Task OnGetAsync(string id)
        {
            await FillViewData(id);
        }
        public async Task<IActionResult> OnPostAddAsync(string userId)
        {
            try
            {
                _dbWrite.Add(new UserCourse
                {
                    CourseId = CourseId,
                    UserId = userId
                });
                var succeeded = await _dbWrite.SaveChangesAsync();
            }
            catch
            {
            }

            await FillViewData(userId);
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveAsync(int courseId, string userId)
        {
            try
            {
                var userCourse = await _dbRead.SingleAsync<UserCourse>(uc =>
                    uc.UserId.Equals(userId) &&
                    uc.CourseId.Equals(courseId));

                if (userCourse != null)
                {
                    _dbWrite.Delete(userCourse);
                    await _dbWrite.SaveChangesAsync();
                }
            }
            catch
            {
            }

            await FillViewData(userId);
            return Page();
        }
        #endregion
    }
}