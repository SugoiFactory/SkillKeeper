using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Windows.Forms;

namespace SkillKeeper
{
    public partial class SKTioImporter : Form
    {
        private List<Match> importMatches = new List<Match>();
        private List<Person> importPlayers = new List<Person>();
        private List<Person> currentPlayers = new List<Person>();
        private List<String> playerNames = new List<String>();
        private List<ImportPlayer> tioList = new List<ImportPlayer>();
        private List<String> eventList = new List<String>();
        private String tournamentName = "";
        private XElement xEle = null;

        private List<String> addAlts = new List<String>();

        private Match finals1 = new Match();
        private Match finals2 = new Match();
        private Boolean hasFinals1 = false;
        private Boolean hasFinals2 = false;

        public SKTioImporter()
        {
            InitializeComponent();
            eventDatePicker.Value = DateTime.Today;
            sKLinkDataGridViewTextBoxColumn.DataSource = playerNames;
            playerNames.Add(" << Create New Player >>");
        }

        // Import the TIO file, get a list of singles events within that TIO file, and update the list of players found in the first event (which is initially selected).
        public void importFile(String fileName, List<Person> playerList)
        {
            xEle = XElement.Load(fileName);

            try
            {
                eventDatePicker.Value = DateTime.ParseExact(xEle.Element("EventList").Element("Event").Element("StartDate").Value, "MM/dd/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (System.FormatException e)
            {
                Console.WriteLine("Invalid date, skipping.");
                Console.WriteLine(e.StackTrace);
            }

            tournamentName = xEle.Element("EventList").Element("Event").Element("Name").Value;
            foreach (XElement tioEvent in xEle.Element("EventList").Element("Event").Element("Games").Elements("Game"))
            {
                if (tioEvent.Element("GameType").Value == "Singles")
                    eventList.Add(tioEvent.Element("Name").Value);
            }
            eventSelector.DataSource = eventList;

            foreach (Person p in playerList)
            {
                currentPlayers.Add(p);
                playerNames.Add(p.Name);
            }
            playerNames.Sort();

            updatePlayerList();
        }

        // Update the list of players found in the selected event.
        private void updatePlayerList()
        {
            List<String> selectedEventIDs = new List<String>();
            foreach (XElement tioEvent in xEle.Element("EventList").Element("Event").Element("Games").Elements("Game"))
            {
                if (tioEvent.Element("Name").Value == eventSelector.Text)
                {
                    foreach (XElement entrant in tioEvent.Element("Entrants").Elements("Entrant"))
                    {
                        selectedEventIDs.Add(entrant.Element("PlayerID").Value);
                    }
                    foreach (XElement entrant in tioEvent.Element("Entrants").Elements("PlayerID"))
                    {
                        selectedEventIDs.Add(entrant.Value);
                    }
                }
            }

            tioList.Clear();
            foreach (XElement player in xEle.Element("PlayerList").Element("Players").Elements("Player"))
            {
                ImportPlayer p = new ImportPlayer();
                p.ID = player.Element("ID").Value;
                p.Name = player.Element("Nickname").Value;
                p.SKLink = getMatch(p.Name);

                foreach (String s in selectedEventIDs)
                {
                    if (s == p.ID)
                    {
                        tioList.Add(p);
                        break;
                    }
                }
            }

            importPlayerBindingSource.DataSource = new BindingList<ImportPlayer>(tioList);
        }

        // Update the associated import player whenever a link is manually set.
        private void importPlayerList_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            foreach (ImportPlayer p in tioList)
            {
                if (p.ID == (String) importPlayerList.CurrentRow.Cells[0].Value)
                {
                    p.SKLink = (String) importPlayerList.CurrentRow.Cells[2].Value;
                }
            }
        }

        // Method to attempt to find a match between a player in the TIO file and a player already in the World. If it cannot find a player with the same name,
        // it then attempts to search through the lists of alternate names for each player.
        private String getMatch(String playerName)
        {
            String result = " << Create New Player >>";
            Boolean foundMatch = false;
            foreach (Person person in currentPlayers)
            {
                if (person.Name.ToUpper() == playerName.ToUpper() || (playerName.ToUpper().StartsWith(person.Team.ToUpper()) && playerName.ToUpper().EndsWith(person.Name.ToUpper()) && person.Name.Length > 0 && person.Team.Length > 0))
                {
                    foundMatch = true;
                    result = person.Name;
                    break;
                }
            }
            if (!foundMatch)
            {
                foreach (Person person in currentPlayers)
                {
                    foreach (String altName in person.Alts)
                    {
                        if (altName.ToUpper() == playerName.ToUpper() || (playerName.ToUpper().StartsWith(person.Team.ToUpper()) && playerName.ToUpper().EndsWith(altName.ToUpper()) && altName.Length > 0 && person.Team.Length > 0))
                        {
                            result = person.Name;
                        }
                    }
                }
            }

            return result;
        }

        // Rebuild the list of players in the event whenever the event selector is used.
        private void eventSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            updatePlayerList();
        }

