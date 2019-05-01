using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Common.Services;

namespace VOD.Admin.Pages.Videos
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        #region Properties
        private readonly IAdminService _db;
        [BindProperty] public VideoDTO Input { get; set; } = new VideoDTO();
        [TempData] public string Alert { get; set; }
        #endregion

        #region Constructor
        public EditModel(IAdminService db)
        {
            _db = db;
        }
        #endregion

        #region Actions
        public async Task<IActionResult> OnGetAsync(int id, int moduleId, int courseId )
        {
            try
            {
                Alert = string.Empty;
                ViewData["Modules"] = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "CourseAndModule");
                Input = await _db.SingleAsync<Video, VideoDTO>(s => s.Id.Equals(id) && s.ModuleId.Equals(moduleId) && s.CourseId.Equals(courseId), true);
                return Page();
            }
            catch
            {
                return RedirectToPage("/Index", new { alert = "You do not have access to this page." }); ;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var id = Input.ModuleId;
                Input.CourseId = (await _db.SingleAsync<Module, ModuleDTO>(s => s.Id.Equals(id))).CourseId;
                var succeeded = await _db.UpdateAsync<VideoDTO, Video>(Input);
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Updated Course: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }

            // Reload the modules when the page is reloaded
            ViewData["Modules"] = (await _db.GetAsync<Module, ModuleDTO>(true)).ToSelectList("Id", "CourseAndModule");
            // Something failed, redisplay the form.
            return Page();
        }
        #endregion
    }
}