using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabiryntMatematyczny.Models
{
    public class MathQuestion
    {
        public string QuestionText { get; private set; }
        public int CorrectAnswer { get; private set; }
        public MathQuestion(string text, int answer)
        {
            QuestionText = text;
            CorrectAnswer = answer;
        }
    }
    
}
