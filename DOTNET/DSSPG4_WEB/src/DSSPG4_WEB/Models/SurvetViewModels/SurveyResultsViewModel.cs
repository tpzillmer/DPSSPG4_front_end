using DSSPG4_WEB.Models.Entities;
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
}
