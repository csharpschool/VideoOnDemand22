using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VOD.Common.Entities
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string Title { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
        public List<Video> Videos { get; set; }
        public List<Download> Downloads { get; set; }
    }
}
