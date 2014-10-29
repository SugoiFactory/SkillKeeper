using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using Moserware.Skills;
using Moserware.Skills.TrueSkill;

namespace SkillKeeper
{
    public partial class SkillKeeper : Form
    {
        private Double startMu = 0;
        private Double startSigma = 0;
        private Boolean scoresChanged = false;

        private List<Person> playerList;
        private List<Match> matchList;

        private String openedWorld = "";

        public SkillKeeper()
        {
            matchList = new List<Match>();
            playerList = new List<Person>();

            startMu = 25;
            startSigma = startMu / 3;

            InitializeComponent();
            manualDatePicker.Value = DateTime.Today;
            historyDatePicker.Value = DateTime.Today;
            personBindingSource.DataSource = new BindingList<Person>(playerList);
            openWorldDialog.Reset();
            exportCSVDialog.Reset();
        }

        // ------------------------------------
        // Useful Methods
        // ------------------------------------
        private void TabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (scoresChanged)
            {
                matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
                Toolbox.recalcMatches(playerList, matchList, startMu, startSigma);
                buildHistory();
                playerList = playerList.OrderByDescending(s => s.Score).ToList();
                personBindingSource.DataSource = new BindingList<Person>(playerList);
            }
            scoresChanged = false;
        }

        

        private void rebuildPlayerSelection()
        {
            personBindingSource.DataSource = new BindingList<Person>(playerList);

            List<String> playerSelectionList = new List<String>();
            playerSelectionList.Clear();
            player1Selector.Items.Clear();
            player2Selector.Items.Clear();
            modifySelector.Items.Clear();
            foreach (Person person in playerList)
            {
                playerSelectionList.Add(person.Name);
            }
            playerSelectionList.Sort();

            foreach (String playerName in playerSelectionList)
            {
                player1Selector.Items.Add(playerName);
                player2Selector.Items.Add(playerName);
                modifySelector.Items.Add(playerName);
            }

        }

        private void updatePlayerMatch(UInt16 winner)
        {
            Person p1 = new Person();
            Person p2 = new Person();
            foreach (Person person in playerList)
            {
                if (person.Name == player1Selector.Text)
                {
                    p1 = person;
                }
                if (person.Name == player2Selector.Text)
                {
                    p2 = person;
                }
            }

            Player p1s = new Player(1);
            Player p2s = new Player(2);
            Rating p1r = new Rating(p1.Mu, p1.Sigma);
            Rating p2r = new Rating(p2.Mu, p2.Sigma);
            Team t1 = new Team(p1s, p1r);
            Team t2 = new Team(p2s, p2r);

            IDictionary<Player, Rating> newRatings = null;

            if(winner == 0)
                newRatings = TrueSkillCalculator.CalculateNewRatings(GameInfo.DefaultGameInfo, Teams.Concat(t1, t2), 1, 1);
            else if(winner == 1)
                newRatings = TrueSkillCalculator.CalculateNewRatings(GameInfo.DefaultGameInfo, Teams.Concat(t1, t2), 1, 2);
            else if(winner == 2)
                newRatings = TrueSkillCalculator.CalculateNewRatings(GameInfo.DefaultGameInfo, Teams.Concat(t1, t2), 2, 1);

            p1.Mu = newRatings[p1s].Mean;
            p1.Sigma = newRatings[p1s].StandardDeviation;
            p2.Mu = newRatings[p2s].Mean;
            p2.Sigma = newRatings[p2s].StandardDeviation;

            foreach (Person person in playerList)
            {
                if (person.Name == player1Selector.Text)
                {
                    person.Mu = p1.Mu;
                    person.Sigma = p1.Sigma;
                    if (winner == 0)
                        person.Draws++;
                    else if (winner == 1)
                        person.Wins++;
                    else if (winner == 2)
                        person.Losses++;
                }
                if (person.Name == player2Selector.Text)
                {
                    person.Mu = p2.Mu;
                    person.Sigma = p2.Sigma;
                    if (winner == 0)
                        person.Draws++;
                    else if (winner == 1)
                        person.Losses++;
                    else if (winner == 2)
                        person.Wins++;
                }
            }

            refreshScoreBoxes();
        }

