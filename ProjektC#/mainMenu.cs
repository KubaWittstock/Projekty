using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace QuizGame
{
    class mainMenu
    {
        string[] Months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        Quiz quiz;
        GameManager gm;
        string response;
        public mainMenu()
        {
            gm = new GameManager();
        }
        public void displayMainMenu()
        {


            Console.Clear();

            Console.WriteLine("Who want to be Milionare?!?");
            Console.WriteLine("");
            Console.WriteLine("1. Start new game");
            Console.WriteLine("2. Display high scores");
            Console.WriteLine("3. Administration panel");
            Console.WriteLine("4. Quit game");
            Console.WriteLine("");
            getResponse();

        }
        private void selectGameMode()
        {
            Console.WriteLine("Select game mode (Normal - n or ByDifficulty - d)");
            string response;
            response = Console.ReadLine();
            response = response.ToUpper();

            switch (response)
            {
                case "N":
                    for (int i = 3; i > 0; i--)
                    {
                        Console.Clear();
                        Console.WriteLine("Starting new game in..");
                        Console.WriteLine("{0}..", i);
                        Thread.Sleep(1000);
                    }
                    quiz.normalGame();
                    getPlayerName();
                    gm.addScore(quiz.points, "Normal", quiz.playerName);
                    gm.saveHighScoresToFile();
                    break;
                case "D":
                    for (int i = 3; i > 0; i--)
                    {
                        Console.Clear();
                        Console.WriteLine("Starting new game in..");
                        Console.WriteLine("{0}..", i);
                        Thread.Sleep(1000);
                    }
                    quiz.byDifficulty();
                    getPlayerName();
                    gm.addScore(quiz.points, "ByDifficulty", quiz.playerName);
                    gm.saveHighScoresToFile();
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    selectGameMode();
                    break;
            }
        }
        public void getPlayerName()
        {
            if (quiz.points > 0)
            {
                Console.Clear();
                Console.Write("Enetr your name here: ");
                quiz.playerName = Console.ReadLine();
                if (quiz.playerName.Length == 0) getPlayerName();
            }
        }
        private void getResponse()
        {

            response = Console.ReadLine();

            switch (response)
            {
                case "1":
                    //Start new game  
                    quiz = new Quiz();
                    Console.Clear();
                    selectGameMode();
                    quiz = null;
                    displayMainMenu();
                    break;
                case "2":
                    //display high scores
                    Console.Clear();
                    setHighScoreListToDisplay();
                    Console.WriteLine("\nPress enter");
                    Console.ReadLine();
                    displayMainMenu();
                    break;
                case "3":
                    //administration panel
                    Console.Clear();
                    Console.WriteLine("Administration panel. Enter password: ");
                    string password = Console.ReadLine();
                    if (password == "1234")
                    {
                        Console.Clear();
                        gm.administrationPanelOptions();
                    }
                    else
                    {
                        Console.WriteLine("Incorrect Password! Press any key to go back to main menu!");
                        Console.ReadLine();
                    }
                    displayMainMenu();
                    break;
                case "4":
                    //quit
                    Console.Clear();
                    System.Environment.Exit(0);
                    break;
                default:
                    displayMainMenu();
                    break;
            }
        }

        public void setHighScoreListToDisplay()
        {
            Console.WriteLine("HighScores lists:");
            Console.WriteLine("1) Top 10 scores");
            Console.WriteLine("2) Top scores in this month");
            string response1 = Console.ReadLine();

            switch (response1)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("HighScores lists:");
                    Console.WriteLine("1) Normal Mode");
                    Console.WriteLine("2) ByDifficulty Mode");
                    string response2 = Console.ReadLine();

                    switch (response2)
                    {
                        case "1":
                            Console.Clear();
                            Console.WriteLine("Top 10 socres in Normal Mode:\n");
                            displayTopHighSocre("Normal");
                            break;
                        case "2":
                            Console.Clear();
                            Console.WriteLine("Top 10 socres in ByDifficulty Mode:\n");
                            displayTopHighSocre("ByDifficulty");
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Wrong input!");
                            setHighScoreListToDisplay();
                            break;

                    }
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("HighScores lists:");
                    Console.WriteLine("1) Normal Mode");
                    Console.WriteLine("2) ByDifficulty Mode");
                    string response3 = Console.ReadLine();

                    switch (response3)
                    {
                        case "1":
                            Console.Clear();
                            Console.WriteLine("Scores in Normal Mode in this month {0}:\n", Months[DateTime.Today.Month - 1]);
                            displayHighSocreByMonth("Normal");
                            break;
                        case "2":
                            Console.Clear();
                            Console.WriteLine("Scores in ByDifficulty Mode in this month {0}:\n", Months[DateTime.Today.Month - 1]);
                            displayHighSocreByMonth("ByDifficulty");
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Wrong input!");
                            setHighScoreListToDisplay();
                            break;

                    }
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    setHighScoreListToDisplay();
                    break;
            }

        }
        public void displayTopHighSocre(string mode)
        {

            if (GameManager.highScores.Count != 0)
            {
                int i = 1;
                Console.WriteLine("Rank\tScore\tName\tGameMode\tDate");
                foreach (var s in GameManager.highScores)
                {
                    if (s.GameMode == mode) Console.WriteLine("{0}\t{1}\t{2}\t{3}\t\t{4} {5}", i++, s.Scores, s.Name, s.GameMode, Months[s.Month-1], s.Year);
                    if (i > 10) break;

                }
            }
            else Console.WriteLine("No scores! Start new game");
        }
        public void displayHighSocreByMonth(string mode)
        {
            if(GameManager.highScores.Count != 0)
            {
                int i = 1;
                Console.WriteLine("Rank\tScore\tName\tGameMode\tDate");
                foreach (var s in GameManager.highScores)
                {
                    if (s.GameMode == mode && s.Month == DateTime.Today.Month) Console.WriteLine("{0}\t{1}\t{2}\t{3}\t\t{4} {5}", i++, s.Scores, s.Name, s.GameMode, Months[s.Month-1], s.Year);
                }
            }
            else Console.WriteLine("No scores! Start new game");
        }
    }
}
