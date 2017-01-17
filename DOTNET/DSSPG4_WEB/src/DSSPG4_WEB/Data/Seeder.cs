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
                    FirstName = "dsspg4_admin",
                    LastName = "na",
                    UserName = "dsspg4_admin@gmail.com",
                    PasswordHash = "!QAZ1qaz",
                    Email = "dsspg4_admin@gmail.com"
                },
                new User
                {
                    FirstName = "dsspg4_admin_2",
                    LastName = "na",
                    UserName = "dsspg4_admin_2@gmail.com",
                    PasswordHash = "!QAZ1qaz",
                    Email = "dsspg4_admin_2@gmail.com"
                }
            };

            List<Role> roles = new List<Role>
            {
                new Role { Name = "admin",Description = "Full site access" },
                new Role { Name = "user", Description = "Regular user"}
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
            }

            List<Survey> surveys = new List<Survey>
            {
                new Survey { SurveyName = "Test_Survey", NumberQuestions = 20, Creator = (_userService.GetByUserName("dsspg4_admin@gmail.com"))  }
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
                new SurveyQuestion { Question = "Do you like dogs?", ParentSurvey = (_surveyService.GetSurveyByName("Test_Survey")) },
                new SurveyQuestion { Question = "Do you like cats?", ParentSurvey = (_surveyService.GetSurveyByName("Test_Survey")) },
                new SurveyQuestion { Question = "Do you like dogs?", ParentSurvey = (_surveyService.GetSurveyByName("Test_Survey")) }
            };


            _surveyService.AddSurveyQuestions(surveyQuestions);
        }
    }
}
