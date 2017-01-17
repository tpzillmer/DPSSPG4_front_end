using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.SurvetViewModels
{
    public class SurveyCreateViewModel
    {
        [Display(Name = "Survey Name")]
        [MaxLength(255)]
        [Required]
        public string SurveyName { get; set; }

        [Display(Name = "Number of Questions")]
        [Range(1, 30, ErrorMessage = "Cannot have more than 30 questions...")]
        [Required]
        public int NumberQuestions { get; set; }
    }
}
