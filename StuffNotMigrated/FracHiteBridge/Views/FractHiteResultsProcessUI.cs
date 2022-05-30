using ChartControlExtensions;
using ChartControls;
using FrachiteData;
using OceanControls;
using Slb.Ocean.Basics;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

//using ChartControl;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Grid = Slb.Ocean.Petrel.DomainObject.PillarGrid.Grid;
using PropertyCollection = Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection;

namespace FracHiteBridge
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class FractHiteResultsProcessUI : UserControl
    {
        private ChartInteractionZoomPanEdit chartInteraction;

        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private FractHiteResultsProcess _process;

        private FrachiteRunner fracHiterunner = new FrachiteRunner();

        //InteractiveChart interactiveChart1;
        private string _gridDroid;

        private Dictionary<string, Property> _propsToReadIn1DSimulation = new Dictionary<string, Property>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FractHiteResultsProcessUI"/> class.
        /// </summary>
        public FractHiteResultsProcessUI( FractHiteResultsProcess process )
        {
            InitializeComponent();

            ConfigureUIComponents();

            //interactiveChart1 = new InteractiveChart();
            // panel1.Controls.Add(interactiveChart1);
            // interactiveChart1.Dock = DockStyle.Fill;

            ConnectEvents();
            chartInteraction = new ChartInteractionZoomPanEdit(chart1);

            _process = process;
        }

        private void ConfigureUIComponents()
        {
            numericUpDownII.Maximum = 1000;
            numericUpDownJJ.Maximum = 1000;
            numericUpDownKK.Maximum = 1000;

            ImageList images = new ImageList();
            images.Images.Add(PetrelImages.Property);
            DropPresentationBox[] drops = new DropPresentationBox[3] { _propsDropPresentationBox, _propsDropPresentationBox2, dropPresentationBox3D };
            Array.ForEach(drops, ( item ) =>
            {
                item.AcceptedTypes = new List<Type>() { typeof(PropertyCollection) };
                item.PlaceHolder = "Drag-drop a property folder from a simple grid";
                item.ErrorImage = PetrelImages.Cancel;
                item.ImageList = images;
            });

            //  _propsDropPresentationBox.AcceptedTypes = new List<Type>() { typeof(PropertyCollection) };
            //_propsDropPresentationBox.PlaceHolder = "Drag-drop a property folder from a simple grid";
            //_propsDropPresentationBox.ErrorImage = PetrelImages.Cancel;
            //ImageList images2 = new ImageList();
            //images2.Images.Add(PetrelImages.Property);
            //_propsDropPresentationBox.ImageList = images2;

            _cancelButton.Image = PetrelImages.Cancel;
            _acceptButton.Image = PetrelImages.Apply;
            _folderButton.Image = PetrelImages.Folder;

            //panel1.Controls.Add(_chart);
            //_chart.Dock = DockStyle.Fill;

            //dropPresentationBox1.AcceptedTypes = new List<Type>() { typeof(Grid) };
            //dropPresentationBox1.PlaceHolder = "Source grid";
            //dropPresentationBox1.ErrorImage = PetrelImages.Cancel;
            //ImageList images2 = new ImageList();
            //images2.Images.Add(PetrelImages.PillarGrid);
            //dropPresentationBox1.ImageList = images2;
        }

        private void ConnectEvents()
        {
            chart1.MouseDoubleClick += ( s, e ) =>
            {
                ChartMainControl configDialog = new ChartMainControl(chart1);
                configDialog.ShowDialog(chart1);
            };
            _cancelButton.Click += ( sender, evt ) => { this.ParentForm.Close(); };

            _acceptButton.Click += ( sender, evt ) => { ProcessData(); };

            _folderButton.Click += ( sender, evt ) =>
            {
                using (OpenFileDialog folderDlg = new OpenFileDialog())
                {
                    if (folderDlg.ShowDialog() == DialogResult.OK)
                    {
                        folderPresentationBox.Text = folderDlg.FileName;
                        if (!ParseFrachiteResultsFile(folderDlg.FileName))
                        {
                            MessageBox.Show("Cannot parse the input file", "I/O error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                        //string folder = Path.GetDirectoryName(folderDlg.FileName);
                        //string droid = GetGridDroidFromfile(folder).Trim();                                        //this is a terrible PATCH
                        //try
                        //{
                        //  var grid = (Grid)(DataManager.Resolve(new Droid(droid)));
                        //  dropPresentationBox1.Value = grid;
                        //}
                        //catch
                        //{
                        //  ;
                        //}
                    }
                }
            };

            clearChart.Click += ( s, evt ) => { chart1.Clear(); };

            createChart.Click += ( sender, evt ) =>
            {
                CreateChart();
            };

            runButton.Click += ( sender, evt ) =>
            {
                run1DSimulation();
            };

            // editChart.Click += (Sender, evt) => { interactiveChart1.InteractionMode = InteractionModes.Edit; };
            //  zoomchart.Click += (Sender, evt) => { interactiveChart1.InteractionMode = InteractionModes.Zoom; };
            //  clearChart.Click += (Sender, evt) => { interactiveChart1.Clear(); };

            _propsDropPresentationBox2.ValueChanged += ( ender, evt ) =>
              {
                  _propsToReadIn1DSimulation.Clear();
                  if (_propsDropPresentationBox2.Value is PropertyCollection)
                  {
                      PropertyCollection c = (PropertyCollection)(_propsDropPresentationBox2.Value);

                      //lets try to find the properties with the required names
                      string[] namesRequired = FrachiteData.FrachiteConfig.RequiredPropertyNamesFromPetrel;
                      List<string> namesInFolder = c.Properties.Select(t => t.Name).ToList(), missingProperties = new List<string>();
                      IEnumerable<Property> props = c.Properties;

                      foreach (string name in namesRequired)
                      {
                          int index = namesInFolder.IndexOf(name);
                          if (index >= 0)
                          {
                              _propsToReadIn1DSimulation.Add(name, props.ElementAt(index));
                          }
                          else
                          {
                              _propsToReadIn1DSimulation.Add(name, null);
                              missingProperties.Add(name);
                          }
                      }

                      if (missingProperties.Count > 0)
                      {
                          MessageBox.Show("some properties are missing", "Missing properties", MessageBoxButtons.OK, MessageBoxIcon.Error);
                          _propsDropPresentationBox2.Value = null;
                      }
                  }
                  else if (_propsDropPresentationBox2.Value != null)
                  {
                      MessageBox.Show("Please drop a valid property folder with the needed properties", "Input error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                      _propsDropPresentationBox2.Value = null;
                  }
                  else
                  {
                      ;
                  }
              };
        }

        #region 1D simulations

        private void ProcessData1D()
        {
            run1DSimulation();
            //int i = (int)numericUpDownII.Value, j = (int)numericUpDownJJ.Value, k = (int)numericUpDownKK.Value;
            //Index3 i3 = (dropPresentationBox1.Value as Grid).NumCellsIJK;
            //int cell = i + j * i3.I + k * (i3.I * i3.J);
        }

        private void CreateChart()
        {
            //try
            //{
            //  Dictionary<string, float[]> data = (Dictionary<string, float[]>)(interactiveChart1.Tag);

            //  var x = data[comboBoxX.SelectedValue as string];

            //  //float [] x = data[comboBoxX.SelectedIndex]
            //  //float[] x = data[ comboBoxX.Se];
            //}
            //catch(Exception e )
            //{
            //  string why = e.ToString();
            //}
        }

        private void run1DSimulation()
        {
            PropertyCollection c = (PropertyCollection)_propsDropPresentationBox2.Value;
            int nCellsK = c.Grid.NumCellsIJK.K;// _propsToReadIn1DSimulation[_propsToReadIn1DSimulation.Keys.ElementAt(0)].NumCellsIJK.K;

            int i = (int)numericUpDownII.Value, j = (int)numericUpDownJJ.Value, k = (int)numericUpDownKK.Value;
            FrachiteSimulationBinaryData data = new FrachiteSimulationBinaryData();
            List<Index3> indices = new List<Index3>();

            //int nCellsK = _propsToReadIn1DSimulation[_propsToReadIn1DSimulation.Keys.ElementAt(0)].NumCellsIJK.K;
            for (int nk = 0; nk < nCellsK; nk++)
                indices.Add(new Index3(i, j, nk));

            var aux = ProjectTools.GetOrCreateProperty(c, "FRACHITESINGLECELL");
            ProjectTools.SetValue(ref aux, 0.0f);
            ProjectTools.SetValues(ref aux, new List<Index3>() { new Index3(i, j, k) }, new List<float>() { 1.0f });

            foreach (var name in FrachiteConfig.RequiredPropertyNamesFromPetrel) //;// _propsToReadIn1DSimulation.Keys)
            {
                //if (name != "FRACHITEFLAG")
                {
                    Property p = ProjectTools.GetProperty(c, name != "FRACHITEFLAG" ? name : "FRACHITESINGLECELL");
                    float[] values = ProjectTools.GetPropertyValuesAtIndices(p, indices).ToArray();// indices);
                    data.Arrays.Add(name, values);
                }
            }
            data.Nx = 1;
            data.Ny = 1;
            data.Nz = nCellsK;

            FrachiteRunner runner = new FrachiteRunner();
            runner.BinaryData = data;
            runner.Folder = "D:\\";//Path.GetTempPath();
            runner.ModelName = "FH1D";
            runner.RunSimulation();

            if ((runner.LastResults == null) || (runner.LastResults.Count() < 1))
            {
                MessageBox.Show("The convergency was not of the expected quality. Simulation results were deleted");
                return;
            }
            else
            {
                //plot last results. vs time
                var results = runner.LastResults.ElementAt(0);
                var step = results.Arrays["No."].ToArray();
                var topTip = results.Arrays["TOP_TIP"].ToArray();
                var bottomTip = results.Arrays["BOT_TIP"];
                var pressure = results.Arrays["PRESSURE"];

                var tip1 = step.Zip(topTip, ( first, second ) => new PointF(first, second)).ToList();// first + " " + second);
                var tip2 = step.Zip(bottomTip, ( first, second ) => new PointF(first, second)).ToList();// first + " " + second);
                var pre = step.Zip(pressure, ( first, second ) => new PointF(first, second)).ToList();// first + " " + second);

                chart1.AddXYSeries("Top_Tip", tip1).Enabled = true;
                chart1.AddXYSeries("Bottom_Tip", tip2).Enabled = true;
                Series s = chart1.AddXYSeries("Pressure", pre);
                s.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                s.Enabled = true;

                //chart1.Clear();
                //interactiveChart1.AxisXGrid = false;
                //interactiveChart1.AxisYGrid = false;
                //interactiveChart1.Chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
                //interactiveChart1.Chart.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
                //interactiveChart1.Chart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
                //bool dothis = true;

                //if (dothis)
                //{
                //interactiveChart1.AddXYSeries( "Top tip", tip1 ).Enabled = true;
                //interactiveChart1.AddXYSeries( "Bottom tip", tip2).Enabled=true;
                //interactiveChart1.AdjustAxes();

                //interactiveChart1.Chart.ChartAreas[0].AxisY2.Enabled  = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;
                //var s = interactiveChart1.AddXYSeries("Pressure", pre);
                //s.YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;
                //s.Enabled = true;

                //interactiveChart1.Chart.ChartAreas[0].AxisY2.Minimum = pressure.Min() * 0.95;
                //interactiveChart1.Chart.ChartAreas[0].AxisY2.IsStartedFromZero = false;
                //interactiveChart1.Chart.ChartAreas[0].AxisY2.RoundAxisValues();
                //}

                //var l = interactiveChart1.AddHorizontalLevel($"Top", 4900f, 0, step.Max());
                //l.BorderWidth = 5;
                //l.BorderColor = Color.Red;
                //l.Enabled = true;

                int counter = 1;
                //interactiveChart1.AddHorizontalLevel($"Top{counter++}", counter*20.0f, 0, step.Max());
                //interactiveChart1.AdjustAxes();

                //
                foreach (var top in results.ReservoirTops)
                {
                    //float y = top.Value[0];
                    //    List<PointF> pts = new List<PointF>(){
                    //                                         new PointF(step.Min() ,y),
                    //                                         new PointF(step.Max(),y)
                    //                                         };

                    ////Series s = GetOrCreateSeries(name, options ?? SeriesOptions.HorizontalLine, pts);

                    //Series h = chart1.AddXYSeries($"Top{counter++}", pts, SeriesOptions.HorizontalLine);
                    //h.IsVisibleInLegend = false;
                    //h.Enabled = true;

                    //counter += 1;
                }

                //    Series s = GetOrCreateSeries(name, options ?? SeriesOptions.HorizontalLine, pts);
                //    s.IsVisibleInLegend = false;
                //    return s;

                ////var l =  interactiveChart1.AddHorizontalLevel( $"Top{counter++}",y, 0, step.Max());

                ////l.BorderWidth = 2;
                ////l.BorderColor = Color.Red;
                ////l.Color = Color.Black;
                ////l.Enabled = true;

                //counter += 1;
                //        }

                //interactiveChart1.Chart.ChartAreas[0].AxisY2.Enabled = System.Windows.Forms.DataVisualization.Charting.AxisEnabled.True;

                // interactiveChart1.AdjustAxes();

                //interactiveChart1.Tag = runner.LastResults.ElementAt(0).Arrays;

                List<string> columnNames = runner.LastResults.ElementAt(0).Arrays.Select(t => t.Key).ToList();
                columnNames.Add(string.Empty);

                comboBoxX.Items.AddRange(columnNames.ToArray());
                comboBoxY1.Items.AddRange(columnNames.ToArray());
                comboBoxY2.Items.AddRange(columnNames.ToArray());
                comboBoxY3.Items.AddRange(columnNames.ToArray());

                comboBoxX.SelectedIndex = 0;
                comboBoxY1.SelectedIndex = 1;
                comboBoxY1.SelectedIndex = 2;
                comboBoxY1.SelectedIndex = 3;
            }
        }

        #endregion 1D simulations

        public string GetGridDroidFromfile( string file )
        {
            var files = Directory.GetFiles(file, "*GridDroid*");
            return files.Count() > 0 ? File.ReadAllText(files.ElementAt(0), Encoding.UTF8) : string.Empty;
        }


private bool ParseFrachiteResultsFile( string file )
        {
            try
            {
                bool retVal = fracHiterunner.DeserializeResults(file);
                MinStep = 0;
                MaxStep = (int)(fracHiterunner.LastResults.Select(t => t.Arrays["No."].Max()).Max());
                Step = MaxStep;

                MaxPressure = fracHiterunner.LastResults.Select(t => t.Arrays["PRESSURE"].Max()).Max();
                MinPressure = MaxPressure;

                foreach (var item in fracHiterunner.LastResults)
                {
                    if (item.Arrays["PRESSURE"].Min() > 1.0)
                    {
                        MinPressure = Math.Min(MinPressure, item.Arrays["PRESSURE"].Min());
                    }
                }

                this.trackBar1.Minimum = 0;
                this.trackBar1.Value = 0;
                this.trackBar1.Maximum = 100;
            }
            catch //(Exception e)
            {
                return false;
            }
            return true;
        }

        private int Step
        {
            get
            {
                return (int)numericUpDown2.Value;//.Value;
            }
            set
            {
                numericUpDown2.Value = value;
            }
        }

        private int MinStep
        {
            get
            {
                return (int)numericUpDown2.Minimum;//.Value;
            }
            set
            {
                numericUpDown2.Minimum = value;
            }
        }

        private int MaxStep
        {
            get
            {
                return (int)numericUpDown2.Maximum;
            }
            set
            {
                numericUpDown2.Maximum = value;
            }
        }

        private float MinPressure { get; set; }
        private float MaxPressure { get; set; }

        //private int[] GetCellIndices(int elementIndex)//int nx, int ny, int nz) (int,int,int) and return (...) in c# 7
        //{
        //  int Nx = BinaryData.Nx, Ny = BinaryData.Ny, Nz = BinaryData.Nz;

        //  int elementVerticalGap = Nx * Ny;
        //  //int element index is assumed to be as  = ii + jj * NumElementsIJK.I + kk * (NumElementsIJ);
        //  int elementK = (int)(elementIndex / (Nx * Ny));
        //  int elementJ = (int)((elementIndex - elementK * elementVerticalGap) / Nx);
        //  int elementI = elementIndex - elementJ * Nx - elementK * elementVerticalGap;

        //  return new int[] { elementI, elementJ, elementK };
        //}

        private Dictionary<string, Property> GerOrCreateGridPropertiesFHiteResults( PropertyCollection col, float? initialValue = null )
        {
            Property p = ProjectTools.GetOrCreateProperty(col, "Bottomhole Pressure", PetrelProject.WellKnownTemplates.PetrophysicalGroup.Pressure);
            Property s = ProjectTools.GetOrCreateProperty(col, "Average Stress", PetrelProject.WellKnownTemplates.GeomechanicGroup.StressTotal);
            Property w = ProjectTools.GetOrCreateProperty(col, "Average Width", PetrelProject.WellKnownTemplates.GeometricalGroup.Distance);

            if (initialValue != null)
            {
                ProjectTools.SetValue(ref p, (float)initialValue);
                ProjectTools.SetValue(ref s, (float)initialValue);
                ProjectTools.SetValue(ref w, (float)initialValue);
            }

            Dictionary<string, Property> data = new Dictionary<string, Property>() { { "Bottomhole Pressure", p }, { "Average Stress", s }, { "Average Width", w } };
            return data;
        }

        private Index3 GetCellIndices( int elementIndex, Grid g )//int nx, int ny, int nz) (int,int,int) and return (...) in c# 7
        {
            int Nx = g.NumCellsIJK.I, Ny = g.NumCellsIJK.J, Nz = g.NumCellsIJK.K;

            int elementVerticalGap = Nx * Ny;
            //int element index is assumed to be as  = ii + jj * NumElementsIJK.I + kk * (NumElementsIJ);
            int elementK = (int)(elementIndex / (Nx * Ny));
            int elementJ = (int)((elementIndex - elementK * elementVerticalGap) / Nx);
            int elementI = elementIndex - elementJ * Nx - elementK * elementVerticalGap;

            return new Index3(elementI, elementJ, elementK);
        }

        private void ProcessData()
        {
            //if (tabControl1.SelectedTab == tabControl1.TabPages[1])
            if (tabControl2.TabPages[1].Visible)
            {
                ProcessData1D();
            }
            if (tabControl2.TabPages[0].Visible)
            {
                ProcessData3D();
            }
            else
            {
                ;
            }
        }

        private void ProcessData3D()
        {
            try
            {
                float? initialValue = 0.0f;
                PropertyCollection col = (PropertyCollection)(dropPresentationBox3D.Value);// _propsDropPresentationBox2.Value);
                Dictionary<string, Property> data = GerOrCreateGridPropertiesFHiteResults(col, initialValue);

                var p = data["Bottomhole Pressure"].SpecializedAccess.OpenFastPropertyIndexer();
                var s = data["Average Stress"].SpecializedAccess.OpenFastPropertyIndexer();
                var w = data["Average Width"].SpecializedAccess.OpenFastPropertyIndexer();

                int counter = 0;
                foreach (FrachiteFileResults result in fracHiterunner.LastResults)
                {
                    counter++;
                    int cell = result.CellIndex;
                    var i3 = result.Indices;
                    var perfCenter = col.Grid.GetCellCenter(new Slb.Ocean.Basics.Index3(i3.I, i3.J, i3.K));

                    var ww = result.Arrays["PRESSURE"];
                    float pcut = (float)(MinPressure + (MaxPressure - MinPressure) * (trackBar1.Value / 100.0));
                    int index = ww.FindIndex(t => t > pcut);
                    if (index < 0) index = ww.Count;
                    index = Math.Min(Step, index);

                    var pressures = ww.Take(index);
                    var width = result.Arrays["AVG_Width"].Take(index);
                    var stress = result.Arrays["AVG_STRESS"].Take(index);
                    var toptip = result.Arrays["TOP_TIP"].Take(index);
                    var bottomtip = result.Arrays["BOT_TIP"].Take(index);

                    float[] avg_stress = width.ToArray();

                    //alright, we will now populate the results for the pressure in the grid.
                    //the idea is that we see where is the top tip (cell), at each step and put a pressure there.
                    //we do the same for the bottom-tip.
                    float topPerf = result.PerforationTop;
                    float bottomPerf = result.PerforationBottom;
                    float initialTVD = 0.5f * (topPerf + bottomPerf);

                    Index3 cellTipPerforation = new Index3(i3.I, i3.J, i3.K);
                    int kTop = i3.K, kBottom = i3.K;
                    int kmax = int.MinValue, kmin = int.MaxValue;

                    for (int k = 0; k < index; k++)
                    {
                        try
                        {
                            for (int dir = 0; dir <= 1; dir++)
                            {
                                var displacement = (dir == 0 ? initialTVD - toptip.ElementAt(k) : bottomtip.ElementAt(k) - initialTVD);
                                var tipPositionZ = (dir == 0 ? perfCenter.Z + displacement : perfCenter.Z - displacement);
                                var cellTip = col.Grid.GetCellAtPoint(new Slb.Ocean.Geometry.Point3(perfCenter.X, perfCenter.Y, tipPositionZ)); //this is the cell

                                if (cellTip != null) //the frac may grow outside the grid and then the celltip is null
                                {
                                    p[cellTip] = Math.Max(0.0f, pressures.ElementAt(k));
                                    s[cellTip] = Math.Max(0.0f, stress.ElementAt(k));
                                    w[cellTip] = Math.Max(0.0f, width.ElementAt(k));

                                    kmax = Math.Max(kmax, cellTip.K);
                                    kmin = Math.Min(kmin, cellTip.K);
                                }
                                else
                                {
                                    ;
                                }
                            }

                            //var displacement = initialTVD - toptip.ElementAt(k);  //must be possitive
                            //var tipPositionZ = perfCenter.Z + displacement;    //
                            //var cellTip = col.Grid.GetCellAtPoint(new Slb.Ocean.Geometry.Point3(perfCenter.X, perfCenter.Y, tipPositionZ)); //this is the cell

                            //if (cellTip != null) //the frac may grow outside the grid and then the celltip is null
                            //{
                            //  p[cellTip] = Math.Max(0.0f, pressures.ElementAt(k));
                            //  s[cellTip] = Math.Max(0.0f, stress.ElementAt(k));
                            //  w[cellTip] = Math.Max(0.0f, width.ElementAt(k));

                            //  kmax = Math.Max(kmax, cellTip.K);
                            //  kmin = Math.Min(kmin, cellTip.K);
                            //}

                            //displacement = bottomtip.ElementAt(k) - initialTVD;  //must be possitive
                            //tipPositionZ = perfCenter.Z - displacement;    //also possitive
                            //cellTip = col.Grid.GetCellAtPoint(new Slb.Ocean.Geometry.Point3(perfCenter.X, perfCenter.Y, tipPositionZ)); //this is the cell

                            //if (cellTip != null)
                            //{
                            //  p[cellTip] = Math.Max(0.0f, pressures.ElementAt(k));
                            //  s[cellTip] = Math.Max(0.0f, stress.ElementAt(k));
                            //  w[cellTip] = Math.Max(0.0f, width.ElementAt(k));

                            //  kmax = Math.Max(kmax, cellTip.K);
                            //  kmin = Math.Min(kmin, cellTip.K);
                            //}
                        }
                        catch (Exception e)
                        {
                            string ew = e.ToString();
                            ;
                        }
                    }//k

                    //PATH: The fracs can grow too quickly ans skip cells. So we fill the hokles with the value above.
                    for (int n = kmin + 1; n <= kmax; n++)
                    {
                        var cellTip = new Index3(i3.I, i3.J, n);
                        if (p[cellTip] < 0.1f)
                        {
                            var theOneAbove = new Index3(i3.I, i3.J, n - 1);
                            p[cellTip] = p[theOneAbove];
                            w[cellTip] = w[theOneAbove];
                            s[cellTip] = s[theOneAbove];
                        }
                    }

                    ;
                    ;
                }

                p.Dispose();
                s.Dispose();
            }
            catch (Exception e)
            {
            }
        }
    }
}