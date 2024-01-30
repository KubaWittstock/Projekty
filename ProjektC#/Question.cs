using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuizGame
{
    [Serializable()]
    public class Question : ISerializable
    {
        static public int numberOfQuestions = 0;
        private int id;
        private string text;
        private string a, b, c, d;
        private string answer;
        private string cat;   
        private int diffLvl;
  
        public Question ShallowCopy()
        {
            return (Question)this.MemberwiseClone();
        }
        public Question()
        {
            numberOfQuestions++;
        }

        public Question(int id, string text, string a, string b, string c, string d, string answer, int diffLvl, string cat)
        {
            this.id = id;
            this.text = text;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.answer = answer;
            this.cat = cat;
            this.diffLvl = diffLvl;


        }
        public Question(string text, string a, string b, string c, string d, string answer, int diffLvl, string cat)
        {
            numberOfQuestions++;
            this.id = numberOfQuestions;
            

            this.text = text;
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.answer = answer;
            this.cat = cat;
            this.diffLvl = diffLvl;
            
            
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public string A
        {
            get { return a; }
            set { a = value; }
        }

        public string B
        {
            get { return b; }
            set { b = value; }
        }

        public string C
        {
            get { return c; }
            set { c = value; }
        }
        public string D
        {
            get { return d; }
            set { d = value; }
        }
        public int DiffLvl
        {
            get { return diffLvl; }
            set { diffLvl = value; }
        }
        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }
        public string Cat
        {
            get { return cat; }
            set { cat = value; }
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", id);
            info.AddValue("Text", text);
            info.AddValue("A", a);
            info.AddValue("B", b);
            info.AddValue("C", c);
            info.AddValue("D", d);
            info.AddValue("Answer", answer);
            info.AddValue("Category", cat);
            info.AddValue("DiffilutyLevel", diffLvl);

        }

        public Question(SerializationInfo info, StreamingContext context)
        {
            id = (int)info.GetValue("ID", typeof(int));
            text = (string)info.GetValue("Text", typeof(string));
            a = (string)info.GetValue("A", typeof(string));
            b = (string)info.GetValue("B", typeof(string));
            c = (string)info.GetValue("C", typeof(string));
            d = (string)info.GetValue("D", typeof(string));
            answer = (string)info.GetValue("Answer", typeof(string));
            cat = (string)info.GetValue("Category", typeof(string));
            diffLvl = (int)info.GetValue("DiffLvl", typeof(int));
        }


    }
}
