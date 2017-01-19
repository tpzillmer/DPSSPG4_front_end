using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DSSPG4_WEB.Models.Entities
{
    [Table("Surveys")]
    public class Survey
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Display(Name = "Creator")]
        [ForeignKey("CreatorId")]
        [Column("CreatorId")]
        public User Creator { get; set; }

        public string CreatorId { get; set; }

        [Display(Name = "Survey Name")]
        [MaxLength(255)]
        public string SurveyName { get; set; }

        [Display(Name = "Number of Questions")]
        [Range(1, 30, ErrorMessage = "Cannot have more than 30 questions...")]
        public int NumberQuestions { get; set; }
    }
}
