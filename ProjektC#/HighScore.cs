using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuizGame
{
    [Serializable()]
    public class HighScore : ISerializable
    {
        static private int numberOfHighScores = 0;
        private int id;
        private string name;
        private int score;
        private string gameMode;
        private int month;
        private int year;

        public HighScore()
        {

        }

        public HighScore(string name, int score, string gameMode, DateTime date)
        {
            id = numberOfHighScores;
            numberOfHighScores++;
            this.name = name;
            this.score = score;
            this.gameMode = gameMode;
            this.month = date.Month;
            this.year = date.Year;
        }
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public int Scores
        {
            get { return score; }
            set { score = value; }
        }
        public string GameMode
        {
            get { return gameMode; }
            set { gameMode = value; }
        }
        public int Month
        {
            get { return month; }
            set { month = value; }
        }
        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("PlayerName", name);
            info.AddValue("Score", score);
            info.AddValue("GameMode", gameMode);
            info.AddValue("Month", month);
            info.AddValue("Year", year);
        }

        public HighScore(SerializationInfo info, StreamingContext context)
        {
            name = (string)info.GetValue("PlayerName", typeof(string));
            score = (int)info.GetValue("Score", typeof(int));
            gameMode = (string)info.GetValue("GameMode", typeof(string));
            month = (int)info.GetValue("Month", typeof(int));
            year = (int)info.GetValue("Year", typeof(int));
        }
    }
}
