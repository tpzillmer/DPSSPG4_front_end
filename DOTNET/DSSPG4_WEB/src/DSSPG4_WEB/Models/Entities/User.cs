using DSSPG4_WEB.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSSPG4_WEB.Models.Entities
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [MaxLength(255)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(255)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Birth { get; set; }
    }

}
