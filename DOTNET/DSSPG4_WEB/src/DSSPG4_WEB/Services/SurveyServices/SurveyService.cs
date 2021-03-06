﻿using DSSPG4_WEB.Data;
using DSSPG4_WEB.Models.Entities;
using DSSPG4_WEB.Models.Enums;
using DSSPG4_WEB.Services.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DSSPG4_WEB.Services.SurveyServices
{
    public class SurveyService
    {
        private DBContext _context;
        private UserService _userService;

        public SurveyService(DBContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public void Add(Survey survey)
        {
            _context.Add(survey);
            Commit();
        }

        public void AddQuestion(SurveyQuestion question)
        {
            if (!SurveyQuestionExists(question))
            {
                _context.Add(question);
                Commit();
            }
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

        public void AddSurveyQuestionResponse(Response response)
        {
            _context.Responses.Add(response);
            Commit();
        }

        public void BumpSurveyTakenCount(int id)
        {
            var survey = GetSurveyById(id);
            survey.SurveysTaken += 1;
            Commit();
        }

        public IEnumerable<SurveyQuestion> GetSurveyQuestions(int id)
        {
            return _context.SurveyQuestions.Where(s => s.SurveyId == id);
        }

        public IEnumerable<Response> GetSurveyResponsesBySurveyId(int id)
        {
            return _context.Responses.Where(r => r.ParentQuestion.ParentSurvey.Id == id);
        }

        public IEnumerable<Response> GetSurveyResponsesBySurveyIdAndUserID(int id,string userId)
        {
            return _context.Responses.Where(r => r.ParentQuestion.ParentSurvey.Id == id && r.SurveyTaker.Id == userId);
        }

        public IEnumerable<String> GetSurveyTakersIds (int surveryId)
        {
            IEnumerable<Response> response = GetSurveyResponsesBySurveyId(surveryId);
            List<String> allUsers = new List<String>();

            foreach (var res in response)
            {
                allUsers.Add(res.UserId);
            }

            return allUsers;
        }

        public SurveyQuestion GetSurveyQuestionById(int id)
        {
            return _context.SurveyQuestions.FirstOrDefault(q => q.Id == id);
        }

        public Survey GetSurveyByName(string name)
        {
            return _context.Surveys.FirstOrDefault(s => s.SurveyName == name);
        }

        public Survey GetSurveyById(int id)
        {
            return _context.Surveys.FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Survey> GetAllSurveys()
        {
            return _context.Surveys.ToList();
        }

        public IEnumerable<Survey> GetAllOpenSurveys()
        {
            return _context.Surveys.Where(s => s.Status == SurveyStatus.Open);
        }

        public IEnumerable<Survey> GetAllClosedSurveys()
        {
            return _context.Surveys.Where(s => s.Status == SurveyStatus.Closed);
        }

        public bool UserTookSurvey(int surveyId, string userId)
        {
            var responsesByThisUser = GetSurveyResponsesBySurveyIdAndUserID(surveyId, userId);

            if (responsesByThisUser.Any(s => s.UserId == userId))
            {
                return true;
            }
            else return false;
        }
    }
}
