using DSSPG4_WEB.Models.Entities;
using DSSPG4_WEB.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.SurvetViewModels
{
    public class SurveyResultsViewModel
    {
        public double ResponsesMarkedImportant { get; set; }
        public double ResponsesMarkedNotImportant { get; set; }
        public Survey Survey { get; set; }
        public SurveyQuestion SurveyQuestion { get; set; }
    }

    public class SurveyResultsByUserViewModel
    {
        public User User { get; set; }
        public Survey Survey { get; set; }
        public IEnumerable<SurveyViewModelResponseData> Responses { get; set; }
    }

    public class SurveyViewModelResponseData
    {
        public string Question { get; set; }
        public ResponseValues ResponseValue { get; set; }
    }

    public class SurveyViewModelResponseDataCSV
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Question { get; set; }
        public ResponseValues ResponseValue { get; set; }
    }
}
