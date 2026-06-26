using poeFirstDraft;
using System.Collections;
using System.IO;
using System.Media;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace POE_FirstDraft
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // creating instances
        ArrayList reply = new ArrayList();
        ArrayList ignore = new ArrayList();
        ArrayList add_answers = new ArrayList();


        // storing the username
        string userName = string.Empty;
        // this is to handle follow up questions
        string lastTopic = string.Empty;
        // this will store what the user is interested in
        string userInterests = string.Empty;

        // tracks whether the quiz is currently running
        bool quizActive = false;

        // keeps track of which question we are on
        int currentQuestionIndex = 0;

        // counts how many the user got right
        int quizScore = 0;

        // holds all the quiz questions
        List<Questions> quizQuestions = new List<Questions>();

        public MainWindow()
        {
            InitializeComponent();
            // Methods.ShowAsciiArt();

            //  Response class handles loading the reply and ignore lists
            new Response(reply, ignore);

            PlayGreeting();
        }

        // plays the welcome audio on startup
        private void PlayGreeting()
        {
            try
            {
                string audioPath = System.IO.Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "Cyber Bot.wav");
                SoundPlayer player = new SoundPlayer(audioPath);
                player.Play();
            }
            catch (Exception e)
            {
                MessageBox.Show($"There was a problem playing the audio: {e.Message}");
            }
        }
        // the NLP detects the intent and sends it here
        private void handle_add_task(string input)
        {// start of handle_add_task

            // I try to extract the task name from the input
            // I look for common phrases and remove them to get just the task
            string title = input
                .ToLower()
                .Replace("add a task to", "")
                .Replace("add a task", "")
                .Replace("add task to", "")
                .Replace("add task", "")
                .Replace("create a task to", "")
                .Replace("create a task", "")
                .Replace("new task to", "")
                .Replace("new task", "")
                .Replace("i want to add", "")
                .Trim();

            // I make sure there is actually a title left after cleaning
            if (string.IsNullOrWhiteSpace(title))
                title = "New Cybersecurity Task";

            // I capitalise the first letter to make it look neat
            title = char.ToUpper(title[0]) + title.Substring(1);

            DatabaseHelper.AddTask(userName, title,
                "Remember to: " + title + " — this will help keep your accounts secure.", "");

            error_method("ChatBot", "Task added: '" + title + "'. Would you like a reminder? If yes, type: remind me in X days.");

            ActivityLog.Log(userName, "Added task: " + title);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }// end of handle_add_task

        // I handle reminders here — the NLP sends any reminder phrase to this method
        private void handle_set_reminder(string input)
        {// start of handle_set_reminder

            // I extract the reminder detail from the input
            string reminder = input
                .ToLower()
                .Replace("remind me to", "")
                .Replace("remind me in", "")
                .Replace("set a reminder for", "")
                .Replace("set reminder for", "")
                .Replace("reminder for", "")
                .Replace("don't let me forget to", "")
                .Replace("remember to", "")
                .Trim();

            if (string.IsNullOrWhiteSpace(reminder))
                reminder = "your task";

            error_method("ChatBot", "Got it! Reminder set for '" + reminder + "'.");

            ActivityLog.Log(userName, "Set reminder: " + reminder);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }// end of handle_set_reminder

        // I show all the user's tasks from the database here
        private void handle_view_tasks()
        {// start of handle_view_tasks

            var tasks = DatabaseHelper.GetTasks(userName);

            if (tasks.Count == 0)
            {
                error_method("ChatBot", "You have no tasks yet. Try saying 'add a task to enable 2FA'.");
            }
            else
            {
                string taskList = "Here are your current tasks:\n\n";
                int count = 1;

                foreach (string[] task in tasks)
                {
                    string status = task[4] == "True" ? "✔ Done" : "⏳ Pending";
                    taskList += count + ". " + task[1] + " — " + status + "\n";

                    if (!string.IsNullOrEmpty(task[3]))
                        taskList += "   Reminder: " + task[3] + "\n";

                    count++;
                }

                error_method("ChatBot", taskList);
            }

            ActivityLog.Log(userName, "Viewed tasks");
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }// end of handle_view_tasks

        // I start the quiz from here — this keeps the Send method clean
        private void handle_quiz_start()
        {// start of handle_quiz_start

            quizQuestions = Quiz.GetQuestions();
            currentQuestionIndex = 0;
            quizScore = 0;
            quizActive = true;

            error_method("ChatBot", "Welcome to the Cybersecurity Quiz! Answer with the letter of your choice (a, b, c or d).\n");
            show_question();

            ActivityLog.Log(userName, "Started quiz");

        }// end of handle_quiz_start

        // I show a summary of recent actions from the activity log
        // this satisfies the brief requirement of "what have you done for me"
        private void handle_activity_log()
        {// start of handle_activity_log
            var recentLogs = ActivityLog.GetRecentLogs(8);   // Getting the last 8 actions

            if (recentLogs.Count == 0)
            {
                error_method("ChatBot", "No activity recorded yet. Start using the bot to see logs here!");
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
                return;
            }

            string summary = "Here's a summary of recent actions:\n\n";
            int count = 1;

            foreach (string log in recentLogs)
            {
                summary += $"{count}. {log}\n";
                count++;
            }

            error_method("ChatBot", summary);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }// end of handle_activity_log
        // displays the current question and its options to the user
        private void show_question()
        {// start of show_question

            // safety check — make sure we havent run out of questions
            if (currentQuestionIndex >= quizQuestions.Count)
            {
                end_quiz();
                return;
            }

            Questions current = quizQuestions[currentQuestionIndex];

            // build the question display with all options underneath
            string display = $"Question {currentQuestionIndex + 1} of {quizQuestions.Count}:\n";
            display += current.Question + "\n\n";

            foreach (string option in current.Options)
            {
                display += option + "\n";
            }

            error_method("ChatBot", display);
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }// end of show_question

        // checks if the user's answer matches the correct one
        private void check_answer(string userAnswer)
        {// start of check_answer


            string answer = userAnswer.ToLower().Trim();

            if (answer != "a" && answer != "b" && answer != "c" && answer != "d")
            {
                error_method("ChatBot", "Please answer with only the letter a, b, c, or d.");
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
                return;
            }

            Questions current = quizQuestions[currentQuestionIndex];

            if (userAnswer == current.CorrectAnswer)
            {
                // correct — give positive feedback and the explanation
                quizScore++;
                error_method("ChatBot", "✔ Correct! " + current.Explanation);
            }
            else
            {
                // wrong — still show explanation so they learn something
                error_method("ChatBot", "✘ Not quite. The correct answer was: " +
                    current.CorrectAnswer.ToUpper() + ". " + current.Explanation);
            }

            // move to the next question
            currentQuestionIndex++;

            // small gap then show next question or end the quiz
            if (currentQuestionIndex < quizQuestions.Count)
            {
                show_question();
            }
            else
            {
                end_quiz();
            }
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }// end of check_answer

        // called when all questions are done — shows the final score
        private void end_quiz()
        {// start of end_quiz

            quizActive = false;

            // calculate percentage so we can give appropriate feedback
            double percentage = (double)quizScore / quizQuestions.Count * 100;

            string result = $"Quiz complete! You scored {quizScore} out of {quizQuestions.Count}.\n\n";

            // give feedback based on how well they did
            if (percentage == 100)
                result += "Perfect score! You are a cybersecurity pro!";
            else if (percentage >= 70)
                result += "Great job! You have a solid understanding of cybersecurity.";
            else if (percentage >= 50)
                result += "Not bad! Keep learning to stay safe online.";
            else
                result += "Keep learning! Cybersecurity knowledge keeps you protected online.";

            error_method("ChatBot", result);

            // log the quiz result for the activity log
            ActivityLog.Log(userName, $"Completed quiz — scored {quizScore}/{quizQuestions.Count}");
            chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);

        }// end of end_quiz

        // fires when the user clicks Submit Username
        private void submit_name(object sender, RoutedEventArgs e)
        {// start of submit_name method

            // temp variables
            string filename = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "user_names.txt");

            // auto create the file if it doesn't exist
            if (!File.Exists(filename))
            {
                File.AppendAllText(filename, "auto_create\n");
            }

            // get the name from the input
            string name = user_name.Text.Trim();

            // make sure the user actually typed something
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Please enter a valid name.");
                return;
            } // this is to make sure the user doesnt type a name lower than two characters long 
            if (name.Length < 2)
            {
                MessageBox.Show("Your name shoud be longer than two characters long");
                return;
            }// making sure the sure the users name ony contains alphabets, numbers and symbols are not allowed
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                MessageBox.Show("Name should only conatain alphabets no numbers or symbols");
                return;
            }

            // check if this user has visited before
            bool found = check_name(name);

            if (!found)
            {
                // save the new user
                File.AppendAllText(filename, name + "\n");
                MessageBox.Show("Welcome " + name + "!");
                error_method("ChatBot", "Hey " + name + ", welcome to the Cyber Security Bot!(Type 'quit' to exit application)");
            }
            else
            {
                error_method("ChatBot", "Hey " + name + ", welcome back! How can I help you today?(Type 'quit' to exit application))");
            }

            // switch to the chat screen
            name_grid.Visibility = Visibility.Hidden;
            chats_grid.Visibility = Visibility.Visible;

            // assign username globally
            userName = name;

        }// end of submit_name method

        // checks if the user has visited before
        private bool check_name(string name)
        {// start of check_name method

            string fileName = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "user_names.txt");
            bool found = false;

            // create the file if it's missing
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            // read all saved usernames
            string[] names = File.ReadAllLines(fileName);

            foreach (string name_found in names)
            {
                if (name_found.ToLower() == name.ToLower())
                {
                    found = true;
                    MessageBox.Show("Welcome back " + name_found + "!!");
                    break;
                }
            }

            return found;

        }// end of check_name method

        // fires when the user clicks Send
        private void Send(object sender, RoutedEventArgs e)
        {
            // start of send method

            string questions = question.Text.Trim();
            error_method(userName, questions);
            question.Clear();

            if (questions == "")
            {
                error_method("ChatBot", "Please enter a question.");
                return;
            }

            if (questions.ToLower() == "quit")
            {
                Application.Current.Shutdown();
                return;
            }

            

            // I check if the quiz is active first
            // if it is I treat every message as a quiz answer
            if (quizActive)
            {
                check_answer(questions.ToLower().Trim());
                return;
            }

            // I use my NLP class to detect what the user is trying to do
            // this works even if they phrase things differently
            var (intent, content) = NLP.DetectIntent(questions);

            if (intent == "add_task")
            {
                handle_add_task(string.IsNullOrEmpty(content) ? questions : content);
                return;
            }
            if (intent == "set_reminder")
            {
                handle_set_reminder(string.IsNullOrEmpty(content) ? questions : content);
                return;
            }
            if (intent == "view_tasks")
            {
                handle_view_tasks();
                return;
            }
            if (intent == "activity_log")
            {
                handle_activity_log();
                return;
            }
            if (intent == "quiz")
            {
                handle_quiz_start();
                return;
            }

            // I clean the input before doing keyword search
            // this removes punctuation that might break matching
            string cleanedInput = NLP.Normalise(questions);
            string[] words = cleanedInput.Split(' ');

            bool found = false;
            string message = string.Empty;
            Random indexer = new Random();

            ArrayList per_word = new ArrayList();
            ArrayList answers_found = new ArrayList();

            // I check sentiment first before doing keyword search
            string sentiment = Methods.check_sentiment(cleanedInput);
            if (sentiment != string.Empty)
            {
                error_method("ChatBot", sentiment);

                string topic_tip = Methods.get_topic_tip(cleanedInput, reply);
                if (topic_tip != string.Empty)
                {
                    error_method("ChatBot", "Here's a tip that might help: " + topic_tip);
                }

                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
                return;
            }

            if (cleanedInput.Contains("interested"))
            { // start of intercept interest if

                string store_interests = string.Empty;
                bool found_interest = false;

                // I loop through the sentence to grab the actual topic they care about
                foreach (string interest in words)
                {
                    if (!ignore.Contains(interest.ToLower()) && interest.ToLower() != "interested")
                    {
                        found_interest = true;
                        store_interests += interest + ", ";
                        found = true;
                    }
                }

                if (found_interest)
                {
                    // I trim the extra commas and save it straight to the file
                    userInterests = store_interests.TrimEnd(',', ' ');
                    string filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "interested_topic.txt");

                    System.IO.File.AppendAllText(filename, userName + ": " + store_interests.TrimEnd(',', ' ') + "\n");

                    answers_found.Add("Great! I'll remember that you are interested in " + store_interests.TrimEnd(',', ' ') + ".");
                    ActivityLog.Log(userName, "Stored interest: " + store_interests);
                }
                else
                {
                    answers_found.Add("Sorry, please make sure the topics are cyber related.");
                }

                // I display the result and stop the method immediately so nothing else runs
                if (answers_found.Count > 0)
                {
                    error_method("ChatBot", answers_found[0].ToString());
                    ActivityLog.Log(userName, "Responded to: " + cleanedInput);
                    chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
                    return;
                }

            } // end of intercept interest if

            // I check for follow up questions using lastTopic
            // so if the user says "tell me more" I already know what topic they mean
            if (cleanedInput.Contains("more") || cleanedInput.Contains("explain") ||
                cleanedInput.Contains("another") || cleanedInput == "tell me more")
            {
                if (!string.IsNullOrEmpty(lastTopic))
                {
                    string moreInfo = Methods.GetMoreInfo(lastTopic, reply);

                    if (moreInfo.Contains("don't have more details"))
                    {
                        error_method("ChatBot", moreInfo);
                    }
                    else
                    {
                        error_method("ChatBot", $"Here's more on '{lastTopic}': {moreInfo}");
                    }
                }
                else
                {
                    error_method("ChatBot", "More about what? Please ask a question first, then say 'tell me more'.");
                }
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
                return;
            }

            // try to match the full sentence first
            foreach (string answer in reply)
            {
                string[] parts = answer.Split('|');
                if (parts.Length < 2) continue;

                string keyword = parts[0].ToLower();

                if (cleanedInput.Contains(keyword))
                {
                    found = true;
                    per_word.Add(answer);
                }
            }

            // I only show the answer if per_word actually has something valid
            if (found && per_word.Count > 0)
            {
                int indexing = indexer.Next(0, per_word.Count);
                string chosen = per_word[indexing]?.ToString() ?? string.Empty;

                // I make sure chosen is not empty before splitting
                if (!string.IsNullOrEmpty(chosen))
                {
                    string[] parts = chosen.Split('|');
                    string responseText = parts.Length > 1 ? parts[1] : chosen;

                    if (parts.Length > 0)
                    {
                        lastTopic = parts[0].ToLower().Trim();
                    }

                    error_method("ChatBot", responseText);
                    ActivityLog.Log(userName, "Responded to: " + cleanedInput);
                    chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
                    return; // stops here only if we actually have a valid response
                }

                // if chosen was empty reset found so word by word search runs
                found = false;
                per_word.Clear();
            }


            // I only get here if the full sentence search found nothing
            // so now I try word by word with synonym resolution
            foreach (string word in words)
            {// start of main foreach

                bool wordFound = false;

                if (!ignore.Contains(word.ToLower()))
                {// start of check word if

                    per_word.Clear();

                    // I resolve synonyms before searching
                    // this way "virus" finds the malware answers
                    string resolvedWord = NLP.ResolveSynonym(word.ToLower());

                    foreach (string answer in reply)
                    {// start of answer loop

                        string[] parts = answer.Split('|');
                        if (parts.Length < 2) continue;

                        string keyword = parts[0].ToLower();
                        string response = parts[1];

                        if (keyword == resolvedWord)
                        {
                            wordFound = true;
                            found = true;
                            per_word.Add(response);
                        }

                    }// end of answer loop

                    // I pick one random answer and save the topic
                    if (wordFound && per_word.Count > 0)
                    {
                        int indexing = indexer.Next(0, per_word.Count);
                        answers_found.Add(per_word[indexing]);
                        lastTopic = resolvedWord;
                    }

                }// end of check word if

            }// end of main foreach

            // I show whatever answers were found across all matched words
            if (found && answers_found.Count > 0)
            {
                foreach (string per_answer in answers_found)
                {
                    message += per_answer + "\n";
                }

                error_method("ChatBot", message);
                ActivityLog.Log(userName, "Responded to: " + cleanedInput);
                chats.ScrollIntoView(chats.Items[chats.Items.Count - 1]);
            }
            else
            {
                // I only show this if absolutely nothing matched
                error_method("ChatBot", "Hmm, I didn't quite get that. Could you try rephrasing?");
                ActivityLog.Log(userName, "Did not understand: " + questions);
            }

        }// end of send method

        // displays a message in the ListView with colour
        private void error_method(string name, string message)
        {// start of error method
         // This method shows messages in different colours:
         // Green for ChatBot, Red/Orange for User

            bool isBot = name.ToLower() == "chatbot";

            chats.Items.Add(
                new TextBlock
                {
                    Padding = new Thickness(6, 4, 6, 4),
                    TextWrapping = TextWrapping.Wrap,
                    Inlines =
                    {
                new Run
                {
                    Text = name + " : ",
                    Foreground = isBot
                        ? (Brush)new SolidColorBrush(Color.FromRgb(0x00, 0xFF, 0x9C)) // green for bot
                        : (Brush)new SolidColorBrush(Color.FromRgb(0xFF, 0x79, 0x79)), // soft red for user
                    FontWeight = FontWeights.Bold
                },
                new Run
                {
                    Text = message,
                    Foreground = isBot
                        ? (Brush)new SolidColorBrush(Color.FromRgb(0xE6, 0xED, 0xF3)) // light white for bot
                        : (Brush)new SolidColorBrush(Color.FromRgb(0xFF, 0xC8, 0xC8))  // light pink for user
                }
                    }
                }
            );

        }// end of error method
    }
}
