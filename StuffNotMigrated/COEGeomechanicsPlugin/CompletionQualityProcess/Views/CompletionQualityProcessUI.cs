using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;

namespace COEGeomechanicsPlugin
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class CompletionQualityProcessUI : UserControl
    {
        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private CompletionQualityProcess process;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompletionQualityProcessUI"/> class.
        /// </summary>
        public CompletionQualityProcessUI( CompletionQualityProcess process )
        {
            InitializeComponent();

            this.process = process;
        }
    }
}
