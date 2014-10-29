using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillKeeper
{
    public class Person
    {
        private String team = "", name = "", characters = "";
        private UInt32 wins = 0, losses = 0, draws = 0;
        private Double mu, sigma;
        private List<String> alts = new List<String>();

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Team
        {
            get { return team; }
            set { team = value; }
        }

        public String Characters
        {
            get { return characters; }
            set { characters = value; }
        }

        public UInt32 Wins
        {
            get { return wins; }
            set { wins = value; }
        }

        public UInt32 Losses
        {
            get { return losses; }
            set { losses = value; }
        }

        public UInt32 Draws
        {
            get { return draws; }
            set { draws = value; }
        }

        public Double Mu
        {
            get { return mu; }
            set { mu = value; }
        }

        public Double Sigma
        {
            get { return sigma; }
            set { sigma = value; }
        }

        public Int32 Score
        {
            get { return (Int32) ((mu - sigma * 3) * 10); }
        }

        public Int32 WinPercent
        {
            get { if (TotalGames > 0) return (Int32)(wins * 100 / TotalGames); else return 0; }
        }

        public UInt32 TotalGames
        {
            get { return (wins + losses + draws); }
        }

        public List<String> Alts
        {
            get { return alts; }
            set { alts = value; }
        }

        public String AltsString
        {
            get
            {
                String temp = "";
                foreach(String alt in alts) {
                    temp += alt + ";";
                }
                if (temp.Length > 0)
                    temp = temp.Substring(0, temp.Length - 1);

                return temp;
            }
            set
            {
                List<String> result = value.Split(';').ToList();

                alts = result;
            }
        }
    }
}
