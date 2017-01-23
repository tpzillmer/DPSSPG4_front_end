using DSSPG4_WEB.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.HomeViewModels
{
    public class HomeIndexViewModel
    {
       public IEnumerable<Survey> Surveys { get; set; }
    }
}
