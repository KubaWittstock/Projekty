using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace QuizGame
{
    public class Quiz
    {

        public int points; // how much money will play win
        private int round = 0; // how many round did the user pass
        public Random random = new Random();
        private List<Question> easyQuestions;
        private List<Question> mediumQuestions;
        private List<Question> hardQuestions;        
        private bool playerWasRight ;
        private bool helpAvailable = true;
        private int difficultyLvl;        
        private Question currentQuestion;
        public string playerName;
        public Quiz()
        {
            easyQuestions = new List<Question>();
            mediumQuestions = new List<Question>();
            hardQuestions = new List<Question>();
            foreach (Question q in GameManager.question)
            {
                if (q.DiffLvl == 1) easyQuestions.Add(q);
                if (q.DiffLvl == 2) mediumQuestions.Add(q);
                if (q.DiffLvl == 3) hardQuestions.Add(q);
            }
            //Console.WriteLine("Easy list: {0} \nMedium list: {1} \nHard list: {2}", easyQuestions.Count, mediumQuestions.Count, hardQuestions.Count);
        }
        public void normalGame()
        {
            difficultyLvl = 1;
            points = 0;
            while (hardQuestions.Count != 0)
            {
                round++;
                if (round == 5 || round == 9 || easyQuestions.Count == 0 || mediumQuestions.Count == 0) difficultyLvl++;
                getRandomQuestion();
                displayQuestion();
                getPlayerResponse();
                if (!playerWasRight) 
                {
                    break;
                }


            }
            endGame();

            Console.ReadLine();

        }
        public void askForDifficulty()
        {
            
            Console.WriteLine("Determinate difficulty level(Easy - e, Medium - m, Hard - h): ");
            string response = Console.ReadLine();
            response = response.ToUpper();
            switch(response)
            {
                case "E":
                    difficultyLvl = 1;
                    break;
                case "M":
                    difficultyLvl = 2;
                    break;
                case "H":
                    difficultyLvl = 3;
                    break;
                default:
                    Console.Clear();  
                    Console.WriteLine("Wrong input!");
                    askForDifficulty();
                    break;
            }

        }
        public void byDifficulty()
        {
            Console.Clear();
            askForDifficulty();
            switch(difficultyLvl)
            {
                case 1:
                    if(easyQuestions.Count > 0)
                    {
                        while (easyQuestions.Count != 0)
                        {
                            round++;
                            getRandomQuestion();
                            displayQuestion();
                            getPlayerResponse();
                            if (!playerWasRight)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No questions from this category! Press enter");
                        Console.ReadLine();
                    }
                    break;
                case 2:
                    if(mediumQuestions.Count > 0)
                    {
                        while (mediumQuestions.Count != 0)
                        {
                            round++;
                            getRandomQuestion();
                            displayQuestion();
                            getPlayerResponse();
                            if (!playerWasRight)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No questions from this category! Press enter");
                        Console.ReadLine();
                    }
                    break;
                case 3:
                    if(hardQuestions.Count > 0)
                    {
                        while (hardQuestions.Count != 0)
                        {
                            round++;
                            getRandomQuestion();
                            displayQuestion();
                            getPlayerResponse();
                            if (!playerWasRight)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No questions from this category! Press enter");
                        Console.ReadLine();
                    }                    
                    break;
            }
            endGame();
            Console.ReadLine();

        }
        private void displayQuestion()
        {
            Console.Clear();
            Console.WriteLine("Question number: " + round);
            Console.WriteLine("Your prize: " + points);
            Console.WriteLine("Question category: " + currentQuestion.Cat);
            Console.WriteLine("Question difficulty: " + currentQuestion.DiffLvl);
            Console.WriteLine("Help available: " + helpAvailable);
            Console.WriteLine(currentQuestion.Text);
            Console.WriteLine("");
            Console.WriteLine("A: " + currentQuestion.A);
            Console.WriteLine("B: " + currentQuestion.B);
            Console.WriteLine("C: " + currentQuestion.C);
            Console.WriteLine("D: " + currentQuestion.D);
            Console.WriteLine("");
            Console.WriteLine("Options: a,b,c,d - pick answer, f - fifty fifty, e - exchange question, g - give up");
            Console.WriteLine("");
            
        }

        private void endGame()
        {
            Console.Clear();
            Console.WriteLine("You have won: " + points);
        }
        private void getPlayerResponse()
        {
            Console.WriteLine("Your answer: ");
            string response = Console.ReadLine();
            response = response.ToUpper();
            switch(response)
            {
                case "A":                          
                case "B":
                case "C":
                case "D":
                    checkAnswer(response);
                    break;
                case "E":                    
                    //exchange current question
                    exchangeTheQuestion();
                    getPlayerResponse();
                    break;
                case "F":
                    //Fifty / Fifty 
                    fiftyFifty();
                    getPlayerResponse();
                    break;
                case "G":
                    //give up
                    string sure;
                    Console.WriteLine("\nAre you sure you want to leave game? Y - yes, N - no");
                    sure = Console.ReadLine();
                    sure = sure.ToUpper();
                    switch (sure)
                    {
                        case "Y":
                            playerWasRight = false;
                            break;
                        default:
                            Console.Clear();
                            displayQuestion();
                            getPlayerResponse();
                            break;
                    }
                                     
                    break;
                default:
                    Console.Clear();
                    displayQuestion();
                    Console.WriteLine("Wrong input");
                    getPlayerResponse();
                    break;


            }
        }
        private void checkAnswer(string answer)
        {
            if (answer == currentQuestion.Answer)
            {
                playerWasRight = true;
                Console.WriteLine("Correct!");
                points += currentQuestion.DiffLvl * 100;
                Thread.Sleep(1000);
            }
            else
            {
                playerWasRight = false;
                Console.WriteLine("Wrong!");
                Thread.Sleep(1000);
            }
        }
        private void getRandomQuestion()
        {
            int randomQuestionIndex;
            switch (difficultyLvl)
            {
                case 1:
                    randomQuestionIndex = random.Next(0, easyQuestions.Count);
                    currentQuestion = easyQuestions[randomQuestionIndex].ShallowCopy();
                    easyQuestions.RemoveAt(randomQuestionIndex);
                    break;
                case 2:
                    randomQuestionIndex = random.Next(0, mediumQuestions.Count);
                    currentQuestion = mediumQuestions[randomQuestionIndex].ShallowCopy();
                    mediumQuestions.RemoveAt(randomQuestionIndex);
                    break;
                case 3:
                    randomQuestionIndex = random.Next(0, hardQuestions.Count);
                    currentQuestion = hardQuestions[randomQuestionIndex].ShallowCopy();
                    hardQuestions.RemoveAt(randomQuestionIndex);
                    break;
            }
            

            
        }

        private void fiftyFifty()
        {
            if (helpAvailable)
            {
                List<string> answers = new List<string> { "A", "B", "C", "D" };
                string answerToRemove = "";
                foreach (string a in answers)
                {
                    if (a == currentQuestion.Answer) answerToRemove = a;
                }
                answers.Remove(answerToRemove);

                int randomAnswer = random.Next(0, answers.Count);
                answers.RemoveAt(randomAnswer);
                foreach (string a in answers)
                {
                    switch (a)
                    {
                        case "A":
                            currentQuestion.A = "";
                            break;
                        case "B":
                            currentQuestion.B = "";
                            break;
                        case "C":
                            currentQuestion.C = "";
                            break;
                        case "D":
                            currentQuestion.D = "";
                            break;
                    }
                }
                Console.Clear();
                displayQuestion();
            }
            else
            {
                Console.Clear();
                displayQuestion();
                Console.WriteLine("\nYou have alredy used your help!\n");
            }
            helpAvailable = false;

        }
        private void exchangeTheQuestion()
        {
            if(helpAvailable)
            {
                getRandomQuestion();
                Console.Clear();
                displayQuestion();
            }
            else
            {
                Console.Clear();
                displayQuestion();
                Console.WriteLine("You have alredy used your help!\n");
            }
            helpAvailable = false;
        }
    }
}
