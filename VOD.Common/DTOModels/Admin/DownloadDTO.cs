using System.ComponentModel.DataAnnotations;

namespace VOD.Common.DTOModels.Admin
{
    public class DownloadDTO
    {
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string Title { get; set; }
        [MaxLength(1024)]
        public string Url { get; set; }

        // Side-step from 3rd normal form for easier
        // access to a video’s course and Module
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
        public string Course { get; set; }
        public string Module { get; set; }

        public ButtonDTO ButtonDTO { get { return new ButtonDTO(CourseId, ModuleId, Id); } }
    }
}
