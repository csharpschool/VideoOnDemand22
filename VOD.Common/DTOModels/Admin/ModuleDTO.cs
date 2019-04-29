using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VOD.Common.DTOModels.Admin
{
    public class ModuleDTO
    {
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string Title { get; set; }

        public int CourseId { get; set; }
        public string Course { get; set; }
        public ICollection<VideoDTO> Videos { get; set; }
        public ICollection<DownloadDTO> Downloads { get; set; }

        public ButtonDTO ButtonDTO { get { return new ButtonDTO(CourseId, Id); } }
    }
}
