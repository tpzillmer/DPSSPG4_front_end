using DSSPG4_WEB.Data;
using DSSPG4_WEB.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Services.RoleServices
{
   
    public class RoleService
    {
        private DBContext _context;
        private RoleManager<Role> _roleManager;

        public RoleService(DBContext context, RoleManager<Role> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public async Task CreateRole(Role role)
        {
            if (!(await _roleManager.RoleExistsAsync(role.Name)))
            {
                await _roleManager.CreateAsync(role);
            }
        }

        public async Task<bool> RoleExist(string roleName)
        {
           return await _roleManager.RoleExistsAsync(roleName);
        }

    }
}
