using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace POE_FirstDraft
{
    internal class Response
    {
        public Response(ArrayList reply, ArrayList ignore)
        { // this is the start of my constructor 
            answers(reply);
            words(ignore);

        } // this is the end of my constructor

        private void words(ArrayList Ignoring)
        {
            Ignoring.Add("i");
            Ignoring.Add("im");
            Ignoring.Add("i'm");
            Ignoring.Add("me");
            Ignoring.Add("my");
            Ignoring.Add("more");
            Ignoring.Add("about");
            Ignoring.Add("please");
            Ignoring.Add("the");
            Ignoring.Add("a");
            Ignoring.Add("an");
            Ignoring.Add("is");
            Ignoring.Add("what");
            Ignoring.Add("good");
            Ignoring.Add("great");
            Ignoring.Add("can");
            Ignoring.Add("you");
            Ignoring.Add("do");
            Ignoring.Add("be");
            Ignoring.Add("go");
            Ignoring.Add("no");
            Ignoring.Add("because");
            Ignoring.Add("come");
            Ignoring.Add("can't");
            Ignoring.Add("some");
            Ignoring.Add("just");
            Ignoring.Add("like");
            Ignoring.Add("want");
            Ignoring.Add("know");
            Ignoring.Add("have");
            Ignoring.Add("get");
            Ignoring.Add("give");
            Ignoring.Add("and");
            Ignoring.Add("or");
            Ignoring.Add("for");
            Ignoring.Add("with");
            Ignoring.Add("to");
        }

        public void answers(ArrayList add_answers)
        {   // Start of my answers Method

            add_answers.Add("greeting|hello! i'm doing fantastic, thank you! how are you today?");
            add_answers.Add("greeting|hey there! i'm doing great, thanks for asking. what can i do for you?");
            add_answers.Add("greeting|i'm good thanks! hope you're having a wonderful day!");

            add_answers.Add("purpose|my goal is to help you stay safe and smart while using the internet.");
            add_answers.Add("purpose|i'm here to teach you practical cybersecurity tips and best practices.");
            add_answers.Add("purpose|i help people protect themselves from online threats and scams.");

            add_answers.Add("cybersecurity|cybersecurity is about protecting systems and networks from digital threats.");
            add_answers.Add("cybersecurity|it involves protecting devices and online accounts from attacks.");
            add_answers.Add("cybersecurity|it focuses on securing digital information and systems.");

            add_answers.Add("phishing|phishing is a scam where attackers pretend to be trusted sources to steal information.");
            add_answers.Add("phishing|it uses fake messages or websites to trick users into revealing sensitive data.");
            add_answers.Add("phishing|attackers use deception to make users believe they are legitimate.");

            add_answers.Add("firewall|a firewall controls network traffic based on security rules.");
            add_answers.Add("firewall|it helps block unwanted access to your device or network.");
            add_answers.Add("firewall|it acts as a protective barrier between trusted and untrusted networks.");

            add_answers.Add("password|a password is used to secure access to your accounts or devices.");
            add_answers.Add("password|it should be strong, long and not easy to guess.");
            add_answers.Add("password|avoid using personal details when creating one.");

            add_answers.Add("hacked|immediately secure your account and log out of all devices.");
            add_answers.Add("hacked|contact support if your account has been compromised.");
            add_answers.Add("hacked|enable extra security like two-factor authentication.");

            add_answers.Add("fraud|contact your bank immediately if fraud is detected.");
            add_answers.Add("fraud|report suspicious financial activity to the authorities.");
            add_answers.Add("fraud|monitor your accounts for unusual activity.");

            add_answers.Add("vpn|a vpn helps protect your privacy on public wi-fi.");
            add_answers.Add("vpn|it encrypts your internet traffic for safety.");
            add_answers.Add("vpn|it improves security when using public networks.");

            add_answers.Add("malware|malware is harmful software designed to damage or gain access to your device.");
            add_answers.Add("malware|common types include viruses, trojans, spyware, and worms.");
            add_answers.Add("malware|always avoid downloading files from untrusted sources.");

            add_answers.Add("ransomware|ransomware encrypts your files and demands payment to unlock them.");
            add_answers.Add("ransomware|never pay the ransom - it encourages more attacks.");
            add_answers.Add("ransomware|regular backups are the best defense against ransomware.");

            add_answers.Add("twofactor|2FA adds an extra layer of security to your accounts.");
            add_answers.Add("twofactor|always enable two-factor authentication when available.");
            add_answers.Add("twofactor|use authenticator apps instead of SMS for better security.");

            add_answers.Add("update|keeping your software and apps updated is crucial for security.");
            add_answers.Add("update|software updates often fix critical security vulnerabilities.");
            add_answers.Add("update|enable automatic updates whenever possible.");

            add_answers.Add("antivirus|good antivirus software helps detect and remove threats.");
            add_answers.Add("antivirus|keep your antivirus definitions updated at all times.");
            add_answers.Add("antivirus|don't rely only on antivirus - use safe browsing habits too.");

            add_answers.Add("socialengineering|social engineering tricks people into revealing confidential information.");
            add_answers.Add("socialengineering|be careful when someone creates urgency or pressure.");
            add_answers.Add("socialengineering|always verify requests for sensitive information.");

            add_answers.Add("databreach|if your data was in a breach, change your password immediately.");
            add_answers.Add("databreach|use a password manager and enable 2FA after a data breach.");
            add_answers.Add("databreach|monitor your accounts for suspicious activity after a breach.");

            add_answers.Add("scamcall|unknown calls asking for personal information are usually scams.");
            add_answers.Add("scamcall|never give out personal or banking details over the phone.");
            add_answers.Add("scamcall|register on do-not-call lists and use call-blocking apps.");

            add_answers.Add("safebrowsing|only visit websites with HTTPS and valid security certificates.");
            add_answers.Add("safebrowsing|avoid clicking on suspicious links in emails or messages.");
            add_answers.Add("safebrowsing|use ad blockers and script blockers for extra protection.");

            add_answers.Add("backup|always back up important files to an external drive or cloud.");
            add_answers.Add("backup|follow the 3-2-1 backup rule for better data protection.");
            add_answers.Add("backup|test your backups regularly to make sure they work.");

            add_answers.Add("scammer|scammers often create urgency to pressure you into quick decisions.");
            add_answers.Add("scammer|never share personal or banking details with someone who contacted you first.");
            add_answers.Add("scammer|if something feels off, trust your gut and close the tab.");

            add_answers.Add("piracy|piracy means illegally downloading or sharing copyrighted material.");
            add_answers.Add("piracy|downloading movies, music or software from illegal sites is risky and can expose you to malware.");
            add_answers.Add("piracy|always use legal platforms to avoid viruses, legal trouble and support the creators.");

            // This is my sentiment detection code each catered to different moods

            // Worried
            add_answers.Add("worried|let's take a closer look and make sure everything is secure.");
            add_answers.Add("worried|your concern shows you take your online safety seriously, that's a good thing.");
            add_answers.Add("worried|let's run through a quick security check to put your mind at ease.");

            // Curious
            add_answers.Add("curious|that's a great mindset to have. knowing more makes you harder to attack.");
            add_answers.Add("curious|let's dive into that topic, the more you know the safer you'll be.");
            add_answers.Add("curious|asking questions is the first step to becoming cybersecurity aware.");

            // Frustrated
            add_answers.Add("frustrated|let's slow down and approach this differently, we'll find a solution.");
            add_answers.Add("frustrated|i get it, security settings can be tricky. let's tackle it together.");
            add_answers.Add("frustrated|your patience is appreciated, we're going to get this resolved.");

            // Angry
            add_answers.Add("angry|that reaction is completely valid, let's focus on getting you protected.");
            add_answers.Add("angry|let's put that energy into taking back control of your security.");
            add_answers.Add("angry|i hear you, now let's make sure this doesn't happen again.");

            // Sad
            add_answers.Add("sad|dealing with a cyber incident can feel very personal, you're not alone.");
            add_answers.Add("sad|let's focus on what we can do right now to improve your situation.");
            add_answers.Add("sad|things will get better, let's start by securing what we can today.");

            // Anxious
            add_answers.Add("anxious|let's start with the basics and work our way through this calmly.");
            add_answers.Add("anxious|focus on what you can control, and i'll help you with the rest.");
            add_answers.Add("anxious|small steps make a big difference, let's begin with the most important one.");

            // Confused
            add_answers.Add("confused|let's set aside the technical terms and focus on what matters most.");
            add_answers.Add("confused|i'll walk you through this in plain language, no jargon involved.");
            add_answers.Add("confused|let's start from the beginning and build your understanding from there.");

            // Scared
            add_answers.Add("scared|the fact that you reached out means you're already handling this well.");
            add_answers.Add("scared|let's focus on the steps you can take right now to feel more in control.");
            add_answers.Add("scared|you've come to the right place, let's work through this together.");

            // Relieved
            add_answers.Add("relieved|that's a good sign. now let's make sure your defences stay strong.");
            add_answers.Add("relieved|great progress. let's use this moment to review your overall security.");
            add_answers.Add("relieved|staying alert even when things are fine is what keeps you protected.");

            // Confident
            add_answers.Add("confident|that confidence will serve you well in staying safe online.");
            add_answers.Add("confident|you clearly have a good grasp of things, let me know how i can help further.");
            add_answers.Add("confident|that's the right attitude. let's build on that and strengthen your security.");


        }   // End of answers method
    }
}

