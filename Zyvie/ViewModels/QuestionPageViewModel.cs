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
    public class QuestionPageViewModel: INotifyPropertyChanged
    {
        public QuizModel Quiz{get;set;}

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand RefreshCommand { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public QuestionPageViewModel()
        {
            AsyncHelpers.RunSync(() => DownloadDataAsync());

           // RefreshCommand = new RelayCommand(async () => await DownloadDataAsync());

            //var task = AsyncContext.RunTask(async () => await DownloadDataAsync());// Task.Run(async () => await DownloadDataAsync());
            //task.Wait();

            //DownloadDataAsync();
        }

        public async Task DownloadDataAsync()
        {
            Quiz = await GetQuizAsync();
        }

        public async Task<QuizModel> GetQuizAsync()
        {
            //var baseUrl = "http://6abcbaa7.ngrok.io/quizzes/general.json";

            var baseUrl = "http://google.com";
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();

            //var responseResult = response.Content.ReadAsStringAsync();
            //var jsonResult = JsonConvert.DeserializeObject(responseResult.Result);

            var jsonResult = "{\"quiz\":{\"id\":59,"
                + "\"questions\":[{\"question\":{\"id\":26,\"title\":\"Хто быў першым каралём Рэчы Паспалітай?\",\"answers\":[{\"answer\":{\"id\":16,\"body\":\"Уладзіслаў IV Ваза\",\"right\":false}},{\"answer\":{\"id\":17,\"body\":\"Стэфан Баторый\",\"right\":true}},{\"answer\":{\"id\":18,\"body\":\"Жыгімонт II Аўгуст\",\"right\":false}},{\"answer\":{\"id\":19,\"body\":\"Казімер III Вялікі\",\"right\":false}}]}},"
                                + "{\"question\":{\"id\":26,\"title\":\"Хто быў першым каралём Рэчы Паспалітай?\",\"answers\":[{\"answer\":{\"id\":16,\"body\":\"Уладзіслаў IV Ваза\",\"right\":false}},{\"answer\":{\"id\":17,\"body\":\"Стэфан Баторый\",\"right\":true}},{\"answer\":{\"id\":18,\"body\":\"Жыгімонт II Аўгуст\",\"right\":false}},{\"answer\":{\"id\":19,\"body\":\"Казімер III Вялікі\",\"right\":false}}]}}]}}";

            //var jsonQuiz = JObject.Parse(responseResult.Result)["quiz"];
            var jsonQuiz = JObject.Parse(jsonResult)["quiz"];

            var quiz = new QuizModel
            {
                Id = Int32.Parse(jsonQuiz["id"].ToString()),
                QuestionsList = new List<Question>()
            };

            foreach (JToken jsonQuestion in jsonQuiz["questions"])
            {
                var question = jsonQuestion["question"];

                quiz.QuestionsList.Add(
                    new Question{
                        Id = Int32.Parse(question["id"].ToString()),
                        Title = question["title"].ToString(),
                        AnswersList = new List<Answer>(),
                        IsAnswered = false
                    });

                foreach (JToken jsonAnswer in question["answers"])
                {
                    var answer = jsonAnswer["answer"];

                    quiz.QuestionsList.Last().AnswersList.Add(
                        new Answer
                        {
                            Id = Int32.Parse(answer["id"].ToString()),
                            Body = answer["body"].ToString(),
                            IsRight = Boolean.Parse(answer["right"].ToString())
                        });
                }
            }

            quiz.CurrentQuestion = quiz.QuestionsList.FirstOrDefault();
            return quiz;
        }
    }

    public class QuizModel
    {
        public int Id { get; set; }

        public Question CurrentQuestion { get; set; }

        public List<Question> QuestionsList { get; set; }
    }
}
