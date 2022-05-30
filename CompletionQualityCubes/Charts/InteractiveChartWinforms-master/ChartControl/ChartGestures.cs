using System;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartControl
{
    internal class ChartGestures
    {
        public event EventHandler ChartAttached = delegate { };

        protected Chart _chart = null;

        public ChartGestures( Chart c = null )
        {
            Attach(c);
        }

        public virtual Chart Attach( Chart chart )
        {
            if (_chart == chart) return chart;

            Detach();
            return _chart;
        }

        protected virtual void Detach()
        {
            if (_chart == null) return;
            _chart = null;
        }

        public static ChartGestures GetInstance( Chart c = null )
        {
            return new ChartGestures(c);
        }
    }
}