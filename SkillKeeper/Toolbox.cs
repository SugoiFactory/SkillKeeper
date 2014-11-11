using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moserware.Skills;
using Moserware.Skills.TrueSkill;
using System.Windows.Forms;

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

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma, Int32 multiplier, UInt16 decay)
        {
            recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, DateTime.Today, null);
        }

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma, Int32 multiplier, UInt16 decay, ProgressBar progress)
        {
            recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, DateTime.Today, progress);
        }

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma, Int32 multiplier, UInt16 decay, DateTime lastDate)
        {
            recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, lastDate, null);
        }

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma, Int32 multiplier, UInt16 decay, DateTime lastDate, ProgressBar progress)
        {
            Dictionary<String, Person> playerMap = new Dictionary<string, Person>();
            DateTime latestMatch = DateTime.MinValue;
            int matchTotal = matchList.Count;
            int counted = 0;
            if (progress != null)
            {
                progress.Value = 0;
                progress.Refresh();
            }

            foreach (Person person in playerList)
            {
                person.Mu = startMu;
                person.Sigma = startSigma;
                person.Wins = 0;
                person.Losses = 0;
                person.Draws = 0;
                person.Multiplier = multiplier;
                person.DecayDays = 0;
                person.DecayMonths = 0;

                playerMap.Add(person.Name, person);
            }

            foreach (Match match in matchList)
            {
                if (progress != null)
                {
                    counted++;
                    progress.Value = (counted * 100) / matchTotal;
                    progress.PerformStep();
                }

                if (match.Timestamp <= lastDate)
                {
                    Person p1 = playerMap[match.Player1];
                    Person p2 = playerMap[match.Player2];

                    if (decay > 0)
                    {
                        uint i = 0;
                        if (decay < 3)
                        {
                            while (p1.LastMatch.AddDays(i).CompareTo(match.Timestamp) < 0)
                            {
                                i++;
                            }
                            p1.DecayDays += i;
                            i = 0;
                            while (p2.LastMatch.AddDays(i).CompareTo(match.Timestamp) < 0)
                            {
                                i++;
                            }
                            p2.DecayDays += i;
                        }
                        else
                        {
                            i = 0;
                            while (p1.LastMatch.AddMonths((int)i).CompareTo(match.Timestamp) < 0)
                            {
                                i++;
                            }
                            p1.DecayMonths += i;
                            i = 0;
                            while (p2.LastMatch.AddMonths((int)i).CompareTo(match.Timestamp) < 0)
                            {
                                i++;
                            }
                            p2.DecayMonths += i;
                        }

                        switch (decay)
                        {
                            case 1:
                                while (p1.DecayDays > 0)
                                {
                                    p1.decayScore(startSigma);
                                    p1.DecayDays--;
                                }
                                while (p2.DecayDays > 0)
                                {
                                    p2.decayScore(startSigma);
                                    p2.DecayDays--;
                                }
                                break;
                            case 2:
                                while (p1.DecayDays > 6)
                                {
                                    p1.decayScore(startSigma);
                                    p1.DecayDays -= 7;
                                }
                                while (p2.DecayDays > 6)
                                {
                                    p2.decayScore(startSigma);
                                    p2.DecayDays -= 7;
                                }
                                break;
                            case 3:
                                while (p1.DecayMonths > 0)
                                {
                                    p1.decayScore(startSigma);
                                    p1.DecayMonths--;
                                }
                                while (p2.DecayMonths > 0)
                                {
                                    p2.decayScore(startSigma);
                                    p2.DecayMonths--;
                                }
                                break;
                            case 4:
                                while (p1.DecayMonths > 11)
                                {
                                    p1.decayScore(startSigma);
                                    p1.DecayMonths -= 12;
                                }
                                while (p2.DecayMonths > 11)
                                {
                                    p2.decayScore(startSigma);
                                    p2.DecayMonths -= 12;
                                }
                                break;
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

                    p1.LastMatch = match.Timestamp;
                    p2.LastMatch = match.Timestamp;
                    if (latestMatch < match.Timestamp)
                        latestMatch = match.Timestamp;

                    if (match.Winner == 0)
                    {
                        p1.Draws++;
                        p2.Draws++;
                    }
                    else if (match.Winner == 1)
                    {
                        p1.Wins++;
                        p2.Losses++;
                    }
                    else if (match.Winner == 2)
                    {
                        p1.Losses++;
                        p2.Wins++;
                    }
                }
                else break;
            }

            foreach (Person p in playerList)
            {
                if (decay > 0)
                {
                    uint i = 0;
                    while (p.LastMatch.AddDays(i).CompareTo(latestMatch) < 0)
                    {
                        i++;
                    }
                    p.DecayDays += i;
                    i = 0;
                    while (p.LastMatch.AddMonths((int)i).CompareTo(latestMatch) < 0)
                    {
                        i++;
                    }
                    p.DecayMonths += i;

                    switch (decay)
                    {
                        case 1:
                            while (p.DecayDays > 0)
                            {
                                p.decayScore(startSigma);
                                p.DecayDays--;
                            }
                            break;
                        case 2:
                            while (p.DecayDays > 6)
                            {
                                p.decayScore(startSigma);
                                p.DecayDays -= 7;
                            }
                            break;
                        case 3:
                            while (p.DecayMonths > 0)
                            {
                                p.decayScore(startSigma);
                                p.DecayMonths--;
                            }
                            break;
                        case 4:
                            while (p.DecayMonths > 11)
                            {
                                p.decayScore(startSigma);
                                p.DecayMonths -= 12;
                            }
                            break;
                    }
                }
            }
        }
    }
}
