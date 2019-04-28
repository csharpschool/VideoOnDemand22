using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VOD.Common.DTOModels.UI;
using VOD.Common.Entities;
using VOD.Database.Services;
using VOD.UI.Models.MembershipViewModels;

namespace VOD.UI.Controllers
{
    public class MembershipController : Controller
    {
        #region Properties
        private readonly string _userId;
        private readonly IMapper _mapper;
        private readonly IUIReadService _db;
        #endregion

        #region Constructor
        public MembershipController(IHttpContextAccessor httpContextAccessor, UserManager<VODUser> userManager, IMapper mapper, IUIReadService db)
        {
            var user = httpContextAccessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _db = db;
        }
        #endregion

        #region Action Methods
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var courseDtoObjects = _mapper.Map<List<CourseDTO>>(await _db.GetCourses(_userId));
            var dashboardModel = new DashboardViewModel();
            dashboardModel.Courses = new List<List<CourseDTO>>();

            var noOfRows = courseDtoObjects.Count <= 3 ? 1 : courseDtoObjects.Count / 3;
            for (var i = 0; i < noOfRows; i++)
            {
                dashboardModel.Courses.Add(courseDtoObjects.Skip(i * 3).Take(3).ToList());
            }

            return View(dashboardModel);
        }

        [HttpGet]
        public async Task<IActionResult> Course(int id)
        {
            var course = await _db.GetCourse(_userId, id);
            var mappedCourseDTO = _mapper.Map<CourseDTO>(course);
            var mappedInstructorDTO = _mapper.Map<InstructorDTO>(course.Instructor);
            var mappedModuleDTOs = _mapper.Map<List<ModuleDTO>>(course.Modules);

            var courseModel = new CourseViewModel
            {
                Course = mappedCourseDTO,
                Instructor = mappedInstructorDTO,
                Modules = mappedModuleDTOs
            };

            return View(courseModel);
        }

        [HttpGet]
        public async Task<IActionResult> Video(int id)
        {
            return View();
        }
        #endregion

    }
}