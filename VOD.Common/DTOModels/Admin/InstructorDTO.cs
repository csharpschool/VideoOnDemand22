using System.ComponentModel.DataAnnotations;

namespace VOD.Common.DTOModels.Admin
{
    public class InstructorDTO
    {
        public int Id { get; set; }
        [MaxLength(80), Required]
        public string Name { get; set; }
        [MaxLength(1024)]
        public string Description { get; set; }
        [MaxLength(1024)]
        public string Thumbnail { get; set; }

        public ButtonDTO ButtonDTO { get { return new ButtonDTO(Id); } }
    }
}
