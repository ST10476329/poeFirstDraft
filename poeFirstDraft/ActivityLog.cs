using System;
using System.Collections.Generic;
using System.IO;

namespace poeFirstDraft   
{
    internal class ActivityLog
    {
        // I use this list to keep recent actions in memory so it's faster to show them
        private static List<string> logs = new List<string>();

        // This method logs every important action the bot does
        public static void Log(string username, string action)
        {// start of Log

            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string entry = $"[{timestamp}] {username}: {action}";

            // Save to memory
            logs.Add(entry);

            //  save to file so it doesn't disappear when we close the app
            try
            {
                string filename = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "activity_log.txt");

                File.AppendAllText(filename, entry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // I don't want the whole program to crash just because logging failed
                Console.WriteLine("Warning: Could not write to log file - " + ex.Message);
            }

        }// end of Log

        // This returns the last few actions when the user asks "what have you done for me"
        public static List<string> GetRecentLogs(int count = 8)
        {
            // If memory is empty, try to load from file
            if (logs.Count == 0)
            {
                LoadLogsFromFile();
            }

            // Return only the most recent actions (keeps it clean)
            int startIndex = Math.Max(0, logs.Count - count);
            List<string> recent = new List<string>();

            for (int i = startIndex; i < logs.Count; i++)
            {
                recent.Add(logs[i]);
            }

            return recent;
        }

        // Helper method to load previous logs from file (useful if app restarts)
        private static void LoadLogsFromFile()
        {
            try
            {
                string filename = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, "activity_log.txt");

                if (File.Exists(filename))
                {
                    string[] allLines = File.ReadAllLines(filename);
                    logs.AddRange(allLines);
                }
            }
            catch
            {
                // If file doesn't exist or can't be read, just start fresh
            }
        }
    }
}