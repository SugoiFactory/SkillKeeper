using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SmashGGApiWrapper;

namespace SkillKeeper
{
    public partial class SKSmashGGEventSelector : Form
    {
        private SmashGGPortal gg;
        private IEnumerable<Event> events;
        private IEnumerable<Phase> phases;
        private IEnumerable<Group> groups;
        public bool importCompleteBracket { get; set; }
        public int eventID { get; set; }
        public int phaseID { get; set; }
        public int groupID { get; set; }
        public SKSmashGGEventSelector()
        {
            InitializeComponent();
            submitButton.DialogResult = DialogResult.OK;
        }
        private List<string> getEventNames(IEnumerable<Event> get)
        {
            List<string> eventNames = new List<string>();
            foreach(Event got in get)
            {
                eventNames.Add(got.name);
            }
            return eventNames;
        }

        internal void importTourney(string tournament)
        {
            gg = new SmashGGPortal(tournament);
            events = gg.GetEvents();
            eventSelector.DataSource = getEventNames(events);
        }
        private void eventSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            Event selected = events.Single(c => c.name == eventSelector.Text);
            eventID = selected.id;
            phases = gg.GetPhases(selected.id);
            phaseSelector.DataSource = phases.Select(p => p.name).ToList();
        }

        private void importFullBracket_CheckedChanged(object sender, EventArgs e)
        {
            if (importFullBracket.Checked)
            {
                bracketSelector.Enabled = false;
                phaseSelector.Enabled = false;
            }
            else
            {
                bracketSelector.Enabled = true;
                phaseSelector.Enabled = true;
            }
        }

        private void phaseSelector_SelectedIndexChanged(object sender, EventArgs e)
        {

            Phase selected = phases.Single(c => c.name == phaseSelector.Text);
            phaseID = selected.id;
            groups = gg.GetGroupsFromPhase(selected.id);
            bracketSelector.DataSource = groups.Select(g => g.displayIdentifier).ToList();
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            groupID = groups.Single(g => g.displayIdentifier == bracketSelector.Text).id;
            importCompleteBracket = importFullBracket.Checked;
            this.Close();
        }
    }
}
