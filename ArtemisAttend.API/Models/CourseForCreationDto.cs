﻿using ArtemisAttend.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtemisAttend.API.Models
{
    public class CourseForCreationDto: CourseForManipulationDto //: IValidatableObject
    {
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "The provided description should be different from the title.",
        //            new[] { "CourseForCreationDto" });
        //    }
        //}
    }
}
