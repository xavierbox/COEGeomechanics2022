using System;
using System.Windows.Forms;

namespace RunFrachiteIndependent
{
    public partial class Form1 : Form
    {
        private FrachiteData.FrachiteRunner runner = new FrachiteData.FrachiteRunner();

        public Form1()
        {
            InitializeComponent();
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            this.binaryButton.Click += new System.EventHandler(this.binaryButton_Click);
            this.optionsButton.Click += new System.EventHandler(this.optionsButton_Click);
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

            runner.ThresholdReached += ( s, evt ) =>
            {
                this.progressBar1.Value = evt.Progress;
            };
        }

        private void binaryButton_Click( object sender, EventArgs e )
        {
            using (OpenFileDialog folderDlg = new OpenFileDialog())
            {
                if (folderDlg.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = folderDlg.FileName;
                }
            }
        }

        private void optionsButton_Click( object sender, EventArgs e )
        {
            using (OpenFileDialog folderDlg = new OpenFileDialog())
            {
                if (folderDlg.ShowDialog() == DialogResult.OK)
                    textBox2.Text = folderDlg.FileName;
            }
        }

        private void runButton_Click( object sender, EventArgs e )
        {
            this.progressBar1.Value = 0;
            runner.Folder = System.IO.Path.GetDirectoryName(textBox1.Text);
            runner.ParseInputData(textBox1.Text, textBox2.Text);
            if (!runner.RunSimulation())
            {
                MessageBox.Show("Humm...something went wrong. Cant run the simulation. The data must be wrong");
                return;
            }

            if (!runner.SerializeLastResults())
            {
                MessageBox.Show("Somehow...the results cannot be serialized. Sorry");
            }
        }

        private void cancelButton_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void Form1_Load( object sender, EventArgs e )
        {
        }

        private void modelNameTextBox_TextChanged( object sender, EventArgs e )
        {
            this.runner.ModelName = modelNameTextBox.Text;
        }
    }
}