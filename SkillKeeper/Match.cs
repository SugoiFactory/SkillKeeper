using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillKeeper
{
    public class Match
    {
        private DateTime timestamp = DateTime.Today;
        private String description = "";
        private UInt32 order = 1;
        private String player1, player2;
        private UInt16 winner = 0;
        private Int32 p1Score, p2Score, p1score2, p2score2;
        private String tourneyName;
        private String id = "000";

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
        public String TourneyName
        {
            get { return tourneyName; }
            set { tourneyName = value; }
        }
        public String Description
        {
            get { return description; }
            set { description = value; }
        }

        public String Player1
        {
            get { return player1; }
            set { player1 = value; }
        }

        public String Player2
        {
            get { return player2; }
            set { player2 = value; }
        }

        public UInt16 Winner
        {
            get { return winner; }
            set { winner = value; }
        }

        public UInt32 Order
        {
            get { return order; }
            set { order = value; }
        }

        public Int32 P1Score
        {
            get { return p1Score; }
            set { p1Score = value; }
        }

        public Int32 P2Score
        {
            get { return p2Score; }
            set { p2Score = value; }
        }

        public Int32 P1Score2
        {
            get { return p1score2; }
            set { p1score2 = value; }
        }

        public Int32 P2Score2
        {
            get { return p2score2; }
            set { p2score2 = value; }
        }

        public String ID
        {
            get { return id; }
            set { id = value; }
        }
    }
}
