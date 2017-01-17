using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.Entities
{
    [Table("SurveyQuestions")]
    public class SurveyQuestion
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Display(Name = "Survey")]
        [ForeignKey("SurveyId")]
        [Column("SurveyId")]
        [Required]
        public Survey ParentSurvey { get; set; }

        public int SurveyId { get; set; }

        [Display(Name = "Question")]
        [Required]
        public string Question { get; set; }
    }
}
