using DSSPG4_WEB.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DSSPG4_WEB.Models.Entities
{
    [Table("Responses")]
    public class Response
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Display(Name = "Survey Question")]
        [ForeignKey("SurveyQuestionId")]
        [Column("SurveyQuestionId")]
        [Required]
        public SurveyQuestion ParentQuestion { get; set; }

        public int SurveyQuestionId { get; set; }

        [Display(Name = "Taker")]
        [ForeignKey("UserId")]
        [Column("UserId")]
        [Required]
        public User SurveyTaker { get; set; }

        public string UserId { get; set; }

        [Display(Name = "ResponseValue")]
        [Required]
        public ResponseValues QuestionResponse { get; set; }

    }
}
