using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChangeFileNames
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = "File Renaming for GFM";

        }


        private string GetFolder()
        {
            string selected = string.Empty;
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    selected = fbd.SelectedPath;
                    textBox1.Text = selected;
                }
            }


            return selected;
        }

        private void Folder_Click(object sender, EventArgs e)
        {
            string mainFolder = GetFolder();

            string[] folders = Directory.GetDirectories(mainFolder);
            string[] files = Directory.GetFiles(mainFolder);

            DirectoryInfo target = Directory.CreateDirectory(Path.Combine(mainFolder, "Renamed"));

            //create present day folder 
           


            ////rename all the folders 
            //foreach (string d in folders)
            //{
            //    string name = d; 
            //    if (d.Contains("_"))
            //    {
            //        name = d.Replace("_","");
            //    }
            //    DirectoryInfo i = new DirectoryInfo(name);
            //    string newFolder = Path.Combine(target.FullName, i.Name);
            //    DirectoryInfo d2 = Directory.CreateDirectory( newFolder );
            //}
            int count = 0;
            progressBar1.Visible = true;
            label1.Visible = true;
            progressBar1.Show();
            label1.Show();
            progressBar1.Value = 0;
            label1.Text = "";

            foreach (string f in files)
            {
                count++;

                progressBar1.Value = (int)(100.0 * count / files.Count());

                label1.Text = "Processing file " + f;

                string name = f;
                if (f.Contains("_"))
                {
                    name = f.Replace("_", "");
                }
                DirectoryInfo i = new DirectoryInfo(name);
                string newFile = Path.Combine(target.FullName, i.Name);
                try
                {
                    File.Copy(f, newFile);
                }
                catch
                {
                    ;
                }
                try
                {
                    string newDirectoryName = newFile.Substring(0, newFile.LastIndexOf("Deposited"));
                    DirectoryInfo d2 = Directory.CreateDirectory(newDirectoryName);

                    //newFile = newFile.Replace("Deposited", "Deposited_");


                    File.Move(newFile, Path.Combine(d2.FullName, Path.GetFileName(newFile.Replace("Deposited", "Deposited_"))));
                }

                catch (Exception ew)
                {

                    string why = ew.ToString();
                }
                //try
                //{
                //    string newDirectoryName= newFile.Substring(0, newFile.LastIndexOf("Deposited"));

                //    //create the folder for this step 
                //    DirectoryInfo d2 = Directory.CreateDirectory(newDirectoryName);


                //}
                //catch
                //{
                //}




            }


            //copy everything called PresentDay to that folder 
            DirectoryInfo presentDay = Directory.CreateDirectory(Path.Combine(target.FullName, "PresentDay"));
            files = Directory.GetFiles(target.FullName);
            foreach (string f in files)
            {
                if(f.ToLower().Contains("presentday"))
                File.Move(f, Path.Combine(presentDay.FullName, Path.GetFileName(f.Replace("PresentDay","PresentDay_"))));
            }

            ;



            label1.Text = "Processing finished";
            //progressBar1.Visible = false;

        }

        private void Apply_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ParentForm?.Close();
        }
    }
}
