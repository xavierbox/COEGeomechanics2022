using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;

namespace Restoration.GFM
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class GFMPostProcessUI : UserControl
    {
        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private GFMPostProcess process;

        /// <summary>
        /// Initializes a new instance of the <see cref="GFMPostProcessUI"/> class.
        /// </summary>
        public GFMPostProcessUI(GFMPostProcess process)
        {
            InitializeComponent();

            this.process = process;
        }
    }
}