        private void refreshScoreBoxes()
        {
            Person p1 = new Person();
            Person p2 = new Person();
            foreach (Person person in playerList)
            {
                if (person.Name == player1Selector.Text)
                {
                    p1 = person;
                }
                if (person.Name == player2Selector.Text)
                {
                    p2 = person;
                }
            }

            p1MuDisplay.Text = p1.Mu.ToString();
            p1SigmaDisplay.Text = p1.Sigma.ToString();
            p1ScoreDisplay.Text = p1.Score.ToString();
            p2MuDisplay.Text = p2.Mu.ToString();
            p2SigmaDisplay.Text = p2.Sigma.ToString();
            p2ScoreDisplay.Text = p2.Score.ToString();
            p1WLDBox.Text = p1.Wins + " -- " + p1.Losses + " -- " + p1.Draws;
            p2WLDBox.Text = p2.Wins + " -- " + p2.Losses + " -- " + p2.Draws;

            Player p1s = new Player(1);
            Player p2s = new Player(2);
            Rating p1r = new Rating(p1.Mu, p1.Sigma);
            Rating p2r = new Rating(p2.Mu, p2.Sigma);
            Team t1 = new Team(p1s, p1r);
            Team t2 = new Team(p2s, p2r);

            matchQualBox.Text = (TrueSkillCalculator.CalculateMatchQuality(GameInfo.DefaultGameInfo, Teams.Concat(t1, t2)) * 100).ToString();
            
        }

        private void buildHistory()
        {
            List<Match> viewList = new List<Match>();
            foreach (Match match in matchList)
            {
                if (historyViewAllCheck.Checked)
                    viewList.Add(match);
                else if (match.Timestamp == historyDatePicker.Value)
                    viewList.Add(match);
            }

            matchBindingSource.DataSource = new BindingList<Match>(viewList);
        }

        // ------------------------------------
        // File Tab
        // ------------------------------------
        private void fileNewButton_Click(object sender, EventArgs e)
        {
            playerList.Clear();
            matchList.Clear();
            modifySelector.Text = "";
            modifyPlayerButton.Enabled = false;
            modifyDeleteButton.Enabled = false;
            p1WinButton.Enabled = false;
            p2WinButton.Enabled = false;
            drawButton.Enabled = false;

            openedWorld = "";

            rebuildPlayerSelection();
            Toolbox.recalcMatches(playerList, matchList, startMu, startSigma);
            buildHistory();
        }

        private void fileLoadButton_Click(object sender, EventArgs e)
        {
            playerList.Clear();
            matchList.Clear();

            openWorldDialog.Filter = "SkillKeeper92 files (*.sk92)|*.sk92|All files (*.*)|*.*";

            if (openWorldDialog.ShowDialog() == DialogResult.OK)
            {
                openedWorld = openWorldDialog.FileName;

                var xEle = XElement.Load(openWorldDialog.FileName);

                foreach (XElement player in xEle.Element("Players").Elements("Player"))
                {
                    Person person = new Person();
                    person.Name = player.Attribute("Name").Value;
                    person.Team = player.Attribute("Team").Value;
                    person.Characters = player.Attribute("Characters").Value;
                    person.AltsString = player.Attribute("Alts").Value;

                    playerList.Add(person);
                }

                foreach (XElement m in xEle.Element("Matches").Elements("Match"))
                {
                    Match match = new Match();
                    match.Timestamp = DateTime.Parse(m.Attribute("Timestamp").Value);
                    match.Order = UInt32.Parse(m.Attribute("Order").Value);
                    match.Player1 = m.Attribute("Player1").Value;
                    match.Player2 = m.Attribute("Player2").Value;
                    match.Winner = UInt16.Parse(m.Attribute("Winner").Value);

                    matchList.Add(match);
                }
            }

            rebuildPlayerSelection();
            Toolbox.recalcMatches(playerList, matchList, startMu, startSigma);
            buildHistory();
        }

