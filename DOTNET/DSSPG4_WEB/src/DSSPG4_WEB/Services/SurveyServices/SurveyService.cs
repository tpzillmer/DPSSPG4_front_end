using DSSPG4_WEB.Data;
using DSSPG4_WEB.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DSSPG4_WEB.Services.SurveyServices
{
    public class SurveyService
    {
        private DBContext _context;

        public SurveyService(DBContext context)
        {
            _context = context;
        }

        public void Add(Survey survey)
        {
            _context.Add(survey);
            Commit();
        }

        public void AddQuestion(SurveyQuestion question)
        {
            _context.Add(question);
            Commit();
        }

        public void Remove(Survey survey)
        {
            _context.Remove(survey);
            Commit();
        }

        public void RemoveQuestion(SurveyQuestion question)
        {
            _context.Remove(question);
            Commit();
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public bool SurveyExists(Survey survey)
        {
            if ((_context.Surveys.FirstOrDefault(s => s.SurveyName == survey.SurveyName) == null))
            {
                return false;
            }
            else return true;
        }

        public bool SurveyQuestionExists(SurveyQuestion question)
        {
            // needs more work....
            var this_question = _context.SurveyQuestions.FirstOrDefault(q => q.ParentSurvey.Id == question.ParentSurvey.Id && q.Question == question.Question);

            if (this_question == null)
            {
                return false;
            }

            else return true;
        }
        public void AddSurveyQuestions(IEnumerable<SurveyQuestion> questions)
        {
            foreach(var q in questions)
            {
                if (!SurveyQuestionExists(q))
                {
                    AddQuestion(q);
                }
            }
        }

        public Survey GetSurveyByName(string name)
        {
            return _context.Surveys.FirstOrDefault(s => s.SurveyName == name);
        }

        public IEnumerable<Survey> GetAllSurveys()
        {
            return _context.Surveys.ToList();
        }
    }
}
