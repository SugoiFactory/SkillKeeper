using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moserware.Skills;
using Moserware.Skills.TrueSkill;

namespace SkillKeeper
{
    abstract class Toolbox
    {
        public static UInt32 getNewMatchNumber(List<Match> matches, DateTime date)
        {
            UInt32 result = 1;

            matches = matches.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList<Match>();
            foreach (Match match in matches)
            {
                if (match.Timestamp == date && match.Order >= result)
                    result = match.Order + 1;
            }

            return result;
        }

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma)
        {
            foreach (Person person in playerList)
            {
                person.Mu = startMu;
                person.Sigma = startSigma;
                person.Wins = 0;
                person.Losses = 0;
                person.Draws = 0;
            }

            foreach (Match match in matchList)
            {
                Person p1 = new Person();
                Person p2 = new Person();
                foreach (Person person in playerList)
                {
                    if (person.Name == match.Player1)
                    {
                        p1 = person;
                    }
                    if (person.Name == match.Player2)
                    {
                        p2 = person;
                    }
                }

                match.P1Score = p1.Score;
                match.P2Score = p2.Score;

                Player p1s = new Player(1);
                Player p2s = new Player(2);
                Rating p1r = new Rating(p1.Mu, p1.Sigma);
                Rating p2r = new Rating(p2.Mu, p2.Sigma);
                Team t1 = new Team(p1s, p1r);
                Team t2 = new Team(p2s, p2r);

                IDictionary<Player, Rating> newRatings = null;

                if (match.Winner == 0)
                    newRatings = TrueSkillCalculator.CalculateNewRatings(GameInfo.DefaultGameInfo, Teams.Concat(t1, t2), 1, 1);
                else if (match.Winner == 1)
                    newRatings = TrueSkillCalculator.CalculateNewRatings(GameInfo.DefaultGameInfo, Teams.Concat(t1, t2), 1, 2);
                else if (match.Winner == 2)
                    newRatings = TrueSkillCalculator.CalculateNewRatings(GameInfo.DefaultGameInfo, Teams.Concat(t1, t2), 2, 1);

                p1.Mu = newRatings[p1s].Mean;
                p1.Sigma = newRatings[p1s].StandardDeviation;
                p2.Mu = newRatings[p2s].Mean;
                p2.Sigma = newRatings[p2s].StandardDeviation;

                match.P1Score2 = p1.Score;
                match.P2Score2 = p2.Score;

                foreach (Person person in playerList)
                {
                    if (person.Name == match.Player1)
                    {
                        person.Mu = p1.Mu;
                        person.Sigma = p1.Sigma;
                        if (match.Winner == 0)
                            person.Draws++;
                        else if (match.Winner == 1)
                            person.Wins++;
                        else if (match.Winner == 2)
                            person.Losses++;
                    }
                    if (person.Name == match.Player2)
                    {
                        person.Mu = p2.Mu;
                        person.Sigma = p2.Sigma;
                        if (match.Winner == 0)
                            person.Draws++;
                        else if (match.Winner == 1)
                            person.Losses++;
                        else if (match.Winner == 2)
                            person.Wins++;
                    }
                }
            }
        }
    }
}
