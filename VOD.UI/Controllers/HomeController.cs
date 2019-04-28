using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VOD.UI.Models;
using Microsoft.AspNetCore.Identity;
using VOD.Common.Entities;
using VOD.Database.Services;

namespace VOD.UI.Controllers
{
    public class HomeController : Controller
    {
        #region Properties
        private readonly SignInManager<VODUser> _signInManager;
        private readonly IUIReadService _db;
        #endregion

        #region Constructor
        public HomeController(SignInManager<VODUser> signInMgr, IUIReadService db)
        {
            _signInManager = signInMgr;
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> Index()
        {
            var courses = (await _db.GetCourses("34d49bd3-df89-4472-b6a6-f358ee92f016")).ToList();
            var course = await _db.GetCourse("34d49bd3-df89-4472-b6a6-f358ee92f016", 1);
            var video = await _db.GetVideo("34d49bd3-df89-4472-b6a6-f358ee92f016", 1);

            if (!_signInManager.IsSignedIn(User))
                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}
