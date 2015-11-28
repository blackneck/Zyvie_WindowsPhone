﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zyvie.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Answer> AnswersList { get; set; }

        public bool IsAnswered { get; set; }
    }
}
