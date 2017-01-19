using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DSSPG4_WEB.Models.Entities
{
    [Table("Roles")]
    public class Role : IdentityRole
    {
        [MaxLength(255)]
        [Display(Name = "Role Description")]
        public string Description { get; set; }
    }
}
