using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Slb.Ocean.Petrel.Seismic;

namespace SeismicPoroelastic
{
    partial class CustomTabName1 : UserControl
    {
        private PoroelasticStressAtribute.Arguments arguments = null;
        private IGeneratorContext context = null;

        public CustomTabName1(PoroelasticStressAtribute.Arguments arguments, IGeneratorContext context)
        {
            this.arguments = arguments;
            this.context = context;

            InitializeComponent();
        }
    }
}