        private void fileSaveButton_Click(object sender, EventArgs e)
        {
            saveWorldDialog.Reset();
            if (openedWorld.Length > 0)
                saveWorldDialog.FileName = openedWorld;

            saveWorldDialog.Filter = "SkillKeeper92 files (*.sk92)|*.sk92|All files (*.*)|*.*";

            if (saveWorldDialog.ShowDialog() == DialogResult.OK)
            {
                var xEle = new XElement(
                    new XElement("SK92",
                        new XElement("Players", from player in playerList select new XElement("Player", 
                            new XAttribute("Name", player.Name), 
                            new XAttribute("Team", player.Team), 
                            new XAttribute("Characters", player.Characters),
                            new XAttribute("Alts", player.AltsString)
                        )),
                        new XElement("Matches", from match in matchList select new XElement("Match",
                            new XAttribute("Timestamp", match.Timestamp.ToString()),
                            new XAttribute("Order", match.Order),
                            new XAttribute("Player1", match.Player1),
                            new XAttribute("Player2", match.Player2),
                            new XAttribute("Winner", match.Winner)
                        ))
                ));
                xEle.Save(saveWorldDialog.FileName);
            }
        }

        private void fileImportTioButton_Click(object sender, EventArgs e)
        {
            importFileDialog.Filter = "TIO files (*.tio)|*.tio|XML files (*.xml)|*.xml|All files (*.*)|*.*";

            if (importFileDialog.ShowDialog() == DialogResult.OK)
            {
                SKTioImporter importer = new SKTioImporter();
                importer.importFile(importFileDialog.FileName, playerList);
                if (importer.ShowDialog() == DialogResult.OK)
                {
                    foreach (Person p in importer.getImportPlayers())
                    {
                        playerList.Add(p);
                    }
                    foreach (Match m in importer.getImportMatches())
                    {
                        m.Order = Toolbox.getNewMatchNumber(matchList, m.Timestamp);
                        matchList.Add(m);
                    }

                    rebuildPlayerSelection();

                    matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
                    Toolbox.recalcMatches(playerList, matchList, startMu, startSigma);
                    buildHistory();
                    playerList = playerList.OrderByDescending(s => s.Score).ToList();
                    personBindingSource.DataSource = new BindingList<Person>(playerList);
                }
            }
        }

        // ------------------------------------
        // Add Player Tab
        // ------------------------------------
        private void addPlayerButton_Click(object sender, EventArgs e)
        {
            Person person = new Person();
            person.Name = addPlayerBox.Text;
            person.Team = addTeamBox.Text;
            person.Characters = addCharacterBox.Text;
            person.Mu = startMu;
            person.Sigma = startSigma;

            addPlayerBox.Clear();
            addTeamBox.Clear();
            addCharacterBox.Clear();

            playerList.Add(person);
            rebuildPlayerSelection();

            addTeamBox.Select();
        }

        private void playerAddBox_TextChanged(object sender, EventArgs e)
        {
            addPlayerButton.Enabled = true;
            if (addPlayerBox.Text.Length == 0)
            {
                addPlayerButton.Enabled = false;
                return;
            }
            foreach (Person person in playerList)
            {
                if (addPlayerBox.Text == person.Name)
                {
                    addPlayerButton.Enabled = false;
                }
            }
        }

        // ------------------------------------
        // Manual Results Tab
        // ------------------------------------
        private void p1WinButton_Click(object sender, EventArgs e)
        {
            Match match = new Match();
            match.Player1 = player1Selector.Text;
            match.Player2 = player2Selector.Text;
            match.Winner = 1;
            match.Description = ManualDescBox.Text;
            match.Timestamp = manualDatePicker.Value;
            match.Order = Toolbox.getNewMatchNumber(matchList, match.Timestamp);

            matchList.Add(match);
            updatePlayerMatch(1);
            scoresChanged = true;
        }

        private void p2WinButton_Click(object sender, EventArgs e)
        {
            Match match = new Match();
            match.Player1 = player1Selector.Text;
            match.Player2 = player2Selector.Text;
            match.Winner = 2;
            match.Description = ManualDescBox.Text;
            match.Timestamp = manualDatePicker.Value;
            match.Order = Toolbox.getNewMatchNumber(matchList, match.Timestamp);

            matchList.Add(match);
            updatePlayerMatch(2);
            scoresChanged = true;
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            Match match = new Match();
            match.Player1 = player1Selector.Text;
            match.Player2 = player2Selector.Text;
            match.Winner = 0;
            match.Description = ManualDescBox.Text;
            match.Timestamp = manualDatePicker.Value;
            match.Order = Toolbox.getNewMatchNumber(matchList, match.Timestamp);

            matchList.Add(match);
            updatePlayerMatch(0);
            scoresChanged = true;
        }

