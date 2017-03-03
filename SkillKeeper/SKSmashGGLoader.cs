using SmashGGApiWrapper;
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
    public partial class SKSmashGGLoader : Form
    {
        public string tournament { get; set; }
        public SKSmashGGLoader()
        {
            InitializeComponent();
            buttonSubmit.DialogResult = DialogResult.OK;
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            tournament = urlText.Text;
            this.Close();
        }
    }
}
