using DSSPG4_WEB.Models.Entities;
using DSSPG4_WEB.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.SurvetViewModels
{
    public class TakeSurveyViewModel
    {
        public List<QResponses> QResponseList { get; set; }
    }

    public class QResponses
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string QuestionValue { get; set; }
        [Required]
        [System.ComponentModel.DefaultValue(ResponseValues.NEUTRAL)]
        public ResponseValues value { get; set; }

    }
}
