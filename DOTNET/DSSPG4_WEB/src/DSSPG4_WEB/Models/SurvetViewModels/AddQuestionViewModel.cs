using DSSPG4_WEB.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.SurvetViewModels
{
    public class AddQuestionViewModel
    {
        public AddQuestionViewModel()
        {
            newQuestion = new SurveyQuestion();
        }

        public Survey Survey { get; set; }
        public IEnumerable<SurveyQuestion> Questions { get; set; }

        public SurveyQuestion newQuestion { get; set; }
    }

    public class NewQuestionViewModel
    {
        [Display(Name = "Question")]
        [Required]
        public string Question { get; set; }
        public Survey Survey { get; set; }
    }
}
