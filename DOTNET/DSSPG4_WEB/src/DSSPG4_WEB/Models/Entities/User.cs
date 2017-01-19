using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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
    }
}
