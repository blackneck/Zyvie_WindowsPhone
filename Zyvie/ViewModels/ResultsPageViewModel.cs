using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Input;
using Zyvie.Common;
using Zyvie.Helpers;
using Zyvie.Models;
using Newtonsoft.Json.Linq;

namespace Zyvie.ViewModels
{
    public class ResultsPageViewModel
    {
        public static QuizModel Quiz { get; set; }

        public static string CorrectAnswersRate
        {
            get
            {
                var answeredQuestions = Quiz.QuestionsList.Where(el => el.IsAnswered);
                var correctAnswers = answeredQuestions.Where(el => el.IsCorrectlyAnswered);
                var correctAnswersRate = answeredQuestions.Any() ? (float)correctAnswers.Count() / (float)answeredQuestions.Count() * 100 : 0;

                return String.Format("{0} %", Math.Round(correctAnswersRate, 2));
            }
        }

        public ResultsPageViewModel()
        {

        }
    }
}
