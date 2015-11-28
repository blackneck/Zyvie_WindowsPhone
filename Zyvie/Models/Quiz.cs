using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zyvie.Models
{
    public class QuizModel
    {
        public int Id { get; set; }

        public List<Question> QuestionsList { get; set; }
    }
}
