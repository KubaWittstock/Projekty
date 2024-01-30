using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace QuizGame
{
    public class GameManager
    {
        public static List<Question> question; // list containing all the questions
        public static List<HighScore> highScores; //list contating top 100 scores
        private string qFilePath = @"questions.xml";
        private string hsFilePath = @"highScores.xml";

        public GameManager()
        {
            question = new List<Question>();
            highScores = new List<HighScore>();
            loadQuestionsFromFile();
            loadHighScoresFormFile();
        }
        public void saveHighScoresToFile()
        {
            using (Stream fs = new FileStream(hsFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<HighScore>));
                serializer.Serialize(fs, highScores);
            }
        }
        public void addScore(int score, string gameType, string playerName)
        {
            const int MAXIMUM_NUMBER_OF_SOCRES = 100;
            int counter = 0;
            if (score > 0)
            {
                foreach (var s in highScores)
                {
                    if (score > s.Scores) break;
                    counter++;

                }
                Console.WriteLine(counter);
                if (highScores.Count < MAXIMUM_NUMBER_OF_SOCRES)
                {
                    highScores.Insert(counter, new HighScore(playerName, score, gameType, DateTime.Now));
                }
                else
                {
                    highScores.Insert(counter, new HighScore(playerName, score, gameType, DateTime.Now));
                    highScores.RemoveAt(MAXIMUM_NUMBER_OF_SOCRES);
                }
            }
        }

        public void loadHighScoresFormFile()
        {
            if (File.Exists(hsFilePath))
            {

                XmlSerializer serializer = new XmlSerializer(typeof(List<HighScore>));
                using (FileStream fs = File.OpenRead(hsFilePath))
                {
                    highScores = (List<HighScore>)serializer.Deserialize(fs);
                }
            }
            else
            {
                saveHighScoresToFile();
            }

        }

        public void resetQuestionsToDefault()
        {
            //Method that creates the file with questions againmethod that creates the file with questions again
            Question.numberOfQuestions = 0;
            question.Clear();
            //1
            question.Add(new Question("What is the length of the equator?", "40 123 km", "40 321 km", "40 057 km", "40 075 km", "D", 1, "Geography"));
            //2
            question.Add(new Question("Which planet from the sun is the earth?", "1", "2", "3", "4", "C", 1, "Astronomy"));
            //3
            question.Add(new Question("4 + 4 * 4 = ?", "64", "20", "32", "16", "B", 1, "Maths"));
            //4
            question.Add(new Question("Which of these languages ​​is the programming language?", "HTML", "C++", "XML", "CSS", "B", 1, "Programming"));
            //5
            question.Add(new Question("How many voivodeships (provinces) are there in Poland?", "12", "14", "16", "18", "C", 2, "Geography"));
            //6
            question.Add(new Question("What is the distance from the earth to the moon?", "101 200 km", "203 900 km", "384 400 km", "732 210 km", "C", 2, "Astronomy"));
            //7
            question.Add(new Question("How many states are the US divided into?", "48", "50", "52", "53", "B", 2, "Geography"));
            //8
            question.Add(new Question("What is the area of ​​a cube with a side of 4 cm?", "96 cm2", "64 cm2", "16 cm2", "32 cm2", "A", 2, "Maths"));
            //9
            question.Add(new Question("How many horsepowers does McLaren F1 have?", "627 hp", "532 hp", "619 hp", "701 hp", "A", 3, "Automotive"));
            //10
            question.Add(new Question("1+2*3-4/5 =", "1", "0.6", "6.2", "-0.6", "C", 3, "Maths"));
            //11
            question.Add(new Question("What is the nearest galaxy to ours?", "Milky Way", "Betelgeuse", "Alpha Centauri", "Andromeda", "D", 3, "Astronomy"));
            //12
            question.Add(new Question("How long is one day on Mars?", "24h 15m", "23h 47m", "24h 01m", "24h 37m", "D", 3, "Astronomy"));
            //13
            question.Add(new Question("Who Invented the light bulb?", "Alexander Graham Bell", "Leonardo da Vinci", "Thomas Alva Edison", "Nikola Tesla", "C", 1, "Science"));
            //14
            question.Add(new Question("Which of the following men does not have a chemical element named for him?", "Niels Bohr", "Isaac Newton", "Enrico Fermi", "Albert Einstein", "B", 1, "Science"));
            //15 
            question.Add(new Question("In the context of Apple iPhones, what is Siri?", "The name of the camera", "The name of a game", "A talking personal assistant", "The photo editing app", "C", 1, "Technology"));
            //16
            question.Add(new Question("Roughly how many years ago was fire - making technology devised?", "10,000", "Over 50,000", "100,000", "Over 500,000", "D", 1, "Technology"));
            //17
            question.Add(new Question("What is the fastest animal on earth?", "Peregrine Falcon", "Frigate Bird", "Sail Fish", "Cheetah", "A", 2, "Zoology"));
            //18
            question.Add(new Question("Which Is The Biggest Land Animal?", "Brown Bear", "Affican Elephant", "Giraffe", "Crocodile", "B", 2, "Zoology"));
            //19
            question.Add(new Question("Arctic King, Saladin and Tom Thumb are which types of vegetable?", "Lettuce", "Tomato", "Potato", "Cabbage", "A", 2, "General Knowledge"));
            //20
            question.Add(new Question("Who of these is considered by many to be the greatest science and technology thinker in history?", "Leonardo da Vinci", "Michelangelo", "Raphael", "Donatello", "A", 2, "General Knowledge"));
            //21
            question.Add(new Question("What is a baby oyster called?", "Saccostrea", "Nacre", "Magallana", "Spat", "D", 3, "General Knowledge"));
            //22
            question.Add(new Question("Which English cathedral was destroyed by fire in 1666?", "Birmingham", "Coventry", "St Paul's", "Lichfield", "C", 3, "History"));
            //23
            question.Add(new Question("Who was the first English monarch to abdicate?", "Richard II", "Edward VIII", "George V", "William IV", "B", 3, "History"));
            //24
            question.Add(new Question("Dempo, Churchill Brothers, and Salgaocar are famous successful Indian what?", "Volleyball Clubs", "Handball Clubs", "Football Club", "Cricket Clubs", "C", 3, "Sport"));
            saveQuestionsToFile();
        }
        public void loadQuestionsFromFile()
        {
            //loading questions form file, if file doesn't exist resetQuestionsToDefault is called
            if (File.Exists(qFilePath))
            {

                XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
                using (FileStream fs = File.OpenRead(qFilePath))
                {
                    question = (List<Question>)serializer.Deserialize(fs);
                }
            }
            else
            {
                resetQuestionsToDefault();
            }


        }
        public void saveQuestionsToFile()
        {
            using (Stream fs = new FileStream(qFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Question>));
                serializer.Serialize(fs, question);
            }
        }

        public void addQuestion()
        {
            //adding new question on the end of the list
            string questionText;
            string ansA;
            string ansB;
            string ansC;
            string ansD;
            string correctAns;
            int diffLevel;
            string category;
            Console.Clear();
            int lastindex = question.Count;
            int newFreeIndex = question[lastindex - 1].ID + 1;


            Console.Clear();
            //Console.WriteLine(newFreeIndex);
            Console.Write("Enter category:");
            category = Console.ReadLine();

            Console.Write("\nEnter question:");
            questionText = Console.ReadLine();

            Console.Write("\nEnter answer A:");
            ansA = Console.ReadLine();

            Console.Write("\nEnter answer B:");
            ansB = Console.ReadLine();

            Console.Write("\nEnter answer C:");
            ansC = Console.ReadLine();

            Console.Write("\nEnter answer D:");
            ansD = Console.ReadLine();

            Console.Write("\nEnter correct answer:");
            correctAns = isCorectAnswerValid();

            Console.Write("\nEnter difficulty level:");
            string text = isDifficultyLevelValid();
            diffLevel = int.Parse(text);



            question.Add(new Question(newFreeIndex, questionText, ansA, ansB, ansC, ansD, correctAns, diffLevel, category));

            saveQuestionsToFile();


            Console.WriteLine("Question added! Press enter to continue!");
            Console.ReadLine();
            Console.Clear();
            administrationPanelOptions();


        }
        public void removeQuestion(int index)
        {
            //removing question with specific index

            if (isQuestionOnList(index))
            {
                Console.WriteLine("Do you want to remove this qustion?\n");
                Console.WriteLine(question[index - 1].Text);
                if (getConfirmation())
                {
                    question.RemoveAt(index - 1);
                    Console.WriteLine("Question with ID: {0} removed", index);
                    saveQuestionsToFile();
                }
                else
                {
                    Console.WriteLine("Opetation canceled!");
                }
            }
            else
            {
                Console.WriteLine("No question with ID: {0}", index);
            }
        }

        public void administrationPanelOptions()
        {
            Console.WriteLine("1 See all questions...");
            Console.WriteLine("2 View and Edit question...");
            Console.WriteLine("3 Add new question...");
            Console.WriteLine("4 Remove question by ID...");
            Console.WriteLine("5 Reset quesitons to deafault...");
            Console.WriteLine("6 Go back to main menu...");
            string answer = Console.ReadLine();

            switch (answer)
            {
                case "1":
                    Console.Clear();
                    getAllQuestions();
                    break;
                case "2":
                    viewAndEditQuestion();
                    break;
                case "3":
                    addQuestion();
                    break;
                case "4":
                    Console.Clear();
                    Console.Write("Enter ID: ");
                    bool isint = false;
                    int id;
                    do
                    {
                        isint = int.TryParse(Console.ReadLine(), out id);
                        if (!isint) Console.WriteLine("You need to enter int!");
                    } while (!isint);
                    removeQuestion(id);
                    Console.WriteLine("Press any key to reurn to administration Panel!");
                    Console.ReadLine();
                    Console.Clear();
                    administrationPanelOptions();
                    break;

                case "5":
                    Console.WriteLine("Questions will be reset to deafault! Are you sure?");
                    if (getConfirmation())
                    {
                        Console.Clear();
                        resetQuestionsToDefault();
                        Console.WriteLine("Questions reseted to deafualt! Press enter!");
                        Console.ReadLine();
                        Console.Clear();
                        administrationPanelOptions();
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Ok! Press enter!");
                        Console.ReadLine();
                        Console.Clear();
                        administrationPanelOptions();
                    }
                    break;
                case "6":
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Wrong input!");
                    Console.WriteLine("");
                    administrationPanelOptions();
                    break;
            }
        }
        public void getAllQuestions()
        {

            foreach (var q in question)
            {
                Console.WriteLine("Question {0}", q.ID);
                Console.WriteLine("Category: {0}", q.Cat);
                Console.WriteLine(q.Text);
                Console.WriteLine("A: {0}", q.A);
                Console.WriteLine("B: {0}", q.B);
                Console.WriteLine("C: {0}", q.C);
                Console.WriteLine("D: {0}", q.D);
                Console.WriteLine("Correct answer: {0}", q.Answer);
                Console.WriteLine("Difficulty level: {0}", q.DiffLvl);
                Console.WriteLine("");
            }
            Console.WriteLine("Press any key to continue!");
            Console.ReadLine();
            Console.Clear();
            administrationPanelOptions();
        }
        private int getIDofQuestion()
        {
            Console.Clear();
            Console.Write("Enter the id of question: ");
            bool isint = true;
            int id;
            do
            {
                if (!isint)
                {
                    Console.Clear();
                    Console.WriteLine("ID must be integer!");
                    Console.Write("Enter the id of question: ");
                }
                isint = int.TryParse(Console.ReadLine(), out id);
            } while (!isint);

            return id;
        }
        private bool isQuestionOnList(int qID)
        {
            foreach (var q in question)
            {
                if (q.ID == qID) return true;
            }
            return false;
        }
        private void dispalyOneQuestion(int qID)
        {
            Console.Clear();
            foreach (var questionToEdit in question)
            {
                if (questionToEdit.ID == qID)
                {
                    Console.WriteLine("Question {0}", questionToEdit.ID);
                    Console.WriteLine("Category: {0}", questionToEdit.Cat);
                    Console.WriteLine(questionToEdit.Text);
                    Console.WriteLine(questionToEdit.A);
                    Console.WriteLine(questionToEdit.B);
                    Console.WriteLine(questionToEdit.C);
                    Console.WriteLine(questionToEdit.D);
                    Console.WriteLine("Correct answer: {0}", questionToEdit.Answer);
                    Console.WriteLine("Difficulty level: {0}", questionToEdit.DiffLvl);
                    Console.WriteLine("");
                }
            }
        }
        private bool getConfirmation()
        {
            string conconfirmation;
            Console.WriteLine("");
            Console.WriteLine("Type \"yes\" or \"no\"");
            Console.Write("Your answer: ");
            conconfirmation = Console.ReadLine();
            conconfirmation = conconfirmation.ToUpper();
            switch (conconfirmation)
            {
                case "YES":
                    return true;
                case "NO":
                    return false;
                default:
                    Console.WriteLine("Error! Press any enter");
                    Console.ReadLine();
                    return false;
            }
        }
        public string isCorectAnswerValid()
        {
            string text = Console.ReadLine();
            text = text.ToUpper();
            switch (text)
            {
                case "A":
                    return "A";
                case "B":
                    return "B";
                case "C":
                    return "C";
                case "D":
                    return "D";
                default:
                    Console.WriteLine("Input must be A, B, C or D!");
                    return isCorectAnswerValid();

            }
        }
        public string isDifficultyLevelValid()
        {
            string text = Console.ReadLine();
            switch (text)
            {
                case "1":
                    return "1";
                case "2":
                    return "2";
                case "3":
                    return "3";
                default:
                    Console.WriteLine("Input must be 1, 2 or 3!");
                    return isDifficultyLevelValid();

            }
        }
        private void selectEditingOption(int qID)
        {
            Console.WriteLine("1) Category");
            Console.WriteLine("2) Text");
            Console.WriteLine("3) Answer A");
            Console.WriteLine("4) Answer B");
            Console.WriteLine("5) Answer C");
            Console.WriteLine("6) Answer D");
            Console.WriteLine("7) Correct answer");
            Console.WriteLine("8) Difficulty level");
            Console.WriteLine("9) Done!");
            Console.WriteLine("");
            Console.Write("What do you want to edit: ");
            string response = Console.ReadLine();
            string newText;
            bool confirmation;
            foreach (var q in question)
            {
                if (q.ID == qID)
                {
                    switch (response)
                    {
                        case "1":
                            Console.WriteLine("");
                            Console.Write("Type new category: ");
                            newText = Console.ReadLine();
                            Console.WriteLine("Are you sure u want to change category to {0}", newText);
                            confirmation = getConfirmation();
                            if (confirmation)
                            {
                                q.Cat = newText;
                                saveQuestionsToFile();
                                Console.Clear();
                                Console.WriteLine("Category changed succesfully! Press any enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Category is not changed! Press enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            break;
                        case "2":
                            Console.WriteLine("");
                            Console.Write("Type new question: ");
                            newText = Console.ReadLine();
                            Console.WriteLine("Are you sure u want to change question to {0}", newText);
                            confirmation = getConfirmation();
                            if (confirmation)
                            {
                                q.Text = newText;
                                saveQuestionsToFile();
                                Console.Clear();
                                Console.WriteLine("Question text changed succesfully! Press any enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Question text is not changed! Press enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            break;
                        case "3":
                            Console.WriteLine("");
                            Console.Write("Type new answer A: ");
                            newText = Console.ReadLine();
                            Console.WriteLine("Are you sure u want to change answer A to {0}", newText);
                            confirmation = getConfirmation();
                            if (confirmation)
                            {
                                q.A = newText;
                                saveQuestionsToFile();
                                Console.Clear();
                                Console.WriteLine("Answer A changed succesfully! Press any enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Answer A is not changed! Press enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            break;
                        case "4":
                            Console.WriteLine("");
                            Console.Write("Type new answer B: ");
                            newText = Console.ReadLine();
                            Console.WriteLine("Are you sure u want to change answer B to {0}", newText);
                            confirmation = getConfirmation();
                            if (confirmation)
                            {
                                q.B = newText;
                                saveQuestionsToFile();
                                Console.Clear();
                                Console.WriteLine("Answer B changed succesfully! Press any enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Answer B is not changed! Press enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            break;
                        case "5":
                            Console.WriteLine("");
                            Console.Write("Type new answer C: ");
                            newText = Console.ReadLine();
                            Console.WriteLine("Are you sure u want to change answer to {0}", newText);
                            confirmation = getConfirmation();
                            if (confirmation)
                            {
                                q.C = newText;
                                saveQuestionsToFile();
                                Console.Clear();
                                Console.WriteLine("Answer C changed succesfully! Press any enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Answer C is not changed! Press enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            break;
                        case "6":
                            Console.WriteLine("");
                            Console.Write("Type new answer D: ");
                            newText = Console.ReadLine();
                            Console.WriteLine("Are you sure u want to change answer to {0}", newText);
                            confirmation = getConfirmation();
                            if (confirmation)
                            {
                                q.D = newText;
                                saveQuestionsToFile();
                                Console.Clear();
                                Console.WriteLine("Answer D changed succesfully! Press any enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Answer D is not changed! Press enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            break;
                        case "7":
                            Console.WriteLine("");
                            Console.Write("Type new correct answer: ");
                            newText = isCorectAnswerValid();
                            Console.WriteLine("Are you sure u want to change correct answer to {0}", newText);
                            confirmation = getConfirmation();
                            if (confirmation)
                            {
                                q.Answer = newText;
                                saveQuestionsToFile();
                                Console.Clear();
                                Console.WriteLine("Correct answer changed succesfully! Press any enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Correct answer is not changed! Press enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            break;
                        case "8":
                            Console.WriteLine("");
                            Console.Write("Type new difficulty level: ");
                            newText = isDifficultyLevelValid();
                            int newDiff = int.Parse(newText);
                            Console.WriteLine("Are you sure u want to difficulty level to {0}", newText);
                            confirmation = getConfirmation();
                            if (confirmation)
                            {
                                q.DiffLvl = newDiff;
                                saveQuestionsToFile();
                                Console.Clear();
                                Console.WriteLine("Answer changed succesfully! Press any enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Answer is not changed! Press enter!");
                                Console.ReadLine();
                                dispalyOneQuestion(qID);
                                selectEditingOption(qID);
                            }
                            break;
                        case "9":
                            Console.Clear();
                            administrationPanelOptions();
                            break;
                        default:
                            Console.Clear();
                            dispalyOneQuestion(q.ID);
                            selectEditingOption(q.ID);
                            break;

                    }
                }
            }

        }
        public void viewAndEditQuestion()
        {
            Console.Clear();
            int id = getIDofQuestion();
            if (isQuestionOnList(id))
            {
                dispalyOneQuestion(id);
                selectEditingOption(id);
            }
            else
            {
                Console.WriteLine("Question is not on the list! Press enter!");
                Console.ReadLine();
                Console.Clear();
                administrationPanelOptions();
            }

        }
    }
}