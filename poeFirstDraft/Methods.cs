using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace POE_FirstDraft
{
    internal class Methods
    {
        // This method removes the keyword from the beginning of the answer.
        public static string CleanAnswer(string fullLine)
        {
            int spaceIndex = fullLine.IndexOf(' ');
            if (spaceIndex > 0)
                return fullLine.Substring(spaceIndex + 1);
            return fullLine;
        }

        // This gives more information about the last topic
        public static string GetMoreInfo(string topic, ArrayList reply)
        {
            ArrayList options = new ArrayList();

            foreach (string answer in reply)
            {
                if (string.IsNullOrEmpty(answer)) continue;

                string[] parts = answer.Split('|');
                if (parts.Length < 2) continue;

                string keyword = parts[0].ToLower().Trim();

                // Match if the topic matches the keyword
                if (keyword == topic.ToLower().Trim())
                {
                    options.Add(answer);
                }
            }

            if (options.Count > 0)
            {
                Random rand = new Random();
                string chosen = options[rand.Next(0, options.Count)].ToString();
                string[] parts = chosen.Split('|');
                return parts.Length > 1 ? parts[1].Trim() : chosen;
            }

            return "I don't have more details on that topic right now. Try asking about something else!";
        }
        public static string check_sentiment(string input)
        {// start of check_sentiment

            if (input.Contains("worried") || input.Contains("scared") || input.Contains("afraid"))
            {
                return "It's completely understandable to feel that way. Scammers can be very convincing. Let me share some tips to help you stay safe.";
            }
            else if (input.Contains("curious") || input.Contains("wondering"))
            {
                return "That curiosity is great! The more you know, the harder you are to attack. Let me share something useful.";
            }
            else if (input.Contains("frustrated") || input.Contains("annoyed"))
            {
                return "I totally get that, security stuff can feel overwhelming. Let's slow down and tackle this together.";
            }
            else if (input.Contains("confused") || input.Contains("unsure") || input.Contains("lost"))
            {
                return "No stress at all! Let me walk you through this in plain simple language.";
            }
            else if (input.Contains("angry"))
            {
                return "That reaction is completely valid. Let's focus on getting you protected right now.";
            }
            else if (input.Contains("sad"))
            {
                return "I'm sorry to hear that. Cyber incidents can feel very personal. You're not alone, let's work through this.";
            }
            else if (input.Contains("anxious"))
            {
                return "Take a breath, we'll go through this one step at a time. I've got you covered.";
            }
            else if (input.Contains("relieved"))
            {
                return "That's great to hear! Now let's make sure your defences stay strong going forward.";
            }
            else if (input.Contains("confident"))
            {
                return "Love that energy! Let's build on that confidence and tighten up your security even more.";
            }

            // no emotion detected, return empty
            return string.Empty;

        }// end of check_sentiment

        public static string get_topic_tip(string input, ArrayList reply)
        {// start of get_topic_tip

            // go through the reply list and find a tip that matches a word in the input
            foreach (string answer in reply)
            {
                // split each reply entry into keyword and response
                string[] parts = answer.Split('|');
                if (parts.Length < 2) continue;

                string keyword = parts[0].ToLower();
                string tip = parts[1];

                // if the user's message contains this keyword, return the tip
                if (input.ToLower().Contains(keyword))
                {
                    return tip;
                }
            }

            // no matching topic found
            return string.Empty;

        }// end of get_topic_tip

    }
}
