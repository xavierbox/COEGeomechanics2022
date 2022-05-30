using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JsonTests
{
    public partial class LevelTrackBar : UserControl
    {

        public decimal Minimum
        {
            get => ValueSelector.Minimum;
            set
            {
                ValueSelector.Minimum = value;
                MinLabel.Text = ValueSelector.Minimum.ToString();
            }
        }

        public decimal Maximum
        {
            get => ValueSelector.Maximum;
            set { ValueSelector.Maximum = value; MaxLabel.Text = ValueSelector.Maximum.ToString(); }
        }

        public int DecimalPlaces
        {
            get => ValueSelector.DecimalPlaces;
            set => ValueSelector.DecimalPlaces = value;
        }

        private void SetValueLabel() 
        {

            ValueLabel.Text = DecimalPlaces == 0 ?
                ((int)(ValueSelector.Value)).ToString():
                (ValueSelector.Value).ToString();

        }

        public decimal Value
        {
            get => ValueSelector.Value;
            set
            {
                decimal _value = Math.Max(Minimum, Math.Min( value,Maximum));
                ValueSelector.Value = _value;
                trackBar1.Value = ValueToTrackbar();
                SetValueLabel();// ValueLabel.Text = ValueSelector.Value.ToString();
            }
        }


        public LevelTrackBar()
        {
            InitializeComponent();

            trackBar1.Minimum = 0;
            trackBar1.Maximum = 100;

            if (ValueSelector.Minimum == ValueSelector.Maximum)
            {
                ValueSelector.Minimum = 0;
                ValueSelector.Maximum = 100;
            }

            DecimalPlaces = 0;

        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            decimal percent = (decimal)((1.0f * trackBar1.Value - trackBar1.Minimum) / (1.0f * trackBar1.Maximum - trackBar1.Minimum));
            Value = ValueSelector.Minimum + percent * (ValueSelector.Maximum - ValueSelector.Minimum);
        }

        private void ValueSelector_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                decimal value = ValueSelector.Value;
                Value = value;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {

            var x = trackBar1.Location.X;
            Point pos = ValueLabel.Location;
            ValueLabel.Location = new Point(x + (int)(TrackBarPercent() * trackBar1.Width), pos.Y);

            //percent = (decimal)((1.0f * trackBar1.Value - trackBar1.Minimum) / (1.0f * trackBar1.Maximum - trackBar1.Minimum));
            //decimal temp = ValueSelector.Minimum + percent * (ValueSelector.Maximum - ValueSelector.Minimum);
            //ValueLabel.Text = temp.ToString();

            decimal temp = TrackBarToValue();
            if(temp!=Value)
            Value = TrackBarToValue();
        }

        decimal TrackBarToValue()
        {
            decimal percent = (decimal)((1.0f * trackBar1.Value - trackBar1.Minimum) / (1.0f * trackBar1.Maximum - trackBar1.Minimum));
            decimal temp = ValueSelector.Minimum + percent * (ValueSelector.Maximum - ValueSelector.Minimum);
            return temp;
        }
        decimal TrackBarPercent()
        {
            decimal percent = (decimal)((1.0f * trackBar1.Value - trackBar1.Minimum) / (1.0f * trackBar1.Maximum - trackBar1.Minimum));
            return percent;
        }

        int ValueToTrackbar()
        {
            float percent = (float)((ValueSelector.Value - Minimum) / (Maximum - Minimum));
            return (int)(percent * trackBar1.Maximum);
        }

        private void ValueSelector_ValueChanged(object sender, EventArgs e)
        {
            trackBar1.Value = ValueToTrackbar();
            SetValueLabel();// ValueLabel.Text = ValueSelector.Value.ToString();
        }
    }
}
