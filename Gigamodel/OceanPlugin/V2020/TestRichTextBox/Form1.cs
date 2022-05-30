using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestRichTextBox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.richTextBox1.Rtf = @"{\rtf1\ansi \b The Geometry of the Model Cannot be Determined \b0.\n Please }";

 

//var sb = new StringBuilder();
//            sb.Append(@"{\rtf1\ansi");
//            sb.Append(@"\b The Geometry of the Model Cannot be Determined: \b0 \n");
//            sb.Append(@" \line ");
//            sb.Append(@" Either a seismic cube was deleted from the project or not all the files exported are available now in the simulation folder");
//            sb.Append(@" \line ");
//            sb.Append(@"\b To fix this this inconsistency, please : \b0 ");

//            sb.Append(@" Drop any seismic cube from materials or pressures used during the model built");

//            sb.Append(@" \line ");
   
//            sb.Append(@"}");

//            this.richTextBox1.Rtf = sb.ToString();


        }
    }
}
