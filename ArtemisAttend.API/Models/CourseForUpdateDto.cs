using System.ComponentModel.DataAnnotations;

namespace ArtemisAttend.API.Models
{
    public class CourseForUpdateDto : CourseForManipulationDto
    {
        [Required(ErrorMessage = "Campo obligatirio")] 
        public override string Description { get => base.Description ; set => base.Description = value; }
    }
}
