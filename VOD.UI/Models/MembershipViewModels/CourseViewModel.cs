using System.Collections.Generic;
using VOD.Common.DTOModels.UI;

namespace VOD.UI.Models.MembershipViewModels
{
    public class CourseViewModel
    {
        public CourseDTO Course { get; set; }
        public InstructorDTO Instructor { get; set; }
        public IEnumerable<ModuleDTO> Modules { get; set; }
    }
}
