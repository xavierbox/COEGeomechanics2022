using CommonData;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector3 = CommonData.Vector3;

namespace Restoration
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class ExportGeometryToDynelUI : UserControl
    {
        /// <summary>
        /// Contains the process instance.
        /// </summary>
        private ExportGeometryToDynel process;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportGeometryToDynelUI"/> class.
        /// </summary>
        public ExportGeometryToDynelUI( ExportGeometryToDynel process )
        {
            InitializeComponent();
            this.objectsGrid.BackgroundColor = Color.White;

            exportButton.Image = PetrelImages.Apply;
            cancelbutton.Image = PetrelImages.Cancel;

            HorizonMeshes = null;
            FaultMeshes = null;
            ExportFolder = null;
            Traslation = new Point3(0.0, 0.0, 0.0);
            ConnectEvents();
            this.process = process;
        }

        private Dictionary<string, IIndexedTriangleMesh> HorizonMeshes { get; set; }

        private Dictionary<string, IIndexedTriangleMesh> FaultMeshes { get; set; }

        private Point3 _traslation;

        private Point3 Traslation
        {
            get { return _traslation; }
            set
            {
                _traslation = value;
                zTraslationControl.Value = (decimal)_traslation.Z;
                yTraslationControl.Value = (decimal)_traslation.Y;
                xTraslationControl.Value = (decimal)_traslation.X;
            }
        }

        private string ExportFolder { get; set; }

        public void ConnectEvents()
        {
            this.dropFaults.DragDrop += ( sender, evt ) =>
            {
                object dropped = evt.Data.GetData(typeof(object));
                FaultMeshes = new Dictionary<string, IIndexedTriangleMesh>();
                HorizonMeshes = new Dictionary<string, IIndexedTriangleMesh>();
                Traslation = new Point3(0.0, 0.0, 0.0);

                objectsGrid.Visible = false;
                Slb.Ocean.Petrel.DomainObject.StructuralGeology.StructuralModel model = null;

                //if (dropped as Slb.Ocean.Petrel.DomainObject.StructuralGeology.FinalFaultCollection != null)
                //{
                //    Slb.Ocean.Petrel.DomainObject.StructuralGeology.FinalFaultCollection faultCollection = dropped as Slb.Ocean.Petrel.DomainObject.StructuralGeology.FinalFaultCollection;

                //    foreach (var f in faultCollection.FinalFaults)
                //        FaultMeshes.Add(f.Name, f.IndexedTriangleMesh);

                //    faultsPresentationBox.Text = faultCollection.Name;
                //    faultsPresentationBox.Image = PetrelImages.FaultModel;

                //    model = faultCollection.StructuralModel;
                //}

                //else if (dropped as Slb.Ocean.Petrel.DomainObject.StructuralGeology.HorizonCollection != null)
                //{
                //    Slb.Ocean.Petrel.DomainObject.StructuralGeology.HorizonCollection horizons = dropped as Slb.Ocean.Petrel.DomainObject.StructuralGeology.HorizonCollection;
                //    foreach (var h in horizons.Horizons)
                //        HorizonMeshes.Add(h.Name, h.IndexedTriangleMesh);

                //    faultsPresentationBox.Text = horizons.Name;
                //    faultsPresentationBox.Image = PetrelImages.Horizon;

                //    model = horizons.StructuralModel;
                //}

                if (dropped as Slb.Ocean.Petrel.DomainObject.StructuralGeology.StructuralModel != null)
                {
                    model = dropped as Slb.Ocean.Petrel.DomainObject.StructuralGeology.StructuralModel;
                    //foreach (var f in model.FinalFaultCollection.FinalFaults)
                    foreach (var f in model.StructuralFramework.FaultFramework.InitialFaults)
                        FaultMeshes.Add(f.Name, f.IndexedTriangleMesh);

                    foreach (var h in model.HorizonCollection.Horizons)
                        HorizonMeshes.Add(h.Name, h.IndexedTriangleMesh);

                    faultsPresentationBox.Text = model.Name;
                    faultsPresentationBox.Image = PetrelImages.Model;

                    objectsGrid.Rows.Clear();

                    if (FaultMeshes.Count > 0)
                        objectsGrid.Rows.Add(PetrelImages.FaultModel, "Fault Model");
                    if (HorizonMeshes.Count > 0)
                        objectsGrid.Rows.Add(PetrelImages.Horizon, "Horizons");

                    objectsGrid.ClearSelection();
                    objectsGrid.Visible = true;
                }
                else
                {
                    faultsPresentationBox.Text = "Please drop a valid structural model ";
                    faultsPresentationBox.Image = null;

                    MessageBox.Show("Please drop a valid sStructural model", "Missing data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Point3 centerFirstHorizon = GetReferecePointFromStructuralModel(model);
                Traslation = GetReferecePointFromStructuralModel(model); ;// centerFirstHorizon;
            };//

            this.flowLayoutPanel1.Resize += new System.EventHandler(this.flowLayoutPanel1_Resize);
            this.selectExportFolderButton.Click += new System.EventHandler(this.selectExportFolderButton_Click);
            this.cancelbutton.Click += new System.EventHandler(this.button2_Click);
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);

            //    foreach (var initialFault in faultFramework.InitialFaults)
            //    {
            //        IndexedTrianglesProvider indexedTriangleProvider = initialFault.GetIndexedTrianglesProvider();//.GetIndexedTriangleProvider();
            //        Console.WriteLine("This initial fault has " + indexedTriangleProvider.Triangles.Count + " triangles.");

            //        Point3SetProvider point3SetProvider = indexedTriangleProvider.GetPointsProvider();
            //        Point3[] point3Set = point3SetProvider.Points.ToArray();
            //        Console.WriteLine("This initial fault has " + point3Set.Length + " points");

            //        IIndexedTriangleMesh meshForFault = initialFault.IndexedTriangleMesh;

            //        var triangles1 = indexedTriangleProvider.Triangles;// meshForFault.Triangles;
            //        var triangles2 = meshForFault.Triangles;// meshForFault.Triangles;
            //        var vertices = meshForFault.Vertices;

            //    }

            //    if (dropped as Collection != null)
            //    {
            //        Collection c = (Collection)(dropped);

            //        System.Collections.Generic.IEnumerator<Slb.Ocean.Data.Hosting.IDomainObject> e = c.GetEnumerator();

            //        foreach (Slb.Ocean.Data.Hosting.IDomainObject d in c)
            //        {
            //            var m = d as TriangleMesh;
            //            ;
            //            ;

            //        }

            //        for (int n = 0; n < c.Count; n++)
            //        {
            //            var item = c[n];
            //            TriangleMesh meshItem = item as TriangleMesh;

            //            if (meshItem != null)
            //            {
            //                var name = meshItem.Layer;
            //                ExportAsGoCad(meshItem, "dfsgdfg");
            //            }
            //        }
            //    }

            //    //string val = dropped.ToString();
            //    //IDescriptionSource src = dropped as IDescriptionSource;
            //    //if (src != null)
            //    //{
            //    //    IDescription descr = src.Description;
            //    //    if (descr != null)
            //    //        val = descr.Layer;
            //    //    ;

            //    //}

            //    //TriangleMesh mesh = dropped as TriangleMesh;

            //};
        }

        //returns the average of all the points in an horizon
        private Point3 GetReferecePointFromStructuralModel( Slb.Ocean.Petrel.DomainObject.StructuralGeology.StructuralModel model )
        {
            if ((model == null) || (model.HorizonCollection.Horizons.Count < 1))
            {
                return null;
            }
            var pts = model.HorizonCollection.Horizons.ElementAt(0).IndexedTriangleMesh.Vertices;

            var avgX = pts.Average(t => t.X);
            var avgY = pts.Average(t => t.Y);
            var avgZ = pts.Average(t => t.Z);

            return new Point3(avgX, avgY, avgZ);
        }

        private bool ExportAsGoCad( string name, string folder, IIndexedTriangleMesh mesh, Point3 displacement = null )
        {
            if (displacement == null) displacement = new Point3(0.0, 0.0, 0.0);
            string header = "GOCAD TSurf 1\nHEADER {\nname: NAMEGOESHERE \n*border:on\n*border*bstone:on\n}\nGOCAD_ORIGINAL_COORDINATE_SYSTEM\nNAME NAMEGOESHERE\n";
            header += "AXIS_NAME \"X\" \"Y\" \"Z\"\n";
            header += "AXIS_UNIT \"m\" \"m\" \"m\"\n";
            header += "ZPOSITIVE Elevation\nEND_ORIGINAL_COORDINATE_SYSTEM\nTFACE\n";
            header = header.Replace("NAMEGOESHERE", Path.GetFileNameWithoutExtension(name));

            var eportFiole = Path.Combine(folder, name);

            if (!name.Contains("BordaJ"))

                using (System.IO.StreamWriter w = new System.IO.StreamWriter(eportFiole))
                {
                    w.WriteLine(header);
                    List<Vector3> vertices = new List<Vector3>();
                    List<IndexedTriangle> triangles = new List<IndexedTriangle>();

                    FixVertexTruncations(mesh, ref vertices, ref triangles);
                    for (int n = 0; n < vertices.Count(); n++)
                    {
                        Vector3 p = vertices[n];
                        string line = "VRTX" + " " + n + " " + (p.X + displacement.X) + " " + (p.Y + displacement.Y) + " " + (p.Z + displacement.Z);
                        w.WriteLine(line);
                    }

                    for (int n = 0; n < triangles.Count(); n++)
                    {
                        IndexedTriangle t = triangles[n];
                        string line = "TRGL" + " " + t.Vertex1 + " " + t.Vertex2 + " " + t.Vertex3;
                        w.WriteLine(line);
                    }

                    w.WriteLine("END");
                }

            return true;
        }

        private List<IndexedTriangle> FixTriangleTruncation( IIndexedTriangleMesh mesh, int[] newIndices )
        {
            var ww = newIndices.Max();

            List<IndexedTriangle> trToReturn = new List<IndexedTriangle>();
            foreach (IndexedTriangle t in mesh.Triangles)
            {
                var v1 = newIndices[t.Vertex1];
                var v2 = newIndices[t.Vertex2];
                var v3 = newIndices[t.Vertex3];

                IndexedTriangle triangle = new IndexedTriangle(v1, v2, v3);
                if (trToReturn.Contains(triangle))
                {
                    ;
                }
                else
                    trToReturn.Add(triangle);
            }

            return trToReturn;
        }

        private void FixVertexTruncations( IIndexedTriangleMesh mesh, ref List<Vector3> newPoints, ref List<IndexedTriangle> newTrianles )// ref List<IndexedTriangle> triangles)
        {
            Vector3[] pts = mesh.Vertices.Select(t => new Vector3(t.X, t.Y, t.Z)).ToArray();

            var xmax = pts.Select(t => t.X).Max();
            var xmin = pts.Select(t => t.X).Min();

            var ymax = pts.Select(t => t.Y).Max();
            var ymin = pts.Select(t => t.Y).Min();

            var zmax = pts.Select(t => t.Z).Max();
            var zmin = pts.Select(t => t.Z).Min();

            var threshold = Math.Max(3000.0, Math.Max(Math.Max(xmax - xmin, ymax - ymin), zmax - zmin));
            threshold /= 1000.00;

            CellMapper map = new CellMapper(pts, threshold); //1m search rage later
            int[] newIndices = Enumerable.Repeat(-1, pts.Count()).ToArray(); //0,1,2,3....

            bool[] duplicates = Enumerable.Repeat(false, pts.Count()).ToArray();

            int counter = 0;
            List<Vector3> ptsToReturn = new List<Vector3>();
            for (int n = 0; n < newIndices.Count(); n++)
            {
                if (newIndices[n] < 0) //hasnt been checked yet
                {
                    var sortedDistances = map.FindNeighbourDistances(pts[n]); //indices of the neighbours in a range of 1m
                    int[] identical = sortedDistances.Where(t => t.Value < 0.000001).Select(t => t.Key).ToArray();

                    if (identical.Count() > 1)
                    {
                        int minIndex = identical.Min();
                        minIndex = counter;
                        counter += 1;
                        for (int k = 0; k < identical.Count(); k++)
                        {
                            int oldIndex = identical[k];
                            newIndices[oldIndex] = minIndex;

                            duplicates[oldIndex] = true;
                        }

                        newIndices[n] = minIndex;
                    }
                    else
                    {
                        newIndices[n] = counter;
                        counter += 1;
                    }

                    duplicates[n] = false;
                    ptsToReturn.Add(pts[n]);
                }
            }
            newPoints = ptsToReturn;

            //option 1: didnt work, dont know why
            //var c = mesh.Triangles.Count();
            //newTrianles = new List<IndexedTriangle>();
            /*foreach (IndexedTriangle t in mesh.Triangles)
            {
                if ((duplicates[t.Vertex1] == true) && (duplicates[t.Vertex2] == true) && (duplicates[t.Vertex3] == true))
                {
                    ;
                }
                else
                {
                    newTrianles.Add(new IndexedTriangle( newIndices[ t.Vertex1], newIndices[t.Vertex2], newIndices[t.Vertex3] ));
                }
            }
            */
            //option 2

            //leave all the vertices as the originals
            //ptsToReturn = mesh.Vertices.Select(t => new Vector3(t.X, t.Y, t.Z)).ToList();
            //newPoints = ptsToReturn;
            IndexedTriangle[] originalTriangles = mesh.Triangles.Select(t => t).ToArray();
            newTrianles = new List<IndexedTriangle>();

            Vector3[] tCenters = mesh.Triangles.Select(t => ((1.0 / 3.0) * (pts[t.Vertex1] + pts[t.Vertex2] + pts[t.Vertex3]))).ToArray();
            CellMapper tmap = new CellMapper(tCenters, threshold);
            duplicates = Enumerable.Repeat(false, tCenters.Count()).ToArray();
            for (int n = 0; n < tCenters.Count(); n++)
            {
                if (duplicates[n] == false)
                {
                    int[] neigh = tmap.FindNeighbourDistances(tCenters[n]).Where(w => w.Value < 0.000001).Select(w => w.Key).ToArray();
                    int cc = neigh.Count();

                    var triangle1 = originalTriangles[n];
                    int v11 = newIndices[triangle1.Vertex1];
                    int v12 = newIndices[triangle1.Vertex2];
                    int v13 = newIndices[triangle1.Vertex3];

                    int minIndex = Math.Min(v11, Math.Min(v12, v13));
                    int maxIndex = Math.Max(v11, Math.Max(v12, v13));
                    int sumOfIndices = v11 + v12 + v13;

                    //bool invertNormal = false;
                    if (neigh.Count() > 1)
                    {
                        for (int k = 0; k < neigh.Count(); k++)
                        {
                            int index = neigh[k];
                            if (index != n)
                            {
                                var triangle2 = originalTriangles[index];
                                int v21 = newIndices[triangle2.Vertex1];
                                int v22 = newIndices[triangle2.Vertex2];
                                int v23 = newIndices[triangle2.Vertex3];

                                int minIndex2 = Math.Min(v21, Math.Min(v22, v23));
                                int maxIndex2 = Math.Max(v21, Math.Max(v22, v23));
                                int sumOfIndices2 = v21 + v22 + v23;

                                if ((minIndex == minIndex2) && (maxIndex == maxIndex2) && (sumOfIndices == sumOfIndices2))
                                {
                                    duplicates[index] = true;
                                    //invertNormal = true;
                                }

                                ;
                            }

                            //if (neigh[k] != n)
                            //    duplicates[neigh[k]] = true;
                        }
                    }

                    //duplicates[n] = false;

                    IndexedTriangle t = originalTriangles.ElementAt(n);
                    int v1 = newIndices[t.Vertex1];
                    int v2 = newIndices[t.Vertex2];
                    int v3 = newIndices[t.Vertex3];

                    //if (invertNormal)
                    //{
                    //   newTrianles.Add(new IndexedTriangle( v13,v12,v11));

                    //}
                    //else
                    //{
                    newTrianles.Add(new IndexedTriangle(v11, v12, v13));
                    //}
                }
            }

            //for (int n = 0; n < duplicates.Count(); n++)
            // {
            //     if (duplicates[n] == false)
            //     {
            //         IndexedTriangle t = originalTriangles.ElementAt(n);

            //         int v1 = newIndices[t.Vertex1];
            //         int v2 = newIndices[t.Vertex2];
            //         int v3 = newIndices[t.Vertex3];
            //         newTrianles.Add(new IndexedTriangle(v1, v2, v3));

            //         //newTrianles.Add(new IndexedTriangle(t.Vertex1, t.Vertex2, t.Vertex3));
            //         //newTrianles.Add(new IndexedTriangle(newIndices[t.Vertex1], newIndices[t.Vertex2], newIndices[t.Vertex3]));
            //     }
            // }

            int total = newTrianles.Count();
            ;
            ;

            //// end up with [0,1,1,2,0,0,2,3,4,5,6,7,7,7,44,45,46,47,8,9,7,7,9,9,2,1,1,11,12,...]

            ;

            //for (int n = 0; n < pts.Count(); n++)
            //{
            //    if (newIndices[n] != n)
            //    {
            //    }
            //    else
            //    {
            //        ptsToReturn.Add(pts[n]);
            //    }
            //}
            //var ww = newIndices.Max();
            //var startCounter = 0;
            //for (int n = 1; n < newIndices.Count(); n++)
            //{
            //    if (newIndices[n] != n) startCounter = Math.Max(startCounter, newIndices[n]);
            //}

            //counter = startCounter;
            //for (int n = 1; n < newIndices.Count(); n++)
            //{
            //    if (newIndices[n] == n)
            //    {
            //        counter += 1;
            //        newIndices[n] = counter;
            //    }
            //}

            //return ptsToReturn;
        }

        private bool ExportAsXYZ( string name, string folder, IIndexedTriangleMesh mesh, Point3 displacement = null )
        {
            if (displacement == null) displacement = new Point3(0.0, 0.0, 0.0);

            var eportFiole = Path.Combine(folder, name);

            using (System.IO.StreamWriter w = new System.IO.StreamWriter(eportFiole))
            {
                //var points = mesh.Vertices.Select(t => new Point3(t.X + displacement.X, t.Y + displacement.Y, t.Z + displacement.Z));

                List<Vector3> newPoints = new List<Vector3>();
                List<IndexedTriangle> newTriangles = new List<IndexedTriangle>();

                FixVertexTruncations(mesh, ref newPoints, ref newTriangles);

                var points = newPoints.Select(t => new Vector3(t.X + displacement.X, t.Y + displacement.Y, t.Z + displacement.Z));

                foreach (Vector3 p in points)
                {
                    string line = p.X + "\t" + p.Y + "\t" + p.Z;
                    w.WriteLine(line);
                }
            }

            return true;
        }

        private void button2_Click( object sender, EventArgs e )
        {
            this.ParentForm.Close();
        }

        private void flowLayoutPanel1_Resize( object sender, EventArgs e )
        {
            objectsGrid.Width = flowLayoutPanel1.Width - 5;
            panel1.Width = flowLayoutPanel1.Width - 5;
            panel2.Width = flowLayoutPanel1.Width - 5;
        }

        private void exportButton_Click( object sender, EventArgs e )
        {
            if ((ExportFolder == null) || (FaultMeshes == null) || (HorizonMeshes == null)) return;

            if (!IsDirectoryWritable(ExportFolder))
            {
                MessageBox.Show("Cannot write in the selected folder", "Access restricted", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!CheckFolderEmpty(ExportFolder))
            {
                const string message = "The selected folder is not empty. Some files may get replaced. Do you want to continue?";
                var result = MessageBox.Show(message, "Export data", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                    return;
            }

            //proceed to export
            Point3 traslation = new Point3(-Traslation.X * (XcheckBox.Checked == true ? 1.0 : 0.0),
                                            -Traslation.Y * (ycheckBox.Checked == true ? 1.0 : 0.0),
                                            -Traslation.Z * (zcheckBox.Checked == true ? 1.0 : 0.0));

            try
            {
                Slb.Ocean.Petrel.PetrelLogger.Status(new string[] { "Exporting data" });

                using (Slb.Ocean.Petrel.IProgress progressBar = Slb.Ocean.Petrel.PetrelLogger.NewProgress(0, 100, Slb.Ocean.Petrel.ProgressType.Cancelable, Cursors.WaitCursor))
                {
                    int totalObjects = HorizonMeshes.Keys.Count() + FaultMeshes.Keys.Count();
                    int objectCounter = 0;
                    progressBar.ProgressStatus = 0;
                    foreach (string name in HorizonMeshes.Keys)
                    {
                        string name2 = System.Text.RegularExpressions.Regex.Replace(name.Trim(new Char[] { ' ', '*', '.', '(', ')' }), @"[^0-9a-zA-Z_]+", "") + ".xyz";
                        ExportAsXYZ(name2, ExportFolder, HorizonMeshes[name], traslation);

                        progressBar.ProgressStatus = Math.Min(100, (int)(100.0 * (++objectCounter) / totalObjects));
                    }

                    foreach (string name in FaultMeshes.Keys)
                    {
                        string name2 = System.Text.RegularExpressions.Regex.Replace(name.Trim(new Char[] { ' ', '*', '.', '(', ')' }), @"[^0-9a-zA-Z_]+", "") + ".ts";
                        ExportAsGoCad(name2, ExportFolder, FaultMeshes[name], traslation);
                        progressBar.ProgressStatus = Math.Min(100, (int)(100.0 * (++objectCounter) / totalObjects));
                    }
                }
                Slb.Ocean.Petrel.PetrelLogger.Status(new string[] { "" });

                MessageBox.Show("Data exported successfully", "Data exported", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error when exporting:\n " + exc.ToString(), "Data export error", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        public static bool CheckFolderEmpty( string path )
        {
            if (string.IsNullOrEmpty(path))
            {
                return true;
            }

            var folder = new DirectoryInfo(path);
            if (folder.Exists)
            {
                return folder.GetFileSystemInfos().Length == 0;
            }

            return true;
        }

        public bool IsDirectoryWritable( string dirPath, bool throwIfFails = false )
        {
            try
            {
                using (FileStream fs = File.Create(
                    Path.Combine(
                        dirPath,
                        Path.GetRandomFileName()
                    ),
                    1,
                    FileOptions.DeleteOnClose)
                )
                { }
                return true;
            }
            catch
            {
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }

        private void selectExportFolderButton_Click( object sender, EventArgs e )
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                ExportFolder = folderBrowserDialog1.SelectedPath;
                folderPresentationBox.Text = ExportFolder;
                folderPresentationBox.Image = PetrelImages.Apply;
            }
        }

        private void xTraslationControl_ValueChanged( object sender, EventArgs e )
        {
            //Traslation = new Point3((double)xTraslationControl.Value, (double)yTraslationControl.Value, (double)zTraslationControl.Value);
        }

        private void xTraslationControl_KeyDown( object sender, KeyEventArgs e )
        {
            if (e.KeyCode == Keys.Enter)
                Traslation = new Point3((double)xTraslationControl.Value, (double)yTraslationControl.Value, (double)zTraslationControl.Value);
        }

        private void folderPresentationBox_KeyDown( object sender, KeyEventArgs e )
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                ExportFolder = string.Empty;
                folderPresentationBox.Text = "";
                folderPresentationBox.Image = null;
            }
        }

        private void faultsPresentationBox_KeyDown( object sender, KeyEventArgs e )
        {
            if ((e.KeyCode == Keys.Delete) || (e.KeyCode == Keys.Back))
            {
                faultsPresentationBox.Text = "";
                objectsGrid.Visible = false;
                objectsGrid.Rows.Clear();
                FaultMeshes.Clear();
                HorizonMeshes.Clear();
            }
        }
    }
}