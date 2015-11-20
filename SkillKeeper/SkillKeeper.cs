using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;
using Moserware.Skills;
using Moserware.Skills.TrueSkill;
using Fizzi.Libraries.ChallongeApiWrapper;

namespace SkillKeeper
{
    public partial class SkillKeeper : Form
    {
        private Double startMu = 0;
        private Double startSigma = 0;
        private Double multiplier = 200;
        private UInt16 decay = 0;
        private UInt32 decayValue = 1;
        private UInt32 minMatches = 1;
        private Boolean scoresChanged = false;

        private Boolean requireSave = false;

        private List<Person> playerList;
        private List<Person> leaderBoardList = new List<Person>();
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
            historyMoveDatePicker.Value = DateTime.Today;
            leaderboardDatePicker.Value = DateTime.Today;
            personBindingSource.DataSource = new BindingList<Person>(leaderBoardList);
            openWorldDialog.Reset();
            exportCSVDialog.Reset();
        }

        // ------------------------------------
        // Useful Methods
        // ------------------------------------
        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            if (requireSave)
            {
                DialogResult checkSave = confirmSave();
                if (checkSave == DialogResult.Yes)
                {
                    fileUpdateButton_Click(sender, e);
                }
                else if (checkSave == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void TabControl1_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (scoresChanged)
            {
                multiplier = 200;
                minMatches = 1;
                try
                {
                    multiplier = Double.Parse(settingsMultiplierBox.Text);
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                    Console.WriteLine(e2.StackTrace);
                }
                try
                {
                    minMatches = UInt32.Parse(settingsMatchesBox.Text);
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                    Console.WriteLine(e2.StackTrace);
                }
                try
                {
                    decayValue = UInt32.Parse(settingsDecayIntBox.Text);
                    if (decayValue < 1)
                        decayValue = 1;
                }
                catch (Exception e2)
                {
                    Console.WriteLine(e2.Message);
                    Console.WriteLine(e2.StackTrace);
                }
                matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
                Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
                buildHistory();

                requireSave = true;
            }
            scoresChanged = false;
            historyApplyButton.Enabled = false;
        }

        

        private void rebuildPlayerSelection()
        {
            personBindingSource.DataSource = new BindingList<Person>(leaderBoardList);

            List<String> playerSelectionList = new List<String>();
            playerSelectionList.Clear();
            player1Selector.Items.Clear();
            player2Selector.Items.Clear();
            modifySelector.Items.Clear();
            modifyCombineSelector.Items.Clear();
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
                modifyCombineSelector.Items.Add(playerName);
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

            requireSave = true;

            refreshScoreBoxes();
        }

        private void combinePlayers(String player1, String player2)
        {
            Person oldP = new Person(), newP = new Person();
            foreach (Person p in playerList)
            {
                if (p.Name == player1)
                {
                    oldP = p;
                }
                else if (p.Name == player2)
                {
                    newP = p;
                }
            }

            foreach (String alt in oldP.Alts)
            {
                newP.Alts.Add(alt);
            }
            newP.Alts.Add(oldP.Name);

            foreach (Match m in matchList)
            {
                if (m.Player1 == player1)
                    m.Player1 = player2;
                else if (m.Player2 == player1)
                    m.Player2 = player2;
            }
            modifySelector.Text = player2;

            playerList.Remove(oldP);
            rebuildPlayerSelection();

            matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
            Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
            buildHistory();

            requireSave = true;
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
            p1WLDBox.Text = p1.Wins + " -- " + p1.Losses + " -- " + p1.Draws + " (" + p1.WinPercent + "%)";
            p2WLDBox.Text = p2.Wins + " -- " + p2.Losses + " -- " + p2.Draws + " (" + p2.WinPercent + "%)";

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

            playerList = playerList.OrderByDescending(s => s.Score).ToList();
            leaderBoardList.Clear();
            foreach (Person p in playerList)
            {
                if (!p.Invisible && p.TotalGames >= minMatches)
                    leaderBoardList.Add(p);
            }
            personBindingSource.DataSource = new BindingList<Person>(leaderBoardList);
        }

        private DialogResult confirmSave()
        {
            ConfirmSave confirmSave = new ConfirmSave();
            confirmSave.ShowDialog();
            return confirmSave.DialogResult;
        }

        // ------------------------------------
        // File Tab
        // ------------------------------------
        private void fileNewButton_Click(object sender, EventArgs e)
        {
            if (requireSave)
            {
                DialogResult checkSave = confirmSave();
                if (checkSave == DialogResult.Yes)
                {
                    fileUpdateButton_Click(sender, e);
                }
                else if (checkSave == DialogResult.Cancel)
                    return;
            }

            playerList.Clear();
            matchList.Clear();
            modifySelector.Text = "";
            modifyPlayerButton.Enabled = false;
            modifyDeleteButton.Enabled = false;
            p1WinButton.Enabled = false;
            p2WinButton.Enabled = false;
            drawButton.Enabled = false;
            settingsDecayNever.Checked = true;

            multiplier = 200;
            decay = 0;
            settingsMultiplierBox.Text = "200";

            openedWorld = "";
            requireSave = false;

            rebuildPlayerSelection();
            Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
            buildHistory();
        }

        private void fileLoadButton_Click(object sender, EventArgs e)
        {
            if (requireSave)
            {
                DialogResult checkSave = confirmSave();
                if (checkSave == DialogResult.Yes)
                {
                    fileUpdateButton_Click(sender, e);
                }
                else if (checkSave == DialogResult.Cancel)
                    return;
            }

            playerList.Clear();
            matchList.Clear();

            openWorldDialog.Filter = "Bacon files (*.bcn)|*.bcn|SkillKeeper92 files (*.sk92)|*.sk92|All files (*.*)|*.*";

            if (openWorldDialog.ShowDialog() == DialogResult.OK)
            {
                openedWorld = openWorldDialog.FileName;
                requireSave = false;
                progressBar1.Visible = true;
                progressBar1.Value = 0;
                progressLabel.Visible = true;

                var xEle = XElement.Load(openWorldDialog.FileName);
                Int32 totalItems = xEle.Element("Players").Elements("Player").Count() + xEle.Element("Matches").Elements("Match").Count();
                Int32 progress = 0;

                if (xEle.Element("Settings") != null)
                {
                    multiplier = Int32.Parse(xEle.Element("Settings").Attribute("Multiplier").Value);
                    if(xEle.Element("Settings").Attribute("MinMatches") != null)
                        minMatches = UInt32.Parse(xEle.Element("Settings").Attribute("MinMatches").Value);
                    decay = UInt16.Parse(xEle.Element("Settings").Attribute("Decay").Value);
                    if (xEle.Element("Settings").Attribute("DecayValue") != null)
                        decayValue = UInt32.Parse(xEle.Element("Settings").Attribute("DecayValue").Value);
                    else
                        decayValue = 1;
                    settingsMultiplierBox.Text = multiplier.ToString();
                }

                progressLabel.Text = "Loading players...";
                progressLabel.Refresh();
                foreach (XElement player in xEle.Element("Players").Elements("Player"))
                {
                    Person person = new Person();
                    person.Name = player.Attribute("Name").Value;
                    person.Team = player.Attribute("Team").Value;
                    person.Invisible = Boolean.Parse(player.Attribute("Invisible").Value);
                    person.Characters = player.Attribute("Characters").Value;
                    person.AltsString = player.Attribute("Alts").Value;

                    playerList.Add(person);

                    progress++;
                    progressBar1.Value = (progress * 100) / totalItems;
                    progressBar1.PerformStep();
                    progressBar1.Refresh();
                }

                progressLabel.Text = "Loading matches...";
                progressLabel.Refresh();
                foreach (XElement m in xEle.Element("Matches").Elements("Match"))
                {
                    Match match = new Match();
                    match.Timestamp = DateTime.Parse(m.Attribute("Timestamp").Value);
                    match.Order = UInt32.Parse(m.Attribute("Order").Value);
                    if(m.Attribute("Description") != null)
                        match.Description = m.Attribute("Description").Value;
                    if (m.Attribute("ID") != null)
                        match.ID = m.Attribute("ID").Value;
                    else
                        match.ID = Guid.NewGuid().ToString("N");
                    match.Player1 = m.Attribute("Player1").Value;
                    match.Player2 = m.Attribute("Player2").Value;
                    match.Winner = UInt16.Parse(m.Attribute("Winner").Value);

                    matchList.Add(match);

                    progress++;
                    progressBar1.Value = (progress * 100) / totalItems;
                    progressBar1.PerformStep();
                }

                settingsDecayIntBox.Text = decayValue + "";
                switch (decay)
                {
                    case 1:
                        settingsDecayDaily.Checked = true;
                        break;
                    case 2:
                        settingsDecayWeekly.Checked = true;
                        break;
                    case 3:
                        settingsDecayMonthly.Checked = true;
                        break;
                    case 4:
                        settingsDecayYearly.Checked = true;
                        break;
                    case 0:
                        settingsDecayNever.Checked = true;
                        break;
                }

                progressLabel.Text = "Processing players...";
                progressLabel.Refresh();
                rebuildPlayerSelection();

                progressLabel.Text = "Calculating scores...";
                progressLabel.Refresh();
                Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue, progressBar1);

                progressLabel.Text = "Verifying match history...";
                progressLabel.Refresh();
                progressBar1.Refresh();
                buildHistory();

                progressBar1.Visible = false;
                progressLabel.Visible = false;

                requireSave = false;
            }
        }