        private void player1Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            p1WinButton.Enabled = false;
            p2WinButton.Enabled = false;
            drawButton.Enabled = false;

            Person person = new Person();
            Boolean p2Valid = false;
            foreach (Person p1 in playerList)
            {
                if (p1.Name == player1Selector.Text)
                {
                    person = p1;
                }
                if (p1.Name == player2Selector.Text)
                {
                    person = p1;
                    p2Valid = true;
                }
            }

            refreshScoreBoxes();

            if (player1Selector.Text != player2Selector.Text && p2Valid)
            {
                p1WinButton.Enabled = true;
                p2WinButton.Enabled = true;
                drawButton.Enabled = true;
            }
        }

        private void player2Box_SelectedIndexChanged(object sender, EventArgs e)
        {
            p1WinButton.Enabled = false;
            p2WinButton.Enabled = false;
            drawButton.Enabled = false;

            Person person = new Person();
            Boolean p1Valid = false;
            foreach (Person p1 in playerList)
            {
                if (p1.Name == player2Selector.Text)
                {
                    person = p1;
                }
                if (p1.Name == player1Selector.Text)
                {
                    person = p1;
                    p1Valid = true;
                }
            }

            refreshScoreBoxes();

            if (player1Selector.Text != player2Selector.Text && p1Valid)
            {
                p1WinButton.Enabled = true;
                p2WinButton.Enabled = true;
                drawButton.Enabled = true;
            }
        }

        // ------------------------------------
        // View/Modify Tab
        // ------------------------------------
        private void modifySelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Person person = new Person();
            foreach (Person p1 in playerList)
            {
                if (p1.Name == modifySelector.Text)
                {
                    person = p1;
                }
            }

            modifyPlayerButton.Enabled = true;
            modifyDeleteButton.Enabled = true;

            modifyTeamBox.Text = person.Team;
            modifyNameBox.Text = person.Name;
            modifyCharacterBox.Text = person.Characters;

            modifyAltsBox.Clear();
            foreach (String alt in person.Alts)
            {
                modifyAltsBox.Text += alt + ";";
            }
            if(modifyAltsBox.Text.Length > 0)
                modifyAltsBox.Text = modifyAltsBox.Text.Substring(0, modifyAltsBox.Text.Length - 1);

            modifyMuBox.Text = person.Mu.ToString();
            modifySigmaBox.Text = person.Sigma.ToString();
            modifyScoreBox.Text = person.Score.ToString();
            modifyWLDBox.Text = person.Wins + " -- " + person.Losses + " -- " + person.Draws;

            String details = "INDIVIDUAL MATCH TALLY:\r\n";

            foreach (Person opponent in playerList)
            {
                if (opponent.Name != person.Name)
                {
                    int wins = 0;
                    int losses = 0;
                    int draws = 0;
                    foreach (Match match in matchList)
                    {
                        if (match.Player1 == opponent.Name && match.Player2 == person.Name)
                        {
                            if (match.Winner == 1)
                                losses++;
                            else if (match.Winner == 2)
                                wins++;
                            else if (match.Winner == 0)
                                draws++;
                        }
                        else if (match.Player1 == person.Name && match.Player2 == opponent.Name)
                        {
                            if (match.Winner == 1)
                                wins++;
                            else if (match.Winner == 2)
                                losses++;
                            else if (match.Winner == 0)
                                draws++;
                        }
                    }
                    if(wins + losses + draws > 0)
                        details += opponent.Name + " -- " + wins + "/" + losses + "/" + draws + "\r\n";
                }
            }
            details += "\r\nMATCH HISTORY:\r\n";

            foreach (Match match in matchList)
            {
                if (match.Player2 == person.Name)
                {
                    if (match.Winner == 1)
                        details += "LOSS vs " + match.Player1 + " (" + match.P2Score + " vs " + match.P1Score + ", " + (match.P2Score2 - match.P2Score) + ") -- " + match.Description + " -- " + match.Timestamp.Date.ToShortDateString() + "\r\n";
                    else if (match.Winner == 2)
                        details += "WIN vs " + match.Player1 + " (" + match.P2Score + " vs " + match.P1Score + ", " + (match.P2Score2 - match.P2Score) + ") -- " + match.Description + " -- " + match.Timestamp.Date.ToShortDateString() + "\r\n";
                    else if (match.Winner == 0)
                        details += "DRAW vs " + match.Player1 + " (" + match.P2Score + " vs " + match.P1Score + ", " + (match.P2Score2 - match.P2Score) + ") -- " + match.Description + " -- " + match.Timestamp.Date.ToShortDateString() + "\r\n";
                }
                else if (match.Player1 == person.Name)
                {
                    if (match.Winner == 1)
                        details += "WIN vs " + match.Player2 + " (" + match.P1Score + " vs " + match.P2Score + ", " + (match.P1Score2 - match.P1Score) + ") -- " + match.Description + " -- " + match.Timestamp.Date.ToShortDateString() + "\r\n";
                    else if (match.Winner == 2)
                        details += "LOSS vs " + match.Player2 + " (" + match.P1Score + " vs " + match.P2Score + ", " + (match.P1Score2 - match.P1Score) + ") -- " + match.Description + " -- " + match.Timestamp.Date.ToShortDateString() + "\r\n";
                    else if (match.Winner == 0)
                        details += "DRAW vs " + match.Player2 + " (" + match.P1Score + " vs " + match.P2Score + ", " + (match.P1Score2 - match.P1Score) + ") -- " + match.Description + " -- " + match.Timestamp.Date.ToShortDateString() + "\r\n";
                }
            }

            modifyDetailBox.Text = details;
        }

        private void modifyPlayerButton_Click(object sender, EventArgs e)
        {
            foreach (Person p1 in playerList)
            {
                if (p1.Name == modifySelector.Text)
                {
                    p1.Name = modifyNameBox.Text;
                    p1.Team = modifyTeamBox.Text;
                    p1.Characters = modifyCharacterBox.Text;

                    p1.Alts =modifyAltsBox.Text.Split(';').ToList();
                }
            }
            foreach (Match match in matchList)
            {
                if (match.Player1 == modifySelector.Text)
                    match.Player1 = modifyNameBox.Text;
                else if (match.Player2 == modifySelector.Text)
                    match.Player2 = modifyNameBox.Text;
            }

            rebuildPlayerSelection();
            modifySelector.SelectedItem = modifyNameBox.Text;
        }

        private void modifyDeleteButton_Click(object sender, EventArgs e)
        {
            modifyPlayerButton.Enabled = false;
            modifyDeleteButton.Enabled = false;

            foreach (Person p1 in playerList)
            {
                if (p1.Name == modifySelector.Text)
                {
                    playerList.Remove(p1);
                    break;
                }
            }
            Boolean recheck = true;
            while (recheck)
            {
                recheck = false;
                foreach (Match match in matchList)
                {
                    if (match.Player1 == modifySelector.Text || match.Player2 == modifySelector.Text)
                    {
                        matchList.Remove(match);
                        recheck = true;
                        break;
                    }
                }
            }
            rebuildPlayerSelection();
            modifySelector.Text = "";
        }

        // ------------------------------------
        // History Tab
        // ------------------------------------
        private void historyTab_Click(object sender, EventArgs e)
        {
            buildHistory();
        }

        private void historyDatePicker_ValueChanged(object sender, EventArgs e)
        {
            buildHistory();
        }

        private void historyViewAllCheck_CheckedChanged(object sender, EventArgs e)
        {
            buildHistory();
        }

        // ------------------------------------
        // Leaderboard Tab
        // ------------------------------------
        private void exportButton_Click(object sender, EventArgs e)
        {
            exportCSVDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            if (exportCSVDialog.ShowDialog() == DialogResult.OK)
            {
                var sb = new StringBuilder();

                var headers = leaderBoardGrid.Columns.Cast<DataGridViewColumn>();
                sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));

                foreach (DataGridViewRow row in leaderBoardGrid.Rows)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>();
                    sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
                }

                System.IO.StreamWriter csvFileWriter = new System.IO.StreamWriter(exportCSVDialog.FileName, false);
                csvFileWriter.Write(sb);
                csvFileWriter.Flush();
                csvFileWriter.Close();
            }
        }
    }
}
