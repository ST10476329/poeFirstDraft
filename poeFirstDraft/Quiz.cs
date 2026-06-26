using System;
using System.Collections.Generic;
using System.Text;

namespace poeFirstDraft
{
    internal class Quiz
    {
        // start of class

        // builds and returns the full list of questions
        // keeping questions here keeps MainWindow clean
        public static List<Questions> GetQuestions()
        {// start of GetQuestions

            List<Questions> questions = new List<Questions>();

            // ── Multiple Choice Questions ──────────────────────

            questions.Add(new Questions(
                "What does phishing attempt to steal?",
                new string[] { "A) Your device", "B) Your personal information", "C) Your wifi", "D) Your files" },
                "b",
                "Phishing tricks users into giving away personal info like passwords through fake emails or websites."
            ));

            questions.Add(new Questions(
                "What makes a strong password?",
                new string[] { "A) Your name and birthday", "B) A single word", "C) A mix of letters, numbers and symbols", "D) All zeros" },
                "c",
                "Strong passwords use a combination of uppercase, lowercase, numbers and symbols and are hard to guess."
            ));

            questions.Add(new Questions(
                "What does 2FA stand for?",
                new string[] { "A) Two File Access", "B) Two Factor Authentication", "C) Two Firewall Alerts", "D) Two Form Authorization" },
                "b",
                "Two Factor Authentication adds a second layer of security beyond just a password."
            ));

            questions.Add(new Questions(
                "What does a VPN do?",
                new string[] { "A) Speeds up your internet", "B) Blocks all viruses", "C) Encrypts your internet traffic", "D) Deletes your history" },
                "c",
                "A VPN encrypts your connection making it harder for others to intercept your data."
            ));

            questions.Add(new Questions(
                "Which of these is a sign of a phishing email?",
                new string[] { "A) Sent from a known contact", "B) Contains urgent language and suspicious links", "C) Has no attachments", "D) Written in formal English" },
                "b",
                "Phishing emails often create urgency and contain links to fake websites designed to steal your info."
            ));

            questions.Add(new Questions(
                "What is malware?",
                new string[] { "A) A type of hardware", "B) A secure browser", "C) Harmful software designed to damage devices", "D) A firewall setting" },
                "c",
                "Malware is malicious software including viruses, trojans and spyware that harms your device."
            ));

            questions.Add(new Questions(
                "What should you do after a data breach?",
                new string[] { "A) Nothing", "B) Change your passwords immediately", "C) Delete your account", "D) Turn off your device" },
                "b",
                "After a breach you should change passwords immediately and enable 2FA to secure your accounts."
            ));

            // ── True / False Questions ─────────────────────────

            questions.Add(new Questions(
                "TRUE or FALSE: You should use the same password for all accounts.",
                new string[] { "A) True", "B) False" },
                "b",
                "False — reusing passwords means if one account is hacked, all your accounts are at risk."
            ));

            questions.Add(new Questions(
                "TRUE or FALSE: HTTPS means a website is completely safe.",
                new string[] { "A) True", "B) False" },
                "b",
                "False — HTTPS means the connection is encrypted but the site could still be malicious."
            ));

            questions.Add(new Questions(
                "TRUE or FALSE: Public Wi-Fi is always safe to use for banking.",
                new string[] { "A) True", "B) False" },
                "b",
                "False — public Wi-Fi is not encrypted and attackers can intercept your data easily."
            ));

            questions.Add(new Questions(
                "TRUE or FALSE: Antivirus software alone is enough to keep you safe online.",
                new string[] { "A) True", "B) False" },
                "b",
                "False — antivirus is one layer of protection but safe browsing habits and updates are equally important."
            ));

            return questions;

        }// end of GetQuestions
    }
}
