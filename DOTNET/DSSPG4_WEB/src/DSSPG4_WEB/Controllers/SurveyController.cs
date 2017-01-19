using DSSPG4_WEB.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DSSPG4_WEB.Models.SurvetViewModels;
using DSSPG4_WEB.Services.SurveyServices;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using DSSPG4_WEB.Services.UserServices;
using System.Linq;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DSSPG4_WEB.Controllers
{
    public class SurveyController : Controller
    {
        private readonly UserManager<User> _userManager;
        private SurveyService _surveyService;
        private UserService _userService;

        public SurveyController(UserManager<User> userManager, SurveyService surveyService, UserService userService)
        {
            _userManager = userManager;
            _surveyService = surveyService;
            _userService = userService;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            string UserID = _userManager.GetUserId(User);
            User this_User = _userService.GetById(UserID);

            SurveyIndexViewModel surveys = new SurveyIndexViewModel();

            IEnumerable<Survey> allSurveys = _surveyService.GetAllSurveys();
            surveys.surveys = allSurveys.Where(s => s.CreatorId == this_User.Id);

            return View(surveys);
        }

        [HttpGet]
        public IActionResult Create()
        {
            SurveyCreateViewModel model = new SurveyCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(SurveyCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string UserID = _userManager.GetUserId(User);
                var this_User = _userService.GetById(UserID);
                Survey newSurvey = new Survey();

                newSurvey.Creator = this_User;
                newSurvey.SurveyName = model.SurveyName;
                newSurvey.NumberQuestions = model.NumberQuestions;

                if (!_surveyService.SurveyExists(newSurvey))
                {
                    _surveyService.Add(newSurvey);
                    return RedirectToAction("index");
                }
                else
                {
                    return Content("Survey Already exists...");
                }
            }
            return Content(ModelState.Values.ToString());
        }

    }
}
