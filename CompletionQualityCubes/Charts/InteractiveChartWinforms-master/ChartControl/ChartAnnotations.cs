using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartControl
{
  class ChartAnnotations
  {
    public event EventHandler ChartAttached = delegate { };

    protected Chart _chart = null;

    public ChartAnnotations(Chart c = null)
    {
      Attach(c);
    }

    public virtual Chart Attach(Chart chart)
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

    public static ChartAnnotations GetInstance(Chart c = null)
    {

      return new ChartAnnotations(c);
    }


  }
}
