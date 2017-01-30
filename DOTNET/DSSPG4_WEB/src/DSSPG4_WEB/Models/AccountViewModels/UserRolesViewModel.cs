using DSSPG4_WEB.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.AccountViewModels
{
    public class RolesByUserViewModel
    {
        public User User { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public string RoleSelected { get; set; }
    }
}
