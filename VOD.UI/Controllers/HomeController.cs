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
        private SignInManager<VODUser> _signInManager;
        private IDbReadService _db;
        #endregion

        #region Constructor
        public HomeController(SignInManager<VODUser> signInMgr, IDbReadService db)
        {
            _signInManager = signInMgr;
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> Index()
        {
            _db.Include<Download>();
            _db.Include<Module, Course>();
            var result1 = await _db.SingleAsync<Download>(d => d.Id.Equals(3));
            var result2 = await _db.GetAsync<Download>(); // Fetch all
            var result3 = await _db.GetAsync<Download>(d => d.ModuleId.Equals(1)); // Fetch all that matches the Lambda expression
            var result4 = await _db.AnyAsync<Download>(d => d.ModuleId.Equals(1)); // True if a record is found
            
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
