using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VOD.Common.DTOModels.Admin
{
    public class CourseDTO
    {
        public int Id { get; set; }
        [MaxLength(255)]
        public string ImageUrl { get; set; }
        [MaxLength(255)]
        public string MarqueeImageUrl { get; set; }
        [MaxLength(80), Required]
        public string Title { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }

        public int InstructorId { get; set; }
        public string Instructor { get; set; }
        public ICollection<ModuleDTO> Modules { get; set; }

        public ButtonDTO ButtonDTO { get { return new ButtonDTO(Id); } }
    }
}
