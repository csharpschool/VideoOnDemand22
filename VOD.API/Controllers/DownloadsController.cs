using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Extensions;
using VOD.Common.Services;

namespace VOD.API.Controllers
{
    [Route("api/courses/{courseId}/modules/{moduleId}/downloads")]
    [ApiController]
    public class DownloadsController : ControllerBase
    {
        #region Properties and Variables
        private readonly IAdminService _db;
        private readonly LinkGenerator _linkGenerator;
        #endregion

        #region Constructor
        public DownloadsController(IAdminService db, LinkGenerator
        linkGenerator)
        {
            _db = db;
            _linkGenerator = linkGenerator;
        }
        #endregion

        #region Actions
        [HttpGet()]
        public async Task<ActionResult<List<DownloadDTO>>> Get(int courseId, int moduleId, bool include = false)
        {
            try
            {
                var dtos = courseId.Equals(0) ? 
                    await _db.GetAsync<Download, DownloadDTO>(include) : 
                    await _db.GetAsync<Download, DownloadDTO>(g => g.CourseId.Equals(courseId) && g.ModuleId.Equals(moduleId), include);

                return dtos;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DownloadDTO>> Get(int id, int courseId, int moduleId, bool include = false)
        {
            try
            {
                var dto = courseId.Equals(0) ?
                    await _db.SingleAsync<Download, DownloadDTO>(s => s.Id.Equals(id), include) :
                    await _db.SingleAsync<Download, DownloadDTO>(s => s.Id.Equals(id) && s.CourseId.Equals(courseId) && s.ModuleId.Equals(moduleId), include);

                if (dto == null) return NotFound();

                return dto;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<DownloadDTO>> Post(int courseId, int moduleId, DownloadDTO model)
        {
            try
            {
                if (courseId.Equals(0)) courseId = model.CourseId;
                if (moduleId.Equals(0)) moduleId = model.ModuleId;
                if (model == null) return BadRequest("No entity provided");
                if (!model.CourseId.Equals(courseId)) return BadRequest("Differing ids");
                if (model.Title.IsNullOrEmptyOrWhiteSpace()) return BadRequest("Title is required");



                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(courseId));
                if (!exists) return NotFound("Could not find related entity");

                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(moduleId) && a.CourseId.Equals(courseId));
                if (!exists) return BadRequest("Could not find related entity");

                var id = await _db.CreateAsync<DownloadDTO, Download>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _db.SingleAsync<Download, DownloadDTO>(s => s.CourseId.Equals(courseId) && s.ModuleId.Equals(moduleId) && s.Id.Equals(id));
                if (dto == null) return BadRequest("Unable to add the entity");

                var uri = _linkGenerator.GetPathByAction("Get", "Downloads", new { id, courseId, moduleId });
                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<DownloadDTO>> Put(int id, int courseId, int moduleId, DownloadDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.Id)) return BadRequest("Differing ids");
                if (model.Title.IsNullOrEmptyOrWhiteSpace()) return BadRequest("Title is required");

                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(courseId));
                if (!exists) return NotFound("Could not find related entity");

                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(moduleId) && a.CourseId.Equals(courseId));
                if (!exists) return NotFound("Could not find related entity");

                exists = await _db.AnyAsync<Module>(a => a.Id.Equals(model.ModuleId) && a.CourseId.Equals(model.CourseId));
                if (!exists) return NotFound("Could not find related entity");

                exists = await _db.AnyAsync<Download>(a => a.Id.Equals(id));
                if (!exists) return NotFound("Could not find entity");

                if (await _db.UpdateAsync<DownloadDTO, Download>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the entity");
            }

            return BadRequest("Unable to update the entity");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, int courseId, int moduleId)
        {
            try
            {
                var exists = await _db.AnyAsync<Download>(a => a.Id.Equals(id) && a.ModuleId.Equals(moduleId) && a.CourseId.Equals(courseId));
                if (!exists) return BadRequest("Could not find entity");

                if (await _db.DeleteAsync<Download>(d => d.Id.Equals(id) && d.ModuleId.Equals(moduleId) && d.CourseId.Equals(courseId))) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the entity");
            }

            return BadRequest("Failed to delete the entity");
        }
        #endregion
    }
}
