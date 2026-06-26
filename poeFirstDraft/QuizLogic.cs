using System;
using System.Collections.Generic;
using System.Text;

namespace poeFirstDraft
{
    internal class Questions
    {
        // start of class

        // the actual question text shown to the user
        public string Question { get; set; }

        // the answer options — could be A/B/C/D or True/False
        public string[] Options { get; set; }

        // the correct answer the user must match
        public string CorrectAnswer { get; set; }

        // shown after the user answers — teaches them something
        public string Explanation { get; set; }

        // constructor so we can build a question in one line
        public Questions(string question, string[] options, string correctAnswer, string explanation)
        {
            Question = question;
            Options = options;
            CorrectAnswer = correctAnswer;
            Explanation = explanation;
        }
    }
}
