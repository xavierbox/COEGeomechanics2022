using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.AllowDrop = true;
            this.webBrowser1.AllowWebBrowserDrop = false;

            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.form_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.form_DragEnter);

        }


        public void test()
        {


        }

        private void form_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void form_DragDrop(object sender, DragEventArgs e)
        {

            string objs = (string)e.Data.GetData(DataFormats.Text);
            string[] objs_str = objs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

     
        }













        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {

            string objs = (string)e.Data.GetData(DataFormats.Text);
            string[] objs_str = objs.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        }
    }
}

