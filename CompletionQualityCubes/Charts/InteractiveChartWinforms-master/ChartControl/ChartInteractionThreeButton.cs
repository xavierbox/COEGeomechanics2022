using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartControlExtensions
{
    public class ChartInteractionThreeButton : ChartInteraction
    {
        //public static ChartInteractionThreeButton CreateInstance(Chart chart)
        //{
        //  return new ChartInteractionThreeButton(chart);
        //}
        public static ChartInteraction CreateInstance( Chart chart )
        {
            return new ChartInteractionThreeButton(chart);
        }

        private List<int> integerList = new List<int>();

        public ChartInteractionThreeButton( Chart chart )
        {
            Attach(chart);
        }

        public void SetNextInteractionMode()
        {
            string[] names = Enum.GetNames(typeof(ChartInteractionMode));

            string currentName = Enum.GetName(typeof(ChartInteractionMode), InteractionMode);

            int index = Array.FindIndex(names, t => t == currentName);

            ChartInteractionMode newValue = (ChartInteractionMode)(Enum.Parse(typeof(ChartInteractionMode), names[index + 1 >= names.Length ? 0 : index + 1]));

            while (newValue == ChartInteractionMode.Pan) //bypass it
            {
                index = 1;
                newValue = (ChartInteractionMode)(Enum.Parse(typeof(ChartInteractionMode), names[index + 1 >= names.Length ? 0 : index + 1]));
            }

            InteractionMode = newValue;
        }

        #region events

        protected override void MouseUp( object sender, MouseEventArgs e )
        {
            EndPoint = (new Point(e.X, e.Y));
            MouseIsDown = null;
            int dx = EndPoint.X - StartPoint.X, dy = EndPoint.Y - StartPoint.Y;

            if (e.Button == MouseButtons.Right) //finish a pan
            {
                if (Math.Abs(dx) + Math.Abs(dy) < 10)      // it was a quick right-click, just change the interaction mode
                {
                    SetNextInteractionMode();
                    return;
                }

                //this is the equivalent in data units to the drag length.
                PixelPositionToValue(StartPoint.X, StartPoint.Y, out double startValueX, out double startValueY, out double startValueY2);
                PixelPositionToValue(EndPoint.X, EndPoint.Y, out double endValueX, out double endValueY, out double endValueY2);

                Axis[] axes = new Axis[] { DefaultChartArea.AxisX, DefaultChartArea.AxisY, DefaultChartArea.AxisY2 };
                double[] delta = { (endValueX - startValueX), (endValueY - startValueY), (endValueY2 - startValueY2) };

                if (_storedAxesViews.Count < 1) StoreAxisViews();
                for (int d = 0; d < 3; d++)
                {
                    axes[d].Minimum = axes[d].ScaleView.ViewMinimum - 0.8 * delta[d];
                    axes[d].Maximum = axes[d].ScaleView.ViewMaximum - 0.8 * delta[d];
                }

                StoreAxisViews();
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (ZoomStarted == true)
                {
                    if (Math.Abs(dx) + Math.Abs(dy) > 10)
                    {
                        if (_storedAxesViews.Count < 1) StoreAxisViews();
                        FinalizeZoom();
                        StoreAxisViews();
                    }

                    DrawZoomRectangle();
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                AdjustAxes();
                _storedAxesViews.Clear();
            }

            ZoomStarted = false;
        }

        protected override void MouseMove( object sender, MouseEventArgs e )
        {
            if (InteractionMode == ChartInteractionMode.Zoom)
            {
                if (!ZoomStarted) return;

                DrawZoomRectangle();
                UpdateRectangle(StartPoint, new Point(e.X, e.Y));
                DrawZoomRectangle();
            }
        }

        protected override void MouseWheel( object sender, MouseEventArgs e )
        {
            //AdjustAxes();
            if (e.Delta > 10)
            {
                _storedAxesViews.RestoreNextAxisView();
            }
            else if (e.Delta < -10)
            {
                _storedAxesViews.RestorePreviousAxisView();
            }
        }

        protected void DrawZoomRectangle()
        {
            Pen pen = new Pen(Color.Black, 1.0f);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            Rectangle screenRect = _chart.RectangleToScreen(ZoomRectangle);
            ControlPaint.DrawReversibleFrame(screenRect, _chart.BackColor, FrameStyle.Dashed);
        }

        protected void FinalizeZoom()
        {
            ZoomStarted = false;
            PixelPositionToValue(ZoomRectangle.X, ZoomRectangle.Y, out double xmin, out double ymin, out double y2min);
            PixelPositionToValue(ZoomRectangle.X + ZoomRectangle.Width, ZoomRectangle.Y + ZoomRectangle.Height, out double xmax, out double ymax, out double y2max);

            _chart.SetAxisInterval(Math.Min(xmin, xmax), Math.Max(xmin, xmax), DefaultChartArea.AxisX);
            _chart.SetAxisInterval(Math.Min(ymin, ymax), Math.Max(ymin, ymax), DefaultChartArea.AxisY);
            _chart.SetAxisInterval(Math.Min(y2min, y2max), Math.Max(y2min, y2max), DefaultChartArea.AxisY2);
        }

        //private void SetAxesLimits(double xmin, double xmax, double ymin, double ymax, double y2min, double y2max)
        //{
        //  ////this works better with panning
        //  var chartArea = ChartArea;
        //  chartArea.AxisX.Minimum = xmin;
        //  chartArea.AxisX.Maximum = xmax;

        //  chartArea.AxisY.Minimum = ymin;
        //  chartArea.AxisY.Maximum = ymax;

        //  chartArea.AxisY2.Minimum = y2min;
        //  chartArea.AxisY2.Maximum = y2max;

        //  Console.WriteLine($" xmin { xmin } xmax {xmax } ymin {ymin} ymax { ymax} zmin {y2min} zmax {y2max}");

        //}

        protected override void MouseDown( object sender, MouseEventArgs e )
        {
            StartPoint = (new Point(e.X, e.Y));
            EndPoint = StartPoint;
            SelectedDataPoint = null;
            SelectedSeries = null;
            MouseIsDown = e.Location;
            ZoomStarted = false;
            _chart.Focus();

            if ((InteractionMode == ChartInteractionMode.Zoom) && (e.Button == MouseButtons.Left))
            {
                ZoomStarted = true;
                UpdateRectangle(StartPoint, EndPoint);
            }

            HitTestResult hitResult = _chart.HitTest(e.X, e.Y);

            if (hitResult.ChartElementType == ChartElementType.DataPoint)
            {
                SelectedSeries = (Series)(hitResult.Series);
                SelectedDataPoint = (DataPoint)hitResult.Object;
                //lastMarkerBorderColor = selectedDataPoint.MarkerBorderColor;
                //selectedDataPoint.MarkerBorderColor = MarkerSelectedColor;//
                //selectedDataPoint.MarkerBorderWidth = selectedDataPoint.MarkerBorderWidth      MarkerSelectedBorderExtra;
                //DisplayPointLabel(selectedDataPoint.XValue, selectedDataPoint.YValues[0]);
            }
        }

        protected void UpdateRectangle( Point p1, Point p2 )
        => ZoomRectangle = new Rectangle(new Point(Math.Min(p1.X, p2.X), Math.Min(p1.Y, p2.Y)), new Size(Math.Abs(p1.X - p2.X), Math.Abs(p1.Y - p2.Y)));

        protected override void KeyDown( object sender, KeyEventArgs e )
        {
            if ((e.KeyCode == Keys.ShiftKey) || (e.KeyCode == Keys.Shift))
            {
                ;
            }

            if ((e.KeyCode == Keys.T))
            {
                if (MouseIsDown != null)
                {
                    Console.WriteLine("Add text activated. Mouse left pressed plus letter t");
                    Label l = new Label();
                    l.Text = "I am a text";
                    l.Parent = _chart;
                    l.Location = (Point)MouseIsDown;
                    l.BackColor = Color.Transparent;
                }
            }
        }

        #endregion events
    }//class
}//namespace