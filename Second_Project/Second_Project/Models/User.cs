using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Second_Project.Models
{
    [Table("User")]
    public partial class User
    {
        [Key]
        [StringLength(128)]
        public string UserName { get; set; }
        [Required]
        [StringLength(500)]
        public string Password { get; set; }
        [Required]
        [StringLength(10)]
        public string Role { get; set; }
    }
}
