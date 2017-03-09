using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSSPG4_WEB.Services.RoleServices;
using DSSPG4_WEB.Services.UserServices;
using DSSPG4_WEB.Models.Entities;
using DSSPG4_WEB.Services.SurveyServices;

namespace DSSPG4_WEB.Data
{
    public class Seeder
    {
        private RoleService _roleService;
        private UserService _userService;
        private SurveyService _surveyService;

        public Seeder(RoleService roleService,UserService userService, SurveyService surveyService)
        {
            _roleService = roleService;
            _userService = userService;
            _surveyService = surveyService;
        }
        public async Task ExecuteSeed()
        {
            List<User> users = new List<User>
            {
                new User
                {
                    FirstName = "admin",
                    LastName = "na",
                    UserName = "admin@gmail.com",
                    PasswordHash = "!QAZ1qaz",
                    Email = "admin@gmail.com"
                },
                new User
                {
                    FirstName = "admin_2",
                    LastName = "na",
                    UserName = "admin_2@gmail.com",
                    PasswordHash = "!QAZ1qaz",
                    Email = "admin_2@gmail.com"
                }
            };

            List<Role> roles = new List<Role>
            {
                new Role { Name = "admin",Description = "Full site access" },
                new Role { Name = "user", Description = "Regular user"},
                new Role { Name = "creator", Description = "Survey Creator"}
            };

            foreach (var r in roles)
            {
                await _roleService.CreateRole(r);
            }

            foreach (var u in users)
            {
                await _userService.RegisterUser(u);
            }

            foreach (var u in users)
            {
                await _userService.AddUserToRole(u, "user");
                await _userService.AddUserToRole(u, "admin");
                await _userService.AddUserToRole(u, "creator");
            }

            List<Survey> surveys = new List<Survey>
            {
                new Survey { SurveyName = "Test_Survey", NumberQuestions = 20,Creator = (_userService.GetByUserName("admin@gmail.com"))  }
            };

            foreach (var s in surveys)
            {
                if (!_surveyService.SurveyExists(s))
                {
                    _surveyService.Add(s);
                }
            }

            List<SurveyQuestion> surveyQuestions = new List<SurveyQuestion>
            {
                new SurveyQuestion { Question = "I Love Dogs", ParentSurvey = (_surveyService.GetSurveyByName("Test_Survey")) },
                new SurveyQuestion { Question = "I Love Cats", ParentSurvey = (_surveyService.GetSurveyByName("Test_Survey")) },
                new SurveyQuestion { Question = "I Love Panda Bears", ParentSurvey = (_surveyService.GetSurveyByName("Test_Survey")) }
            };

            _surveyService.AddSurveyQuestions(surveyQuestions);
        }
    }
}
