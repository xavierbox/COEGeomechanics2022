using Slb.Ocean.Basics;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Gigamodel
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class BigDataCalculatorUI : UserControl
    {
        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private BigDataCalculator process;

        /// <summary>
        /// Initializes a new instance of the <see cref="BigDataCalculatorUI"/> class.
        /// </summary>
        public BigDataCalculatorUI( BigDataCalculator process )
        {
            InitializeComponent();

            this.process = process;
        }

        private void button3_Click( object sender, EventArgs e )
        {
        }

        private void dropTarget1_DragDrop( object sender, DragEventArgs e )
        {
            Property p = e.Data.GetData(typeof(Property)) as Property;

            //  var access = p.SpecializedAccess.OpenFastPropertyIndexer.g.FastPropertyAccessor;

            FastPropertyIndexer indexer = p.SpecializedAccess.OpenFastPropertyIndexer();

            Stopwatch timer = new Stopwatch();
            timer.Start();
            Grid grid = p.Grid;
            var ymValues = new List<float>(grid.NumCellsIJK.K * grid.NumCellsIJK.J * grid.NumCellsIJK.I);
            using (var ym = p.SpecializedAccess.OpenFastPropertyIndexer())
            {
                for (var k = 0; k < grid.NumCellsIJK.K; k++)
                {
                    for (var j = 0; j < grid.NumCellsIJK.J; j++)
                    {
                        for (var i = 0; i < grid.NumCellsIJK.I; i++)
                        {
                            var i3 = new Index3(i, j, k);
                            ymValues.Add(ym[i3[0], i3[1], i3[2]]);
                        }
                    }
                }
            }
            var ellapsedTime1 = timer.ElapsedMilliseconds;

            Property p2 = ProjectTools.GetOrCreateProperty(p.PropertyCollection, "Squared");
            ProjectTools.SetValues(p2, ymValues);

            var ellapsedTime2 = timer.ElapsedMilliseconds;
        }
    }
}