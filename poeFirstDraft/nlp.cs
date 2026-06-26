using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace POE_FirstDraft
{
    internal class NLP
    {   // start of class
        // I created this dictionary to map different ways a user might say something
        // to a single intent my bot can understand
        // for example "remind me" and "set a reminder" both mean the same thing
        private static Dictionary<string, string> intentMap = new Dictionary<string, string>
        {
          // Task related (longer phrases first)
            { "add a task to", "add_task" },
            { "add task to", "add_task" },
            { "create a task to", "add_task" },
            { "add a task", "add_task" },
            { "add task", "add_task" },
            { "new task", "add_task" },
            { "create a task", "add_task" },
            { "i want to add", "add_task" },

            // Reminder related
            { "remind me to", "set_reminder" },
            { "remind me", "set_reminder" },
            { "set a reminder for", "set_reminder" },
            { "set reminder for", "set_reminder" },
            { "set a reminder", "set_reminder" },
            { "set reminder", "set_reminder" },
            { "reminder for", "set_reminder" },
            { "don't let me forget to", "set_reminder" },
            { "remember to", "set_reminder" },

            // View tasks
            { "view tasks", "view_tasks" },
            { "show tasks", "view_tasks" },
            { "my tasks", "view_tasks" },
            { "what are my tasks", "view_tasks" },
            { "list tasks", "view_tasks" },

            // Summary
            { "what have you done for me", "activity_log" },
            { "what have you done", "activity_log" },
            { "recent actions", "activity_log" },
            { "show activity", "activity_log" },

            // Quiz
            { "start quiz", "quiz" },
            { "quiz me", "quiz" },
        };

        public static (string intent, string content) DetectIntent(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return ("unknown", "");

            string lower = input.ToLower().Trim();

            // Greetings
            if (lower.Contains("how are you") || lower.Contains("hello") ||
                lower.Contains("hi") || lower.Contains("hey"))
            {
                return ("greeting", "");
            }

            if (lower.Contains("show activity log") || lower.Contains("activity log") ||
                lower.Contains("show activity") || lower.Contains("recent actions") ||
                lower.Contains("what have you done") || lower.Contains("show log") ||
                lower.Contains("what did you do"))
              {
                return ("activity_log", "");
              }


            if (lower.Contains("add a reminder") || lower.Contains("add reminder") ||
                lower.Contains("set a reminder") || lower.Contains("set reminder") ||
                lower.Contains("remind me") || lower.Contains("reminder for") ||
                lower.Contains("don't let me forget") || lower.Contains("remember to"))
            {
                string content = ExtractContent(lower, new[] {
            "add a reminder to", "add reminder to", "add a reminder", "add reminder",
            "set a reminder for", "set reminder for", "remind me to", "remind me",
            "reminder for"
        });
                return ("set_reminder", content);
            }

            // Task detection
            if (lower.Contains("add a task") || lower.Contains("add task") ||
                lower.Contains("new task") || lower.Contains("create a task"))
            {
                string content = ExtractContent(lower, new[] {
            "add a task to", "add task to", "add a task", "add task"
        });
                return ("add_task", content);
            }

            // View tasks, quiz, summary...
            if (lower.Contains("view tasks") || lower.Contains("show tasks") ||
                lower.Contains("my tasks") || lower.Contains("list tasks"))
                return ("view_tasks", "");

            if (lower.Contains("what have you done") || lower.Contains("summary") ||
                lower.Contains("recent actions"))
                return ("activity_log", "");

            if (lower.Contains("start quiz") || lower == "quiz")
                return ("quiz", "");

            return ("unknown", "");
        }// end of DetectIntent


        private static string ExtractContent(string text, string[] triggers)
        {
            foreach (string trigger in triggers.OrderByDescending(t => t.Length))
            {
                if (text.Contains(trigger))
                {
                    int index = text.IndexOf(trigger);
                    string content = text.Substring(index + trigger.Length).Trim();
                    if (!string.IsNullOrEmpty(content))
                        return content;
                }
            }
            return text.Trim();
        }

        public static string Normalise(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            return input.ToLower()
                .Replace("?", "").Replace("!", "").Replace(".", "")
                .Replace(",", "").Replace("'", "").Replace("\"", "")
                .Trim();
        }

        // I added synonyms so the bot can understand related words
        // for example if someone types "virus" the bot treats it like "malware"
        // this makes the bot feel smarter without needing real AI
        public static string ResolveSynonym(string word)
        {// start of ResolveSynonym

                var synonyms = new Dictionary<string, string>
            {
                // password
                { "passwords", "password" },
                { "passcode", "password" },
                { "credentials", "password" },
                { "pin", "password" },

                // phishing
                { "phising", "phishing" },
                { "phish", "phishing" },
                { "scam", "phishing" },
                { "fake email", "phishing" },

                // malware
                { "virus", "malware" },
                { "trojan", "malware" },
                { "spyware", "malware" },
                { "worm", "malware" },

                // hacked
                { "hack", "hacked" },
                { "hacking", "hacked" },
                { "breach", "databreach" },
                { "breached", "databreach" },
                { "leaked", "databreach" },

                // two factor
                { "2fa", "twofactor" },
                { "two factor", "twofactor" },
                { "authenticator", "twofactor" },

                // safe browsing
                { "browsing", "safebrowsing" },
                { "browser", "safebrowsing" },
                { "https", "safebrowsing" },
                { "website", "safebrowsing" },

                // greetings
                { "hello", "greeting" },
                { "hi", "greeting" },
                { "hey", "greeting" },

                // help
                { "help", "purpose" },
                { "assist", "purpose" },
            };

            string lower = word.ToLower();
            return synonyms.ContainsKey(lower) ? synonyms[lower] : word;
        }// end of ResolveSynonym

    }
}