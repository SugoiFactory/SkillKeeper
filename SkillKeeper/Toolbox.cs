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

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma, Double multiplier, UInt16 decay, UInt32 decayValue)
        {
            recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue, DateTime.Today, null);
        }

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma, Double multiplier, UInt16 decay, UInt32 decayValue, ProgressBar progress)
        {
            recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue, DateTime.Today, progress);
        }

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma, Double multiplier, UInt16 decay, UInt32 decayValue, DateTime lastDate)
        {
            recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue, lastDate, null);
        }

        public static void recalcMatches(List<Person> playerList, List<Match> matchList, Double startMu, Double startSigma, Double multiplier, UInt16 decay, UInt32 decayValue, DateTime lastDate, ProgressBar progress)
        {
            lastDate += new TimeSpan(23, 59, 59);

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
                    
                    //Explained on line 177
                    p1.DecayDays = 0;
                    p2.DecayDays = 0;
                    p1.DecayMonths = 0;
                    p2.DecayMonths = 0;
                    
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
                                while (p1.DecayDays > decayValue - 1)
                                {
                                    p1.decayScore(startSigma);
                                    p1.DecayDays -= decayValue;
                                }
                                while (p2.DecayDays > decayValue - 1)
                                {
                                    p2.decayScore(startSigma);
                                    p2.DecayDays -= decayValue;
                                }
                                break;
                            case 2:
                                while (p1.DecayDays > (7 * decayValue) - 1)
                                {
                                    p1.decayScore(startSigma);
                                    p1.DecayDays -= 7 * decayValue;
                                }
                                while (p2.DecayDays > (7 * decayValue) - 1)
                                {
                                    p2.decayScore(startSigma);
                                    p2.DecayDays -= 7 * decayValue;
                                }
                                break;
                            case 3:
                                while (p1.DecayMonths > decayValue - 1)
                                {
                                    p1.decayScore(startSigma);
                                    p1.DecayMonths -= decayValue;
                                }
                                while (p2.DecayMonths > decayValue - 1)
                                {
                                    p2.decayScore(startSigma);
                                    p2.DecayMonths -= decayValue;
                                }
                                break;
                            case 4:
                                while (p1.DecayMonths > (12 * decayValue) - 1)
                                {
                                    p1.decayScore(startSigma);
                                    p1.DecayMonths -= 12 * decayValue;
                                }
                                while (p2.DecayMonths > (12 * decayValue) - 1)
                                {
                                    p2.decayScore(startSigma);
                                    p2.DecayMonths -= 12 * decayValue;
                                }
                                break;
                        }
                    }
                    /*
                    Correction to v1.0.1.6:
                    While decayDays/Months can be removed if they exceed the decayInterval,
                    if they do not they remain in the players cummulative decayDays/Months.
                    This means that players that would ordinarily not receive decay would.
                    
                    Ex. 6 Days between Matches with a week decayInterval.
                    Day 0  | Match 1 : No decayDays; No decay
                    DecayDays for player: 0
                    Day 6  | Match 2 : +6 decayDays; 6 > 7 days for week interval? No; No decay
                    DecayDays for player: 6
                    Day 12 | Match 3 : +6 decayDays; 12 > 7? Yes; unexpected decay
                    DecayDays for player: 5
                    
                    To correct this after each player is selected from the match they will have 
                    the decayDays/Months reset on line 85 and 246.
                    */
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
                //Explained on line 177
                p.DecayDays = 0;
                p.DecayMonths = 0;
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
                    /*
                    Correction to v1.0.1.6:
                    To correct 7 days and 1 week decayIntervals not yielding the same score,
                    it was found that the final player score adjustment did not take into 
                    account the decayValue like in the match-to-match decay.  This was added.
                    */
                        case 1:
                            while (p.DecayDays > decayValue - 1)
                            {
                                p.decayScore(startSigma);
                                p.DecayDays-= decayValue;
                            }
                            break;
                        case 2:
                            while (p.DecayDays > (7*decayValue) - 1)
                            {
                                p.decayScore(startSigma);
                                p.DecayDays -= 7*decayValue;
                            }
                            break;
                        case 3:
                            while (p.DecayMonths > decayValue - 1)
                            {
                                p.decayScore(startSigma);
                                p.DecayMonths-= decayValue;
                            }
                            break;
                        case 4:
                            while (p.DecayMonths > (12*decayValue) - 1)
                            {
                                p.decayScore(startSigma);
                                p.DecayMonths -= 12*decayValue;
                            }
                            break;
                    }
                }
            }
        }
    }
}
