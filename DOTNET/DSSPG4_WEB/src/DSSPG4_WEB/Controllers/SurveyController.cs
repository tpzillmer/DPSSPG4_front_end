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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DSSPG4_WEB.Controllers
{
    [Authorize(Policy = "RequireCreatorRole")]
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

        [HttpGet]
        public IActionResult Take(int id)
        {
            string UserID = _userManager.GetUserId(User);

            if (_surveyService.UserTookSurvey(id, UserID))
            {
                return View("~/Views/Survey/_SurveyTaken.cshtml");
            }
            else
            {

                TakeSurveyViewModel model = new TakeSurveyViewModel();

                var this_Survey = _surveyService.GetSurveyById(id);
                var questionsList = _surveyService.GetSurveyQuestions(this_Survey.Id);

                List<QResponses> qResponses = new List<QResponses>();
                foreach (var q in questionsList)
                {
                    QResponses r = new QResponses();
                    r.QuestionId = q.Id;
                    r.QuestionValue = q.Question;
                    qResponses.Add(r);
                }

                model.QResponseList = qResponses;
                return View(model.QResponseList);
            }
        }

        [HttpPost]
        public IActionResult Take(int id, IList<QResponses> model)
        {
            if (ModelState.IsValid)
            {
                string UserID = _userManager.GetUserId(User);
                var this_User = _userService.GetById(UserID);
                var this_survey = _surveyService.GetSurveyById(id);

                if (model.Count > 0)
                {
                    foreach (var obj in model)
                    {
                        Response res = new Response();
                        res.ParentQuestion = _surveyService.GetSurveyQuestionById(obj.QuestionId);
                        res.SurveyTaker = this_User;
                        res.QuestionResponse = obj.value;
                        
                        _surveyService.AddSurveyQuestionResponse(res);
                    }
                }
                else
                {
                    return Content("Count: " + model.ToString());
                }
                _surveyService.BumpSurveyTakenCount(this_survey.Id);
                return RedirectToAction("Index","Home");
            }

            return View(model);
        }

        public IActionResult SurveyResults(int id)
        {
            List<SurveyResultsByUserViewModel> model = new List<SurveyResultsByUserViewModel>();
            var allTakers = _surveyService.GetSurveyTakersIds(id);
            var this_Survey = _surveyService.GetSurveyById(id);

            foreach (var takerId in allTakers)
            {
                var this_User = _userService.GetById(takerId);
                bool userAlreadyInList = model.Any(m => m.User.Id == this_User.Id);

                if (userAlreadyInList == false)
                {
                    SurveyResultsByUserViewModel thisUserModel = new SurveyResultsByUserViewModel();
                    var usrResponses = _surveyService.GetSurveyResponsesBySurveyIdAndUserID(id, takerId);
                    thisUserModel.User = this_User;
                    thisUserModel.Survey = this_Survey;
                    List<SurveyViewModelResponseData> rspDataList = new List<SurveyViewModelResponseData>();
                    foreach (var res in usrResponses)
                    {
                        SurveyViewModelResponseData rspData = new SurveyViewModelResponseData();
                        SurveyQuestion question = _surveyService.GetSurveyQuestionById(res.SurveyQuestionId);
                        rspData.Question = question.Question;
                        rspData.ResponseValue = res.QuestionResponse;
                        rspDataList.Add(rspData);
                    }

                    thisUserModel.Responses = rspDataList;
                    model.Add(thisUserModel);
                }

            }

            return View(model);
        }

        [HttpGet]
        [Route("data.csv")]
        [Produces("text/csv")]
        public IActionResult Export(int id)
        {
            List<SurveyViewModelResponseDataCSV> model = new List<SurveyViewModelResponseDataCSV>();
            var allTakers = _surveyService.GetSurveyTakersIds(id);
            var this_Survey = _surveyService.GetSurveyById(id);
            foreach (var takerId in allTakers)
            {
                var this_User = _userService.GetById(takerId);
                SurveyViewModelResponseDataCSV thisUserModel = new SurveyViewModelResponseDataCSV();
                var usrResponses = _surveyService.GetSurveyResponsesBySurveyIdAndUserID(id, takerId);
                thisUserModel.FirstName = this_User.FirstName;
                thisUserModel.LastName = this_User.LastName;

                foreach (var res in usrResponses)
                {
                    SurveyQuestion question = _surveyService.GetSurveyQuestionById(res.SurveyQuestionId);
                    thisUserModel.Question = question.Question;
                    thisUserModel.ResponseValue = res.QuestionResponse;
                }

                model.Add(thisUserModel);
            }

            return Ok(model);
        }

        /*public IActionResult SurveyResults(int id)
        {
            List<SurveyResultsViewModel> model = new List<SurveyResultsViewModel>();
            var responseList = _surveyService.GetSurveyResponsesBySurveyId(id);
            var allQuestions = _surveyService.GetSurveyQuestions(id);

            foreach(var q in allQuestions)
            {
                var importantCount = responseList.Where(r => r.SurveyQuestionId == q.Id && r.QuestionResponse == ResponseValues.IMPORTANT).Count();
                var notimportantCount = responseList.Where(r => r.SurveyQuestionId == q.Id && r.QuestionResponse == ResponseValues.NOTIMPORTANT).Count();
                SurveyResultsViewModel obj = new SurveyResultsViewModel();

                obj.ResponsesMarkedImportant = importantCount;
                obj.ResponsesMarkedNotImportant = notimportantCount;
                obj.Survey = _surveyService.GetSurveyById(id);
                obj.SurveyQuestion = q;

                model.Add(obj);
            }


            return View(model);
        } */


        private Task<User> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }
    }
}
