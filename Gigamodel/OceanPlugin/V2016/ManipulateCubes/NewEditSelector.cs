using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManipulateCubes
{
    public partial class NewEditSelector : UserControl
    {

        public event EventHandler SelectionChanged;
        public event EventHandler NewClicked;

        public NewEditSelector()
        {
            InitializeComponent();

            newName.Visible = true;
            selector.Visible = true;
            newButton.Checked = true;

            this.selector.AllowDrop = false;
            this.selector.DropDownStyle = ComboBoxStyle.DropDownList;  //prevent user entering text.
            this.selector.SelectedIndexChanged += new System.EventHandler(this.selectionChanged);
            this.editButton.CheckedChanged += new System.EventHandler(this.editButton_CheckedChanged);

        }


        public string Title
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        public void UpdateSelector(string s)
        {
            //dont delete the combobox or chaneg where it points to, just update whats needed. 
            List<string> currentNames = getComboBoxNames().ToList();
            int index = currentNames.IndexOf(s);

            if (index >= 0)
            {
                editButton.Checked = true;
                selector.SelectedIndex = index;

            }
            else if ((s != null) && (s != string.Empty))
            {
                selector.Items.Add(s);
                index = selector.Items.Count - 1;
                selector.SelectedIndex = index;
                editButton.Checked = true;
            }
            else
            {
                editButton.Checked = false;
            }

        }

        public string[] ModelNames
        {
            get { return getComboBoxNames().ToArray(); }

            set
            {
                selector.Items.Clear();
                //try this selector.Items.AddRange(value);
                foreach (string s in value) selector.Items.Add(s);
                if (selector.Items.Count > 0) selector.SelectedIndex = 0;

            }
        }

        public string SelectedName
        {
            get
            {
                if (newButton.Checked)
                {
                    return newName.Text;
                }
                else
                {
                    string txt = selector.Text;
                    return txt;
                }
            }
        }

        public bool IsNewSelected { get { return newButton.Checked; } }

        private void editButton_CheckedChanged(object sender, EventArgs e)
        {
            selector.Enabled = editButton.Checked;
            newName.Enabled = !(editButton.Checked);

            if (newButton.Checked) newName.Text = string.Empty;

            if (NewClicked != null)
                NewClicked(this, EventArgs.Empty);
        }

        private void selectionChanged(object sender, EventArgs e)
        {
            if (SelectionChanged != null)
                SelectionChanged(this, new EventArgs());

        }

        private List<string> getComboBoxNames()
        {
            List<string> l = new List<string>();
            foreach (object x in selector.Items) l.Add(x.ToString());
            return l;
        }

    }
}
