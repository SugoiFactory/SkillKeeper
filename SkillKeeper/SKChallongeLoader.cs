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
        public SKChallongeLoader()
        {
            InitializeComponent();
        }

        public String getAPIKey()
        {
            return apiKeyBox.Text;
        }

        public String getSubDomain()
        {
            return subDomainBox.Text;
        }

        private void authButton_Click(object sender, EventArgs e)
        {
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
