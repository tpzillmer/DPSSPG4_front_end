using DSSPG4_WEB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.SurvetViewModels
{
    public class SurveyIndexViewModel
    {
        public IEnumerable<Survey> surveys { get; set; }
    }
}
