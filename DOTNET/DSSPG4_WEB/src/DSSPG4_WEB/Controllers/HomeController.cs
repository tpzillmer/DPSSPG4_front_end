using DSSPG4_WEB.Models.Entities;
using DSSPG4_WEB.Models.HomeViewModels;
using DSSPG4_WEB.Services.SurveyServices;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace DSSPG4_WEB.Controllers
{
    public class HomeController : Controller
    {
        private SurveyService _surveyService;

        public HomeController(SurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        public IActionResult Index()
        {
            IEnumerable<Survey> surveyList = _surveyService.GetAllOpenSurveys();
            HomeIndexViewModel model = new HomeIndexViewModel();
            model.Surveys = surveyList;
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Tautua - Surveys made easy!";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
