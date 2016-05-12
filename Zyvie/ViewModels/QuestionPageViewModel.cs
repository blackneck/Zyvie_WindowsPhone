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
        //private static string _getUrl = "http://733160ee.ngrok.io/quizzes/general.json";
        //private static string _saveUrl = "http://733160ee.ngrok.io/quizzes/result";

        #region [Properties, Variables]

        public QuizModel Quiz { get; set; }

        private Question _currentQuestion;

        public Question CurrentQuestion
        {
            get { return _currentQuestion; }
            set
            {
                _currentQuestion = value;
                OnPropertyChanged();
            }
        }

        public ICommand ChangeCurrentQuestionCommand { get; set; }

        #endregion [Properties, Variables]

        #region [Events]

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion [Events]

        public QuestionPageViewModel()
        {
            ChangeCurrentQuestionCommand = new RelayCommand(ChangeCurrentQuestion);
            //AsyncHelpers.RunSync(() => DownloadDataAsync());
            DownloadDataAsync();
        }

        public void UpdateCurrentQuestion(bool isRight)
        {
            var currentIndex = Quiz.QuestionsList.IndexOf(CurrentQuestion);

            var currentQuestion = Quiz.QuestionsList[currentIndex];

            currentQuestion.IsAnswered = true;
            currentQuestion.IsCorrectlyAnswered = isRight;
        }

        public void ChangeCurrentQuestion()
        {
            var currentIndex = Quiz.QuestionsList.IndexOf(CurrentQuestion);

            if (AreAllQuestionsAnswered())
            {
                CurrentQuestion = null;
                OnPropertyChanged();
                return;
            }

            var questionsCount = Quiz.QuestionsList.Count();

            var lastNotAnswered = Quiz.QuestionsList.Last(el => !el.IsAnswered);
            var indexOfLastNotAnswered = Quiz.QuestionsList.IndexOf(lastNotAnswered);

            if (currentIndex < indexOfLastNotAnswered)
            {
                CurrentQuestion = Quiz.QuestionsList.Where(el => !el.IsAnswered && Quiz.QuestionsList.IndexOf(el) > currentIndex).First();
                OnPropertyChanged();
                return;
            }

            var firstNotAnswered = Quiz.QuestionsList.First(el => !el.IsAnswered);
            var indexOfFirstNotAnswered = Quiz.QuestionsList.IndexOf(firstNotAnswered);

            if (currentIndex > indexOfFirstNotAnswered)
            {
                CurrentQuestion = Quiz.QuestionsList.Where(el => !el.IsAnswered).First();
                OnPropertyChanged();
                return;
            }
        }

        public bool AreAllQuestionsAnswered()
        {
            return Quiz.QuestionsList.All(el => el.IsAnswered);
        }

        public void DownloadDataAsync()
        {
            Quiz = GetQuizAsync();
        }

        public QuizModel GetQuizAsync()
        {
            //var httpClient = new HttpClient();

            //var response = await httpClient.GetAsync(_getUrl);
            //response.EnsureSuccessStatusCode();

            //var responseResult = response.Content.ReadAsStringAsync();
            //var jsonResult = JsonConvert.DeserializeObject(responseResult.Result);

            //var jsonQuiz = JObject.Parse(responseResult.Result)["quiz"];

            var jsonResult = "{\"quiz\":{\"id\":161,\"questions\":[{\"id\":27,\"title\":\"Гэта адзін з самых старых запаведных лясных масіваў Яўропы, які знаходзіцца на мяжы Беларусі і Польшчы\",\"img\":{\"url\":null},\"answers\":[{\"id\":24,\"body\":\"Белавежская пушча\",\"right\":true},{\"id\":25,\"body\":\"Бярэзінскі біасферны запаведнік\",\"right\":false},{\"id\":26,\"body\":\"Нацыянальны парк Браслаўскія азёры\",\"right\":false},{\"id\":27,\"body\":\"Палескі дзяржаўны радыяцыйна-экалагічны запаведнік\",\"right\":false}]},{\"id\":24,\"title\":\"У якім годзе адбылася бітва пад Воршай - найбуйнейшая бітва вайны Маскойскай дзяржавы з Вялікім Княствам Літоўскім?\",\"img\":{\"url\":null},\"answers\":[{\"id\":11,\"body\":\"1896\",\"right\":false},{\"id\":12,\"body\":\"1514\",\"right\":true},{\"id\":13,\"body\":\"1663\",\"right\":false},{\"id\":14,\"body\":\"1794\",\"right\":false}]},{\"id\":28,\"title\":\"Нацыянальны герой Беларусі, адзін з кіраўнікоў нацыянальна-вызвольнага паўстаньня 1863—1864 гадоў?\",\"img\":{\"url\":null},\"answers\":[{\"id\":28,\"body\":\"Тадэвуш Касцюшка\",\"right\":false},{\"id\":29,\"body\":\"Кастусь Каліноўскі\",\"right\":true},{\"id\":30,\"body\":\"Ігнат Дамейка\",\"right\":false},{\"id\":31,\"body\":\"Якуб Наркевіч-Ёдка\",\"right\":false}]},{\"id\":29,\"title\":\"Беларускі мастак, адзін з заснавальнікаў супрэматызма, імкнуўся сумясціць прынцыпы кубізму і футурызму. Cамая дарагая карціна ў гісторыі («Супрэматычная кампазіцыя») аўтарства мастака, звязанага з Беларуссю, была напісаная ім.\",\"img\":{\"url\":null},\"answers\":[{\"id\":32,\"body\":\"Казімір Малевіч\",\"right\":true},{\"id\":33,\"body\":\"Марк Шагал\",\"right\":false},{\"id\":34,\"body\":\"Ілля Рэпін\",\"right\":false},{\"id\":35,\"body\":\"Язэп Драздовіч\",\"right\":false}]},{\"id\":33,\"title\":\"Груша цвiла апошнi год. Усе галiны яе, усе вялiкiя расохi, да апошняга пруцiка, былi ўсыпаны бурным бела-ружовым цветам. Яна кiпела, млела i раскашавалася ў пчаліным звоне, цягнула да сонца сталыя лапы i распасцiрала ў яго ззяннi маленькiя, кволыя пальцы новых парасткаў. I была яна такая магутная i свежая, так утрапёна спрачалiся ў яе ружовым раi пчолы, што, здавалася, не будзе ёй зводу i не будзе канца. I, аднак, надыходзiла яе апошняя часiна. Які твор пачынаецца з гэтых строк?\",\"img\":{\"url\":null},\"answers\":[{\"id\":58,\"body\":\"Раскіданае гняздо Я.Купала\",\"right\":false},{\"id\":59,\"body\":\"Каласы пад сярпом тваім У.Караткевіч\",\"right\":true},{\"id\":60,\"body\":\"Новая зямля Я.Колас\",\"right\":false},{\"id\":61,\"body\":\"Людзі на балоце І.Мележ\",\"right\":false}]},{\"id\":31,\"title\":\"Адна з найбольш славутых беларускіх страў. Гатуюць зазвычай з разнастайных мясных абрэзак, дадаюць тук, муку і ваду, а таксама цыбулю, грыбы і кроп.\",\"img\":{\"url\":null},\"answers\":[{\"id\":42,\"body\":\"Камы\",\"right\":false},{\"id\":43,\"body\":\"Смажанік\",\"right\":false},{\"id\":44,\"body\":\"Дранікі\",\"right\":true},{\"id\":45,\"body\":\"Мачанка\",\"right\":false}]}]}}";

            var jsonQuiz = JObject.Parse(jsonResult)["quiz"];

            var quiz = new QuizModel
            {
                Id = Int32.Parse(jsonQuiz["id"].ToString()),
                QuestionsList = new List<Question>()
            };

            foreach (JToken jsonQuestion in jsonQuiz["questions"])
            {
                quiz.QuestionsList.Add(
                    new Question{
                        Id = Int32.Parse(jsonQuestion["id"].ToString()),
                        Title = jsonQuestion["title"].ToString(),
                        AnswersList = new List<Answer>()
                    });

                foreach (JToken jsonAnswer in jsonQuestion["answers"])
                {
                    quiz.QuestionsList.Last().AnswersList.Add(
                        new Answer
                        {
                            Id = Int32.Parse(jsonAnswer["id"].ToString()),
                            Body = jsonAnswer["body"].ToString(),
                            IsRight = Boolean.Parse(jsonAnswer["right"].ToString())
                        });
                }
            }

            CurrentQuestion = quiz.QuestionsList.FirstOrDefault();
            return quiz;
        }

        public void FinishQuiz()
        {
            CopyQuizToResults();
            SaveResults();
        }

        private void CopyQuizToResults()
        {
            ResultsPageViewModel.Quiz = Quiz;
        }

        private void SaveResults()
        {
            var httpClient = new HttpClient();

            HttpRequestMessage httpRequest = new HttpRequestMessage();
            httpRequest.Method = HttpMethod.Post;
            //httpRequest.RequestUri = new Uri(_saveUrl);

            var objToSave = new
            {
                id = Quiz.Id,
                answers = Quiz.QuestionsList.Select(el => new { id = el.Id, isCorrect = el.IsCorrectlyAnswered })
            };

            httpRequest.Content = new StringContent(JsonConvert.SerializeObject(objToSave));
            httpClient.SendAsync(httpRequest);
        }
    }
}
