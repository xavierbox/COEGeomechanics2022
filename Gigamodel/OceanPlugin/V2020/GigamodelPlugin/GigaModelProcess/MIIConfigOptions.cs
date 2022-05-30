using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gigamodel.GigaModelProcess
{
    public partial class MIIConfigOptions : Form
    {
        public MIIConfigOptions()
        {
            InitializeComponent();
            this.AcceptButton = button1;
        }

        private void button1_Click( object sender, EventArgs e )
        {
            this.DialogResult = DialogResult.OK;
        }

        public Dictionary<string, List<KeyValuePair<string, bool>>> GetOptions()
        {
            Dictionary<string, List<KeyValuePair<string, bool>>> ret = new Dictionary<string, List<KeyValuePair<string, bool>>>();

            List<KeyValuePair<string, bool>> degugOptions = new List<KeyValuePair<string, bool>>();

            foreach (Control c in debugPanel.Controls)
            {
                if (c is CheckBox)
                    degugOptions.Add(new KeyValuePair<string, bool>(((CheckBox)c).Text, ((CheckBox)c).Checked));
            }
            ret.Add("NOECHO", degugOptions);

            List<KeyValuePair<string, bool>> resOptions = new List<KeyValuePair<string, bool>>();
            foreach (Control c in resultsPanel.Controls)
            {
                if (c is CheckBox)
                    resOptions.Add(new KeyValuePair<string, bool>(((CheckBox)c).Text, ((CheckBox)c).Checked));
            }
            ret.Add("RESULTS", resOptions);

            List<KeyValuePair<string, bool>> commands = new List<KeyValuePair<string, bool>>();
            foreach (Control c in commandsPanel.Controls)
            {
                if (c is CheckBox)
                {
                    ret.Add(((CheckBox)c).Text, new List<KeyValuePair<string, bool>>());
                }
            }
            //  ret.Add("RESULTS", resOptions);

            //List<KeyValuePair<string, bool>> commandsOptions = new List<KeyValuePair<string, bool>>();
            //foreach (Control c in commandsOptions.Controls)
            //{
            //    if (c is CheckBox)
            //        commandsOptions.Add(new KeyValuePair<string, bool>(((CheckBox)c).Text, ((CheckBox)c).Checked));
            //}
            //ret.Add("RESULTS", resOptions);

            return ret;
        }

        private void MIIConfigOptions_Load( object sender, EventArgs e )
        {
        }
    }
}