        // Cancel
        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Confirm Import. Process all matches in selected event.
        private void importButton_Click(object sender, EventArgs e)
        {
            foreach (ImportPlayer p in tioList)
            {
                if (p.SKLink == " << Create New Player >>")
                {
                    if (!playerNames.Contains(p.Name))
                    {
                        Person person = new Person();
                        person.Name = p.Name;
                        importPlayers.Add(person);
                    }
                }
                else if (p.SKLink != p.Name)
                {
                    addAlts.Add(p.SKLink + "\t" + p.Name);
                }
            }

            foreach (XElement tioEvent in xEle.Element("EventList").Element("Event").Element("Games").Elements("Game"))
            {
                if (tioEvent.Element("Name").Value == eventSelector.Text)
                {
                    if (tioEvent.Element("Bracket").Element("Pools") != null)
                        foreach (XElement pool in tioEvent.Element("Bracket").Element("Pools").Elements("Pool"))
                            foreach (XElement match in pool.Element("Matches").Elements("Match"))
                                createMatch(match);
                    else foreach (XElement match in tioEvent.Element("Bracket").Element("Matches").Elements("Match"))
                            createMatch(match);
                }
            }
            if (hasFinals1)
                importMatches.Add(finals1);
            if (hasFinals2)
                importMatches.Add(finals2);

            this.Close();
        }

        // Create a match out of XML data. If not grand finals, add it immediately to the list of matches to import. Otherwise, store for adding at the end.
        private void createMatch(XElement match)
        {
            Match m = new Match();

            m.Description = tournamentName + " - " + eventSelector.Text;
            m.Timestamp = eventDatePicker.Value;

            foreach (ImportPlayer p in tioList)
            {
                if (p.ID == match.Element("Player1").Value)
                {
                    if (p.SKLink == " << Create New Player >>")
                        m.Player1 = p.Name;
                    else
                        m.Player1 = p.SKLink;
                }
                if (p.ID == match.Element("Player2").Value)
                {
                    if (p.SKLink == " << Create New Player >>")
                        m.Player2 = p.Name;
                    else
                        m.Player2 = p.SKLink;
                }
            }

            if (m.Player1 != null && m.Player2 != null)
            {
                if (match.Element("Player1").Value == match.Element("Winner").Value)
                    m.Winner = 1;
                else if (match.Element("Player2").Value == match.Element("Winner").Value)
                    m.Winner = 2;
                else
                    return;

                m.ID = Guid.NewGuid().ToString("N");

                if (match.Element("IsChampionship") != null || match.Element("IsSecondChampionship") != null)
                {
                    if (match.Element("IsChampionship") != null && match.Element("IsChampionship").Value == "True")
                    {
                        hasFinals1 = true;
                        finals1 = m;
                    }
                    else if (match.Element("IsSecondChampionship") != null && match.Element("IsSecondChampionship").Value == "True")
                    {
                        hasFinals2 = true;
                        finals2 = m;
                    }
                    else
                        importMatches.Add(m);
                }
                else
                    importMatches.Add(m);
            }
        }

        public List<Person> getImportPlayers()
        {
            return importPlayers;
        }

        public List<Match> getImportMatches()
        {
            return importMatches;
        }

        public List<String> getNewAlts()
        {
            return addAlts;
        }
    }
}
