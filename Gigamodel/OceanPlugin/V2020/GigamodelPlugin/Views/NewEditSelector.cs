using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Gigamodel
{
    public partial class NewEditSelector : UserControl
    {
        public event EventHandler SelectionChanged;

        public event EventHandler NewClicked;

        public event EventHandler DeleteClicked;

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

            newButton.Checked = false;
            newButton.Checked = true;
        }

        public Image DeleteImage
        {
            get { return deleteButton.Image; }
            set { deleteButton.Image = value; }
        }

        public string Title
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        public void UpdateSelector( string s )
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

        public List<string> ModelNames
        {
            get { return getComboBoxNames(); }

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

        public bool IsNewSelected
        {
            get { return newButton.Checked; }

            set { newButton.Checked = value; }
        }

        private void editButton_CheckedChanged( object sender, EventArgs e )
        {
            selector.Enabled = editButton.Checked;
            newName.Enabled = !(editButton.Checked);

            if (newButton.Checked) newName.Text = string.Empty;

            if (NewClicked != null)
                NewClicked(this, EventArgs.Empty);
        }

        private void selectionChanged( object sender, EventArgs e )
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

        private void deleteButtonClicked( object sender, EventArgs e )
        {
            DeleteClicked?.Invoke(this, EventArgs.Empty);
        }
    }

    //public partial class NewEditSelector : UserControl
    //{
    //    public event EventHandler SelectionChanged;
    //    public event EventHandler NewClickedOrUnClicked;

    //    private string _lastSelected;
    //    public NewEditSelector()
    //    {
    //        InitializeComponent();
    //        _lastSelected = string.Empty;
    //        this.selector.AllowDrop = false;
    //        this.selector.DropDownStyle = ComboBoxStyle.DropDownList;  //prevent user entering text.
    //        this.selector.SelectedIndexChanged += new System.EventHandler(this.selectionChanged);
    //        this.editButton.CheckedChanged += new System.EventHandler(this.checkedChanged);
    //    }

    //   public void AddIfMissingAndSelect(string s)
    //    {
    //        List<string> currentNames = getComboBoxNames().ToList();
    //        int index = currentNames.IndexOf(s);

    //        //from older version
    //        if (index >= 0)
    //        {
    //            editButton.Checked = true;
    //            selector.SelectedIndex = index;

    //        }
    //        else if ((s != null) && (s != string.Empty))
    //        {
    //            selector.Items.Add(s);
    //            index = selector.Items.Count - 1;
    //            selector.SelectedIndex = index;
    //            editButton.Checked = true;
    //        }
    //        else
    //        {
    //            editButton.Checked = false;
    //        }

    //        /*
    //        if (index < 0)
    //        {
    //            selector.Items.Add(s);
    //            index = selector.Items.Count - 1;
    //        }

    //        selector.SelectedIndex = index;
    //        editButton.Checked = true;
    //        */
    //    }

    //    public List<string> ModelNames
    //    {
    //        get { return getComboBoxNames(); }

    //        set //if there anrent names different to the actual, the selection is not changed.
    //        {
    //            if ((value == null) || (value.Count < 1))
    //            {
    //                selector.Items.Clear();
    //                newButton.Checked = true;
    //                return;
    //            }
    //            List<string> l = getComboBoxNames();

    //            //we go through this pain, just to update thye names but keep the
    //            //original selected item untouched and not triggering uneeded events.
    //            //add new names.
    //            object lastNewName = null;
    //            foreach (string s in value)
    //            if (!(l.Contains(s))) { selector.Items.Add(s); lastNewName = selector.Items[selector.Items.Count - 1]; }

    //            //remove old un-used names.
    //            for (int n = 0; n < selector.Items.Count; n++)
    //            {
    //                if (!(value.Contains(selector.Items[n].ToString())))
    //                {
    //                    selector.Items.RemoveAt(n);
    //                    n -= 1;
    //                }
    //            }

    //            if (lastNewName != null) //return;
    //            selector.SelectedIndex = selector.Items.IndexOf(lastNewName);
    //        }
    //    }

    //    public string Title
    //    {
    //        get { return titleLabel.Text; }
    //        set { titleLabel.Text = value; }
    //    }

    //    public string SelectedName
    //    {
    //        get
    //        {
    //            return newButton.Checked ? newName.Text : selector.Text;
    //        }
    //    }

    //    public bool IsNewSelected { get { return newButton.Checked; } set {  editButton.Checked = !(value); } }

    //    private void checkedChanged(object sender, EventArgs e)
    //    {
    //        if (editButton.Checked)
    //        {
    //            string lastKnownName = _lastSelected;
    //            selector.Enabled = true;
    //            if (ModelNames.Count() > 0)
    //            {
    //                //selector.SelectedIndex = (Math.Max(0, ModelNames.IndexOf(_lastSelected)));
    //                //SelectionChanged?.Invoke(this, new EventArgs());
    //                selectionChanged(null, EventArgs.Empty);
    //            }
    //        }
    //        else
    //        {
    //            selector.Enabled = false;
    //            newName.Enabled = !(editButton.Checked);
    //            newName.Text = string.Empty;

    //        }

    //        if (NewClickedOrUnClicked != null)
    //            NewClickedOrUnClicked(this, EventArgs.Empty);
    //    }

    //    private void selectionChanged(object sender, EventArgs e)
    //    {
    //        _lastSelected = SelectedName;
    //        if (editButton.Checked)
    //            if (SelectionChanged != null)
    //            SelectionChanged?.Invoke(this, new EventArgs());

    //    }

    //    private List<string> getComboBoxNames()
    //    {
    //        List<string> l = new List<string>();
    //        foreach (object x in selector.Items) l.Add(x.ToString());
    //        return l;
    //    }
    //}
}