        private void fileSaveButton_Click(object sender, EventArgs e)
        {
            if (scoresChanged)
                TabControl1_SelectedIndexChanged(sender, e);

            saveWorldDialog.Reset();
            if (openedWorld.Length > 0)
                saveWorldDialog.FileName = openedWorld;

            saveWorldDialog.Filter = "Bacon files (*.bcn)|*.bcn|SkillKeeper92 files (*.sk92)|*.sk92|All files (*.*)|*.*";

            if (saveWorldDialog.ShowDialog() == DialogResult.OK)
            {
                var xEle = new XElement(
                    new XElement("SK92",
                        new XElement("Settings",
                            new XAttribute("Multiplier", multiplier),
                            new XAttribute("MinMatches", minMatches),
                            new XAttribute("Decay", decay),
                                new XAttribute("DecayValue", decayValue)
                        ),
                        new XElement("Players", from player in playerList select new XElement("Player", 
                            new XAttribute("Name", player.Name), 
                            new XAttribute("Team", player.Team), 
                            new XAttribute("Invisible", player.Invisible),
                            new XAttribute("Characters", player.Characters),
                            new XAttribute("Alts", player.AltsString)
                        )),
                        new XElement("Matches", from match in matchList select new XElement("Match",
                            new XAttribute("ID", match.ID),
                            new XAttribute("Timestamp", match.Timestamp.ToString()),
                            new XAttribute("Order", match.Order),
                            new XAttribute("Description", match.Description),
                            new XAttribute("Player1", match.Player1),
                            new XAttribute("Player2", match.Player2),
                            new XAttribute("Winner", match.Winner)
                        ))
                ));
                xEle.Save(saveWorldDialog.FileName);
                openedWorld = saveWorldDialog.FileName;
                requireSave = false;
            }
        }

