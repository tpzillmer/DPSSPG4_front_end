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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
                Survey newSurvey = new Survey()
                {
                    Creator = this_User,
                    SurveyName = model.SurveyName,
                    NumberQuestions = model.NumberQuestions,
                    SampleSize = model.SampleSize,
                    Status = model.Status.Value
                };

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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Survey this_survey = _surveyService.GetSurveyById(id);
            SurveyCreateViewModel model = new SurveyCreateViewModel() {
                SurveyName = this_survey.SurveyName,
                NumberQuestions = this_survey.NumberQuestions,
                SampleSize = this_survey.SampleSize,
                Status = this_survey.Status
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, SurveyCreateViewModel model)
        {
            var this_survey = _surveyService.GetSurveyById(id);
            if (ModelState.IsValid && this_survey != null)
            {
                this_survey.NumberQuestions = model.NumberQuestions;
                this_survey.SurveyName = model.SurveyName;
                this_survey.Status = model.Status.Value;
                this_survey.SampleSize = model.SampleSize;
                var result = _surveyService.Commit();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Questions(int id)
        {
            AddQuestionViewModel model = new AddQuestionViewModel();
            model.Survey = _surveyService.GetSurveyById(id);
            model.Questions = _surveyService.GetSurveyQuestions(id);
            return View(model);
        }

        [HttpGet]
        public IActionResult AddQuestion(int id)
        {
            NewQuestionViewModel model = new NewQuestionViewModel();
            model.Survey = _surveyService.GetSurveyById(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult AddQuestion(int id,NewQuestionViewModel model)
        {
            SurveyQuestion question = new SurveyQuestion();

            question.Question = model.Question;
            question.ParentSurvey = _surveyService.GetSurveyById(id);
            if (ModelState.IsValid)
            {
                _surveyService.AddQuestion(question);
                return RedirectToAction("Questions", new { id = question.ParentSurvey.Id });
            }

            return View(model);
        }
    }
}
