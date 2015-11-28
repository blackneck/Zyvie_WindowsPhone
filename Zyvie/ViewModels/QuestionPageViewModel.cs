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
            RefreshCommand = new RelayCommand(async () => await DownloadDataAsync());

            DownloadDataAsync();
        }

        public async Task DownloadDataAsync()
        {
            Quiz = await GetQuizAsync();
        }

        public async Task<QuizModel> GetQuizAsync()
        {
            var baseUrl = "http://6abcbaa7.ngrok.io/quizzes/general.json";
            var httpClient = new HttpClient();

            var response = await httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();

            var jsonResult = response.Content.ReadAsStringAsync();

            //var quiz = await JsonConvert.DeserializeObjectAsync<QuizModel>(jsonResponse);

            var quiz = new QuizModel
            {
                JsonResponse = await jsonResult
                //QuestionsList = questionsList,
                //CurrentQuestion = questionsList.FirstOrDefault()
            };

            return quiz;
        }
    }

    public class QuizModel
    {
        public QuestionModel CurrentQuestion { get; set; }

        public List<QuestionModel> QuestionsList { get; set; }

        public string JsonResponse { get;set; }

    }

    public class QuestionModel
    {
        public List<AnswerModel> AnswersList {get; set;}
    }

    public class AnswerModel
    {

    }
}
