using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SkillKeeper
{
    public partial class SKChallongeLoader : Form
    {
        private static string apiKey = string.Empty;
        private static string subdomain = string.Empty;
        private static string tournamentName = string.Empty;

        public SKChallongeLoader()
        {
            InitializeComponent();
        }

        private void SKChallongeLoader_Load(object sender, EventArgs e)
        {
            //Load apikey and subdomain on load. These will be populated if they were previously set
            apiKeyBox.Text = apiKey;
            subDomainBox.Text = subdomain;
            tournamentNameBox.Text = tournamentName;
        }

        public String getAPIKey()
        {
            return apiKeyBox.Text;
        }

        public String getSubDomain()
        {
            return subDomainBox.Text;
        }

        public String getTournamentName()
        {
            return tournamentName;
        }

        private void authButton_Click(object sender, EventArgs e)
        {
            //Store api key and subdomain inputs for use if import form is closed and re-opened
            apiKey = apiKeyBox.Text;
            subdomain = subDomainBox.Text;
            tournamentName = tournamentNameBox.Text;

            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void apiKeyBox_TextChanged(object sender, EventArgs e)
        {
            if (apiKeyBox.Text.Length > 0)
                authButton.Enabled = true;
            else
                authButton.Enabled = false;
        }
    }
}
