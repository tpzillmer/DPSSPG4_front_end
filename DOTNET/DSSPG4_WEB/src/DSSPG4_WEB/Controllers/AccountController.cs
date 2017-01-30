using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DSSPG4_WEB.Models.Entities;
using DSSPG4_WEB.Models.AccountViewModels;
using Microsoft.Extensions.Logging;
using System.Linq;
using DSSPG4_WEB.Services.UserServices;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using DSSPG4_WEB.Services.RoleServices;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DSSPG4_WEB.Controllers
{
 
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger _logger;
        private readonly UserManager<User> _userManager;
        private readonly UserService _userService;
        private readonly RoleService _roleService;
        public AccountController(SignInManager<User> signInManager,
                                 ILoggerFactory loggerFactory,
                                 UserManager<User> userManager,
                                 UserService userService,
                                 RoleService roleService)
        {
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _userManager = userManager;
            _userService = userService;
            _roleService = roleService;
        }

        [Authorize(Policy = "RequireAdministratorRole")]
        public ActionResult Index()
        {
            var all = _userManager.Users.ToList();
            IndexUsersViewModal users = new IndexUsersViewModal();
            users.Users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModels model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {

                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new User {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender.Value,
                    UserName = model.Email, 
                    Email = model.Email,
                    Birth = model.Birth
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    await _userService.AddUserToRole(user, "user");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }

                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> AddUserToRole(string Id)
        {
            RolesByUserViewModel model = new RolesByUserViewModel();
            User user = await _userManager.FindByIdAsync(Id);
            model.Roles = _roleService.GetRolesSelectList(); ;
            model.User = user;
            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> AddUserToRole(string Id, RolesByUserViewModel model)
        {
            User this_user = await _userManager.FindByIdAsync(Id);
            await _userManager.AddToRoleAsync(this_user, model.RoleSelected);
            return RedirectToAction("Index", "Account");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
