using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OceanControlsLib
{
    public partial class NumericTextBox : UserControl
    {
        public NumericTextBox()
        {
            InitializeComponent();
            Value = MinValue + (MaxValue - MinValue)/2;
            Units = string.Empty;
        }

        public int Decimals { get; set; } = 2;

        bool ValueFromString(string s, out decimal v)
        {
            bool ret = decimal.TryParse(s, out v);
            v = Math.Min(MaxValue,Math.Max(MinValue,Math.Round(v, Decimals)));
            return ret;
                            
        }

        string StringFromValue(decimal v)
        {
            return Math.Round(v, Decimals).ToString();
        }

        public string Units { get =>UnitsLabel.Text; set { UnitsLabel.Text = value; } }

        public decimal Value 
        { 
            get
            {
                decimal v; 
                ValueFromString(presentationBox1.Text, out v);
                return v;
            }
            set 
            {

                decimal v = Math.Min(MaxValue, Math.Max(value, MinValue));
                presentationBox1.Text = StringFromValue(v); 
            } 
        } 

        public decimal MaxValue { get; set; } = 100.0M;

        public decimal MinValue { get; set; } = 0.0M;

    }
}
