using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VOD.Common.DTOModels.Admin;
using VOD.Common.Entities;
using VOD.Common.Services;

namespace VOD.API.Controllers
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        #region Properties and Variables
        private readonly IAdminService _db;
        private readonly LinkGenerator _linkGenerator;
        #endregion

        #region Constructor
        public InstructorsController(IAdminService db, LinkGenerator
        linkGenerator)
        {
            _db = db;
            _linkGenerator = linkGenerator;
        }
        #endregion

        #region Actions
        [HttpGet()]
        public async Task<ActionResult<List<InstructorDTO>>> Get(bool include = false)
        {
            try
            {
                return await _db.GetAsync<Instructor, InstructorDTO>(include);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        #endregion
    }
}
