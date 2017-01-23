using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.Enums
{
    public enum SurveyStatus
    {
        [Display(Name = "In Development")]
        InDevelopment = 0,
        [Display(Name = "Open")]
        Open = 1,
        [Display(Name = "Closed")]
        Closed = 2
    }
}
