using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VOD.Common.Entities
{
    public class Course
    {
        [Key]
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
        public Instructor Instructor { get; set; }
        public List<Module> Modules { get; set; }
    }
}