        private void fileUpdateButton_Click(object sender, EventArgs e)
        {
            if (scoresChanged)
                TabControl1_SelectedIndexChanged(sender, e);

            if (openedWorld != "")
            {
                var xEle = new XElement(
                        new XElement("SK92",
                            new XElement("Settings",
                                new XAttribute("Multiplier", multiplier),
                                new XAttribute("MinMatches", minMatches),
                                new XAttribute("Decay", decay),
                                new XAttribute("DecayValue", decayValue)
                            ),
                            new XElement("Players", from player in playerList
                                                    select new XElement("Player",
                                                        new XAttribute("Name", player.Name),
                                                        new XAttribute("Team", player.Team),
                                                        new XAttribute("Invisible", player.Invisible),
                                                        new XAttribute("Characters", player.Characters),
                                                        new XAttribute("Alts", player.AltsString)
                                                        )),
                            new XElement("Matches", from match in matchList
                                                    select new XElement("Match",
                                                        new XAttribute("ID", match.ID),
                                                        new XAttribute("Timestamp", match.Timestamp.ToString()),
                                                        new XAttribute("Order", match.Order),
                                                        new XAttribute("Description", match.Description),
                                                        new XAttribute("Player1", match.Player1),
                                                        new XAttribute("Player2", match.Player2),
                                                        new XAttribute("Winner", match.Winner)
                                                        ))
                    ));
                xEle.Save(openedWorld);
                requireSave = false;
            }
            else
            {
                fileSaveButton_Click(sender, e);
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
                    foreach (String s in importer.getNewAlts())
                    {
                        String name = s.Split('\t')[0];
                        String alt = s.Split('\t')[1];
                        foreach (Person p in playerList)
                        {
                            if (p.Name == name)
                            {
                                if (!p.Alts.Contains(alt))
                                    p.Alts.Add(alt);

                                break;
                            }
                        }
                    }

                    rebuildPlayerSelection();

                    matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
                    Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
                    buildHistory();
                }
            }
        }

        private void fileImportChallongeButton_Click(object sender, EventArgs e)
        {
            SKChallongeLoader skChallongeLoader = new SKChallongeLoader();
            if (skChallongeLoader.ShowDialog() == DialogResult.OK)
            {
                SKChallongeImporter importer = new SKChallongeImporter();

                try
                {
                    importer.importChallonge(skChallongeLoader.getAPIKey(), skChallongeLoader.getSubDomain(), 
                                             skChallongeLoader.getTournamentName(), playerList);
                }
                catch (ChallongeApiException ex)
                {
                    MessageBox.Show(this, "The following error(s) were encountered:\n" + ex.Errors.Aggregate((one, two) => one + "\n" + two), "Challonge Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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
                    foreach (String s in importer.getNewAlts())
                    {
                        String name = s.Split('\t')[0];
                        String alt = s.Split('\t')[1];
                        foreach (Person p in playerList)
                        {
                            if (p.Name == name)
                            {
                                if (!p.Alts.Contains(alt))
                                    p.Alts.Add(alt);

                                break;
                            }
                        }
                    }

                    rebuildPlayerSelection();

                    matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
                    Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
                    buildHistory();
                }
            }
        }

        private void fileImportGlickoButton_Click(object sender, EventArgs e)
        {
            importFileDialog.Filter = "GLK files (*.glk)|*.glk|CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            if (importFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamReader sr = new StreamReader(importFileDialog.FileName);

                using (sr)
                {
                    String line;
                    String[] row;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Equals("--==--"))
                            break;

                        row = line.Split(',');
                        if (row.Length < 8)
                            return;

                        Person p = new Person();
                        p.Team = row[0];
                        p.Name = row[1];

                        Boolean alreadyExists = false;
                        foreach (Person pl in playerList)
                        {
                            if (pl.Name == p.Name)
                            {
                                alreadyExists = true;
                                break;
                            }
                        }

                        if (!alreadyExists)
                        {
                            playerList.Add(p);
                        }
                    }

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!(line.Equals("//reset,//reset") || line.Equals("//reset,//reset,")))
                        {
                            if (line.Equals("--==--"))
                                break;
                            row = line.Split(',');
                            Match m = new Match();
                            m.Player1 = row[0];
                            m.Player2 = row[1];
                            m.Winner = 1;
                            if (row.Length == 3)
                                m.Description = row[2];
                            m.Order = Toolbox.getNewMatchNumber(matchList, DateTime.Today);

                            matchList.Add(m);
                        }
                    }
                }

                rebuildPlayerSelection();

                matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
                Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
                buildHistory();
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

            modifyHideCheck.Checked = person.Invisible;

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
            modifyWLDBox.Text = person.Wins + " -- " + person.Losses + " -- " + person.Draws + " (" + person.WinPercent + "%)";

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
                    p1.Invisible = modifyHideCheck.Checked;

                    p1.Alts = modifyAltsBox.Text.Split(';').ToList();
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
            requireSave = true;

            buildHistory();
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
            requireSave = true;
        }

        private void modifyCombineSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modifySelector.Text != modifyCombineSelector.Text && modifySelector.Text.Length > 0 && modifyCombineSelector.Text.Length > 0)
            {
                modifyCombineButton.Enabled = true;
            }
        }

        private void modifyCombineButton_Click(object sender, EventArgs e)
        {
            if (modifyCombineButton.Enabled)
            {
                combinePlayers(modifySelector.Text, modifyCombineSelector.Text);
                modifyCombineSelector.Text = "";
            }
        }

        private void modifyNameBox_TextChanged(object sender, EventArgs e)
        {
            modifyPlayerButton.Enabled = true;
            if (modifyNameBox.Text.Length == 0)
            {
                modifyPlayerButton.Enabled = false;
                return;
            }
            if (modifyNameBox.Text != modifySelector.Text)
            {
                foreach (Person person in playerList)
                {
                    if (modifyNameBox.Text == person.Name)
                    {
                        modifyPlayerButton.Enabled = false;
                    }
                }
            }
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

            if (historyMoveDatePicker.Value != historyDatePicker.Value)
                historyMoveTourneyButton.Enabled = true;
            else
                historyMoveTourneyButton.Enabled = false;
        }

        private void historyViewAllCheck_CheckedChanged(object sender, EventArgs e)
        {
            buildHistory();
        }

        private void historyGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            historyApplyButton.Enabled = true;
            scoresChanged = true;
            requireSave = true;
        }

        private void historyGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (historyGridView.SelectedRows.Count > 0)
            {
                historyDatePicker.Value = DateTime.Parse(historyGridView.SelectedRows[0].Cells[4].Value.ToString());
                historyDeleteMatchButton.Enabled = true;
                historyDeleteTournamentButton.Enabled = true;
                if (historyDatePicker.Value != historyMoveDatePicker.Value)
                    historyMoveTourneyButton.Enabled = true;
            }
            else
            {
                historyDeleteMatchButton.Enabled = false;
                historyDeleteTournamentButton.Enabled = false;
            }
        }

        private void historyMoveDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (historyMoveDatePicker.Value != historyDatePicker.Value)
                historyMoveTourneyButton.Enabled = true;
            else
                historyMoveTourneyButton.Enabled = false;
        }

        private void historyMoveTourneyButton_Click(object sender, EventArgs e)
        {
            String tourneyName = historyGridView.SelectedRows[0].Cells[3].Value.ToString();
            foreach (Match m in matchList)
            {
                if (m.Description == tourneyName)
                    m.Timestamp = historyMoveDatePicker.Value;
            }

            historyDatePicker.Value = historyMoveDatePicker.Value;
            matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
            // Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
            requireSave = true;
            scoresChanged = true;
            // buildHistory();
        }

        private void historyApplyButton_Click(object sender, EventArgs e)
        {
            if (historyApplyButton.Enabled)
            {
                foreach (Match m in (BindingList<Match>)matchBindingSource.DataSource)
                {
                    foreach (Match m2 in matchList)
                    {
                        if (m.ID == m2.ID)
                        {
                            m2.Winner = m.Winner;
                            m2.Order = m.Order;

                            break;
                        }
                    }
                }

                historyApplyButton.Enabled = false;

                historyMoveDatePicker.Value = historyDatePicker.Value;
                matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
                // Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
                requireSave = true;
                scoresChanged = true;
                // buildHistory();
            }
        }

        private void historyDeleteMatchButton_Click(object sender, EventArgs e)
        {
            matchList.Remove((Match) historyGridView.SelectedRows[0].DataBoundItem);
            matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
            // Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
            requireSave = true;
            scoresChanged = true;
            // buildHistory();
        }

        private void historyDeleteTournamentButton_Click(object sender, EventArgs e)
        {
            String tourneyName = historyGridView.SelectedRows[0].Cells[3].Value.ToString();
            Boolean allDeleted = false;
            while (!allDeleted)
            {
                allDeleted = true;
                foreach (Match m in matchList)
                {
                    if (m.Description == tourneyName)
                    {
                        matchList.Remove(m);
                        allDeleted = false;
                        break;
                    }
                }
            }

            matchList = matchList.OrderBy(s => s.Timestamp).ThenBy(s => s.Order).ToList();
            // Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue);
            requireSave = true;
            scoresChanged = true;
            // buildHistory();
        }

        // ------------------------------------
        // Leaderboard Tab
        // ------------------------------------
        private void leaderboardDatePicker_ValueChanged(object sender, EventArgs e)
        {
            Toolbox.recalcMatches(playerList, matchList, startMu, startSigma, multiplier, decay, decayValue, leaderboardDatePicker.Value);
            buildHistory();
        }

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

        // ------------------------------------
        // Settings Tab
        // ------------------------------------
        private void settingsMultiplierBox_TextChanged(object sender, EventArgs e)
        {
            scoresChanged = true;
            requireSave = true;
        }

        private void settingsMatchesBox_TextChanged(object sender, EventArgs e)
        {
            scoresChanged = true;
            requireSave = true;
        }

        private void settingsDecayIntBox_TextChanged(object sender, EventArgs e)
        {
            scoresChanged = true;
            requireSave = true;
        }

        private void settingsCheckDecayVal()
        {
            if (settingsDecayDaily.Checked)
            {
                settingsDecayIntBox.Enabled = true;
                decay = 1;
            }
            else if (settingsDecayWeekly.Checked)
            {
                settingsDecayIntBox.Enabled = true;
                decay = 2;
            }
            else if (settingsDecayMonthly.Checked)
            {
                settingsDecayIntBox.Enabled = true;
                decay = 3;
            }
            else if (settingsDecayYearly.Checked)
            {
                settingsDecayIntBox.Enabled = true;
                decay = 4;
            }
            else if (settingsDecayNever.Checked)
            {
                settingsDecayIntBox.Enabled = false;
                decay = 0;
            }

            scoresChanged = true;
            requireSave = true;
        }

        private void settingsDecayDaily_CheckedChanged(object sender, EventArgs e)
        {
            settingsCheckDecayVal();
        }

        private void settingsDecayWeekly_CheckedChanged(object sender, EventArgs e)
        {
            settingsCheckDecayVal();
        }

        private void settingsDecayMonthly_CheckedChanged(object sender, EventArgs e)
        {
            settingsCheckDecayVal();
        }

        private void settingsDecayYearly_CheckedChanged(object sender, EventArgs e)
        {
            settingsCheckDecayVal();
        }

        private void settingsDecayNever_CheckedChanged(object sender, EventArgs e)
        {
            settingsCheckDecayVal();
        }
    }
}
