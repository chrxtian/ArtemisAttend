using ArtemisAttend.API.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace ArtemisAttend.API.Models
{
    [CourseTitleMustBeDifferentFromDescriptionAttribute(
        ErrorMessage = "Titulo y Descripcion deben ser diferentes")]
    public abstract class CourseForManipulationDto
    {
        [Required(ErrorMessage = "Campo obligatirio")]
        [MaxLength(100, ErrorMessage = "Longitud maxima 100")]
        public string Title { get; set; }
                
        [MaxLength(200, ErrorMessage = "Longitud maxima 200")]
        public virtual string Description { get; set; }
    }
}
