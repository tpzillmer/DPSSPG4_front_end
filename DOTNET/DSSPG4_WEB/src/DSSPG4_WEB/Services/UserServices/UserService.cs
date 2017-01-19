using DSSPG4_WEB.Models.Entities;
using Microsoft.AspNetCore.Identity;
using DSSPG4_WEB.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DSSPG4_WEB.Services.UserServices
{
    public class UserService 
    {
        private UserManager<User> _userManager;
        private DBContext _context;
        private RoleManager<Role> _roleManager;
        private string _default_role;

        public UserService(DBContext context,
                          UserManager<User> userManager,
                          RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _default_role = "user";
        }

        public async Task RegisterUser(User user)
        {
            if (_context.Users.FirstOrDefault(usr => usr.UserName == user.UserName) == null)
            {
                var result = await  _userManager.CreateAsync(user, user.PasswordHash);
            }
        }

        public async Task AddUserToRole(User user, string roleName)
        {
            var this_user = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);

            if (!(await _userManager.IsInRoleAsync(this_user, roleName)))
            {
              var result = await _userManager.AddToRoleAsync(this_user, roleName);
            }
        }

        public string GetUserId(User user)
        {
            var this_user = _context.Users.FirstOrDefault(u => u.UserName == user.UserName);
            return this_user.Id;
        }

        public User GetByUserName(string username)
        {
            var this_user = _context.Users.FirstOrDefault(u => u.UserName == username);
            return this_user;
        }

        public User GetById(string id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

    }
}
