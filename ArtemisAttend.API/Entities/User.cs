using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtemisAttend.API.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
    }
}
