
using Slb.Ocean.Basics;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Analysis;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.DomainObject.Well;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;


public class ProjectTools
{
    public static string GetOceanFolder()
    {
        string storageFolder = string.Empty;
        if (PetrelProject.IsPrimaryProjectOpen)
        {
            Project proj = PetrelProject.PrimaryProject;
            var info = PetrelProject.GetProjectInfo(DataManager.DataSourceManager);
            var dir = info.ProjectStorageDirectory;
            var file = info.ProjectFile;

            try
            {
                storageFolder = System.IO.Path.Combine(dir.FullName, "Ocean");
            }
            catch (Exception exx)
            {
                string why = exx.ToString();
                storageFolder = string.Empty;
            }
        }

        return storageFolder;
    }

    public static string GetInstallationFolder()
    {
        //string storageFolder = "C:\\Program Files\\Schlumberger\\Petrel 2018\\Extensions\\RestorationPlugin\\";

        string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        return assemblyFolder;

        //return storageFolder;
    }

    public static Color GetRandomColor(int index)
    {
        Color[] colors = new Color[] { Color.AliceBlue, Color.Orange, Color.Green, Color.Cyan, Color.Pink, Color.Beige, Color.White, Color.Yellow, Color.Turquoise, Color.YellowGreen, Color.Salmon, Color.PaleGreen, Color.LightGray };

        int i = index / colors.Length;

        i = index - i * colors.Length;

        return colors[i];
    }

    #region functions

    public static Function CreateOrReplaceFunction(Collection col, string name, List<KeyValuePair<double, double>> functionData)
    {
        Function f = null;
        foreach (Function function in col.Functions)
        {
            if (function.Name == name) f = function;
        }

        if (f == null)
        {
            using (var trans = DataManager.NewTransaction())
            {
                trans.Lock(col);
                f = col.CreateFunction(name);
                trans.Commit();
            }
        }

        var pts = functionData.Select(t => new Point2(t.Key, t.Value)).ToList();
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(f);
            f.Points = pts;
            trans.Commit();
        }

        return f;
    }

    #endregion functions

    #region grid tools

    public static Index3 CellIndex3FromCounter(int n, Grid g)
    {
        int cij = g.NumCellsIJK.I * g.NumCellsIJK.J;
        int klayer = (int)(n / (g.NumCellsIJK.I * g.NumCellsIJK.J));
        int cellIJ = n - klayer * cij;
        int cj = cellIJ / g.NumCellsIJK.I;
        int ci = n - cj * g.NumCellsIJK.I - klayer * (g.NumCellsIJK.I * g.NumCellsIJK.J);

        return new Index3(ci, cj, klayer);
    }

    public static CommonData.Vector3[] GetCellCenters(Grid g)
    {
        Index3 numCellsIJK = g.NumCellsIJK;
        int c = 0, nCells = numCellsIJK.I * numCellsIJK.J * numCellsIJK.K;
        CommonData.Vector3[] cellCenters = new CommonData.Vector3[nCells];

        for (int k = 0; k < numCellsIJK.K; k++)
        {
            for (int j = 0; j < numCellsIJK.J; j++)
            {
                for (int i = 0; i < numCellsIJK.I; i++)
                {
                    Index3 i3 = new Index3(i, j, k);
                    Point3 p = g.GetCellCenter(i3);
                    cellCenters[c++] = new CommonData.Vector3(p.X, p.Y, p.Z);
                }
            }
        }
        return cellCenters;
    }

    public static CommonData.Vector3[] GetCellCentersAtIndices(Grid g, List<Index3> indices)
    {
        CommonData.Vector3[] cellCenters = new CommonData.Vector3[indices.Count()];
        int n = 0;
        foreach (Index3 i in indices)
        {
            Point3 p = g.GetCellCenter(i);
            cellCenters[n++] = new CommonData.Vector3(p.X, p.Y, p.Z);
        }

        return cellCenters;
    }

    public static CommonData.Vector3[] GetCellCenters(PropertyCollection p)
    {
        return ProjectTools.GetCellCenters(p.Grid);
    }

    public float[] GetZoneAverages(string propertyName, Grid g)
    {
        return null;
    }

    //public static float[] GetPropertyValues(Property p)
    //{
    //    Grid g = p.Grid;
    //    var numCellsIJK = g.NumCellsIJK;

    //    float[] values = Enumerable.Repeat(float.NaN, numCellsIJK.I * numCellsIJK.J * numCellsIJK.K).ToArray();

    //    using (var aux = p.SpecializedAccess.OpenFastPropertyIndexer())
    //    {
    //        int c = 0;
    //        for (var k = 0; k < numCellsIJK.K; k++)
    //            for (var j = 0; j < numCellsIJK.J; j++)
    //                for (var i = 0; i < numCellsIJK.I; i++)
    //                    values[c++] = (aux[i, j, k]);
    //    }
    //    return values;
    //}

    public static float[] GetGridPropertyValues(PropertyCollection pc, string name, Template temp = null)
    {
        Property p = ProjectTools.GetProperty(pc, name, temp);
        if ((p == Property.NullObject) || (p == null)) return null;

        Index3 numCellsIJK = pc.Grid.NumCellsIJK;
        int c = 0, nCells = numCellsIJK.I * numCellsIJK.J * numCellsIJK.K;
        float[] values = Enumerable.Repeat(float.NaN, nCells).ToArray();

        using (var aux = p.SpecializedAccess.OpenFastPropertyIndexer())
        {
            for (var k = 0; k < numCellsIJK.K; k++)
                for (var j = 0; j < numCellsIJK.J; j++)
                    for (var i = 0; i < numCellsIJK.I; i++)
                    {
                        values[c++] = (aux[i, j, k]);
                    }
        }
        //var ss = p[i, j, k];

        //if (ss != null)
        //{
        //    ;
        return values;
    }

    public static Property ClusterBySimilarity(List<Property> l, int nClusters = 10, string propertyIndexName = "MatId", PropertyCollection col = null)
    {
        if (l.Count < 1)
        {
            return Property.NullObject;
        }
        var grid = l[0].Grid;
        var distance = new double[grid.NumCellsIJK.I * grid.NumCellsIJK.J * grid.NumCellsIJK.K];
        for (var n = 0; n < distance.Length; n++)
        {
            distance[n] = 0.0;
        }

        foreach (var t in l)
        {
            var ymValues = GetNormalizedPropertyValues(t); //,  out minym,  out rangeym );

            Console.WriteLine(@"size1 = " + ymValues.Count + @" size2 = " + distance.Length);
            for (var i = 0; i < ymValues.Count; i++)
            {
                var d2 = ymValues[i] * ymValues[i];
                distance[i] = distance[i] + d2;
            }
        }
        for (var n = 0; n < distance.Length; n++)
        {
            distance[n] = Math.Sqrt(distance[n]);
        }

        double maxDistance = distance.Max(), minDistance = distance.Min();
        //this is neeeded in case all the variables are constant, i.e. no variation
        maxDistance = maxDistance + 1.00e-18;
        minDistance = minDistance - 1.00e-18;
        var rangeDistance = maxDistance - minDistance;
        Console.WriteLine(@" range = " + rangeDistance);

        var delta = rangeDistance / nClusters;

        if (col == PropertyCollection.NullObject || col == null)
        {
            col = l[0].PropertyCollection; // Grid.PropertyCollection;
        }

        var index = GetOrCreateProperty(col, propertyIndexName, PetrelProject.WellKnownTemplates.MiscellaneousGroup.GeneralInteger);

        var nn = 0;
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(index);
            using (var d = index.SpecializedAccess.OpenFastPropertyIndexer())
            {
                for (var k = 0; k < grid.NumCellsIJK.K; k++)
                {
                    for (var j = 0; j < grid.NumCellsIJK.J; j++)
                    {
                        for (var i = 0; i < grid.NumCellsIJK.I; i++)
                        {
                            var i3 = new Index3(i, j, k);
                            d[i3[0], i3[1], i3[2]] = (int)(distance[nn++] / delta);
                        }
                    }
                }
            }
            trans.Commit();
        } //transaction

        return index;
    } //body

    public static void MapGridPropertyToPointAttribute(Property index, PointSet pSet)
    {
        if (index == Property.NullObject || pSet == PointSet.NullObject)
        {
            return;
        }
        var p = index.SpecializedAccess.OpenFastPropertyIndexer();
        var pProperty = GetOrCreatePointProperty(pSet, index.Name, index.Template);

        var grid = index.Grid;
        var pts = new List<Point3>(pSet.Points);
        //System.Console.WriteLine("nPoints " + pts.Count);
        var values = new List<double>();

        foreach (var point in pts)
        {
            var i3 = grid.GetCellAtPoint(point);
            //System.Console.WriteLine("checking point " + point);
            values.Add(i3 != null ? p[i3[0], i3[1], i3[2]] : double.NaN);
        }
        var records = pProperty.Records;
        var pointPropertyRecords = records as PointPropertyRecord[] ?? records.ToArray();

        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(pProperty);
            pProperty.SetRecordValues(pointPropertyRecords, values);
            trans.Commit();
        }
    }

    //tested
    public static Grid GetGridByName(string name)
    {
        Grid toReturn = null;
        foreach (var g in PillarGridRoot.Get(PetrelProject.PrimaryProject).Grids)
        {
            if (g.Name == name)
            {
                toReturn = g;
            }
        }

        return toReturn;
    }

    public static Property GetProperty(PropertyCollection pc, string name, Template temp = null)
    {
        var toReturn = Property.NullObject;
        if (pc == null) return toReturn;

        foreach (var pp in pc.Properties)
        {
            if (pp.Name == name)
            {
                if (temp == null || pp.Template == temp)
                {
                    toReturn = pp;
                    break;
                }
            }
        }
        return toReturn;
    }

    public static Property GetOrCreateProperty(PropertyCollection pc, string name, Template temp = null)
    {
        var toReturn = Property.NullObject;
        foreach (var pp in pc.Properties)
        {
            if (pp.Name == name)
            {
                if (temp == null || pp.Template.TemplateType == temp.TemplateType)
                {
                    toReturn = pp;
                    break;
                }
            }
        }

        if (toReturn == null)
        {
            if (temp == null)
            {
                temp = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
            }
            using (var trans = DataManager.NewTransaction())
            {
                trans.Lock(pc);
                toReturn = pc.CreateProperty(temp);
                toReturn.Name = name;
                trans.Commit();
            }
        }
        return toReturn;
    }

    public static PropertyCollection GetOrCreatePropertyCollection(Grid grid, String collectionName)
    {
        PropertyCollection toReturn = PropertyCollection.NullObject;
        foreach (PropertyCollection pp in grid.PropertyCollection.PropertyCollections)
            if (pp.Name == collectionName) { toReturn = pp; break; }

        if (toReturn == null)
        {
            PropertyCollection pc = grid.PropertyCollection;//.CreatePropertyCollection(collectionName);
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(pc);
                toReturn = pc.CreatePropertyCollection(collectionName);
                toReturn.Name = collectionName;
                trans.Commit();
            }
        }
        return toReturn;
    }

    public static List<float> GetNormalizedPropertyValues(Property ymp, out float minValue, out float rangeValue)
    {
        var ymValues = new List<float>();
        var grid = ymp.Grid;
        using (var ym = ymp.SpecializedAccess.OpenFastPropertyIndexer())
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

        minValue = ymValues.Min();
        rangeValue = ymValues.Max() - minValue;
        if (Math.Abs(rangeValue) < 1.0e-8)
        {
            for (var n = 0; n < ymValues.Count; n++)
            {
                ymValues[n] = 0.0f;
            }
        }
        else
        {
            for (var n = 0; n < ymValues.Count; n++)
            {
                ymValues[n] = (ymValues[n] - minValue) / rangeValue;
            }
        }
        return ymValues;
    }

    public static void SetValues(Property p, List<float> idValues)
    {
        if (p == Property.NullObject)
        {
            return;
        }
        var pp = p.SpecializedAccess.OpenFastPropertyIndexer();
        var g = p.Grid;

        var counter = 0;
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(p);
            for (var k = 0; k < g.NumCellsIJK.K; k++)
            {
                for (var j = 0; j < g.NumCellsIJK.J; j++)
                {
                    for (var i = 0; i < g.NumCellsIJK.I; i++)
                    {
                        pp[new Index3(i, j, k)] = idValues[counter++];
                    }
                }
            }
            trans.Commit();
        }
    }

    public static void SetValue(ref Property prop, float value)
    {
        if (prop == null) return;
        var pp = prop.SpecializedAccess.OpenFastPropertyIndexer();
        using (ITransaction trans = DataManager.NewTransaction())
        {
            trans.Lock(prop);
            Index3 i3 = prop.NumCellsIJK;

            for (int k = 0; k < i3[2]; k++)

                for (int j = 0; j < i3[1]; j++)
                    for (int i = 0; i < i3[0]; i++)
                        pp[i, j, k] = value;
            trans.Commit();
        }
    }

    public static void SetValues(ref Property prop, List<Index3> indices, List<float> values)
    {
        if (prop == null) return;
        var pp = prop.SpecializedAccess.OpenFastPropertyIndexer();
        using (ITransaction trans = DataManager.NewTransaction())
        {
            trans.Lock(prop);
            for (int n = 0; n < indices.Count; n++)
                pp[indices[n]] = values[n];
            trans.Commit();
        }
    }

    public static List<float> GetNormalizedPropertyValues(Property ymp)
    {
        float minValue, rangeValue;
        return GetNormalizedPropertyValues(ymp, out minValue, out rangeValue);
    }

    public static List<float> GetPropertyValues(Property p)
    {
        if (p == Property.NullObject)
        {
            return null;
        }

        var ymValues = new List<float>();
        var grid = p.Grid;
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

        return ymValues;
    }

    public static List<Index3> GetIndicesWithPropertyInRange(Property p, float min, float max)
    {
        List<Index3> indices = new List<Index3>();
        var grid = p.Grid;

        using (var ym = p.SpecializedAccess.OpenFastPropertyIndexer())
        {
            for (var k = 0; k < grid.NumCellsIJK.K; k++)
            {
                for (var j = 0; j < grid.NumCellsIJK.J; j++)
                {
                    for (var i = 0; i < grid.NumCellsIJK.I; i++)
                    {
                        var i3 = new Index3(i, j, k);
                        var value = ym[i3[0], i3[1], i3[2]];
                        if ((value > min) && (value < max))
                        {
                            indices.Add(i3);
                        }
                    }
                }
            }
        }

        return indices;
    }

    public static void GetValidPropertyValues(Property p, ref List<float> values, ref List<Index3> indices, int kmin = -1, int kmax = -1)
    {
        if (p == Property.NullObject)
        {
            return;
        }

        values = new List<float>();
        indices = new List<Index3>();

        var grid = p.Grid;
        if (kmin < 0) kmin = 0;
        if (kmax < 0) kmax = grid.NumCellsIJK.K;

        using (var ym = p.SpecializedAccess.OpenFastPropertyIndexer())
        {
            for (var k = kmin; k < kmax; k++)
            {
                for (var j = 0; j < grid.NumCellsIJK.J; j++)
                {
                    for (var i = 0; i < grid.NumCellsIJK.I; i++)
                    {
                        float value = ym[i, j, k];//, i3[1], i3[2];

                        if ((float.IsNaN(value)) || (float.IsNaN(value)))
                        {
                            ;
                        }
                        else
                        {
                            indices.Add(new Index3(i, j, k));
                            values.Add(value);
                        }
                    }
                }
            }
        }
    }

    public static List<float> GetPropertyValuesAtIndices(Property p, List<Index3> indices)
    {
        if (p == Property.NullObject)
        {
            return null;
        }

        List<float> values = new List<float>();
        using (var ym = p.SpecializedAccess.OpenFastPropertyIndexer())
        {
            foreach (Index3 index in indices)
                values.Add(ym[index]);
        }

        return values;
    }

    //tested

    #endregion grid tools

    #region collections

    public static Collection GetCollectionByName(string name, Project prj = null)
    {
        if (!PetrelProject.IsPrimaryProjectOpen)
        {
            return Collection.NullObject;
        }

        if (prj == null)
        {
            prj = PetrelProject.PrimaryProject;
        }
        var col = Collection.NullObject;
        foreach (var c in prj.Collections)
        {
            if (c.Name.Equals(name))
            {
                col = c;
            }
        }
        return col;
    }

    public static Collection GetOrCreateCollectionByName(string name, Collection parent)
    {
        var c = Collection.NullObject;
        if ((parent == null) || (parent == Collection.NullObject))
        {
            return c;
        }

        foreach (var col in parent.Collections)
        {
            if (col.Name.Equals(name))
            {
                c = col;
            }
        }

        if (c == null)
        {
            using (var trans = DataManager.NewTransaction())
            {
                trans.Lock(parent);
                c = parent.CreateCollection(name);
                trans.Commit();
            }
        }

        return c;
    }

    public static Collection GetOrCreateCollectionByName(string name, Project prj = null)
    {
        if (!PetrelProject.IsPrimaryProjectOpen)
        {
            return Collection.NullObject;
        }

        if (prj == null)
        {
            prj = PetrelProject.PrimaryProject;
        }
        var col = GetCollectionByName(name, prj);
        if (col == null)
        {
            using (var trans = DataManager.NewTransaction())
            {
                trans.Lock(prj);
                col = prj.CreateCollection(name);
                trans.Commit();
            }
        }
        return col;
    }

    public static void RenameCollection(Collection c, string name)
    {
        if (c.Name == name)
        {
            return;
        }
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(c);
            c.Name = name;
            trans.Commit();
        }
    }

    #endregion collections

    #region lines and points

    public static PointSet CreatePointSet(ref Collection scol, List<Point3> pts, string name = "Pts")
    {
        PointSet ps;
        var points = pts.ToArray();
        var pss = new Point3Set(points);
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(scol);
            ps = scol.CreatePointSet(name);
            trans.Commit();
        }
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(ps);
            ps.Points = pss;
            trans.Commit();
        }

        return ps;
    }

    public static PointSet GetOrCreatePointSet(ref Collection scol, List<Point3> pts, string name = "Pts")
    {
        PointSet ps;
        foreach (PointSet pointSet in scol.PointSets)
        {
            if (pointSet.Name == name)
                return pointSet;
        }

        var points = pts.ToArray();
        var pss = new Point3Set(points);
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(scol);
            ps = scol.CreatePointSet(name);
            trans.Commit();
        }
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(ps);
            ps.Points = pss;
            trans.Commit();
        }

        return ps;
    }

    public static PolylineSet GetPolylineByName(string name, Collection col = null)
    {
        if (!PetrelProject.IsPrimaryProjectOpen)
        {
            return PolylineSet.NullObject;
        }

        var ret = PolylineSet.NullObject;
        IEnumerable<PolylineSet> sets;

        if (col == null || col == Collection.NullObject)
        {
            sets = PetrelProject.PrimaryProject.PolylineSets;
        }
        else
        {
            sets = col.PolylineSets;
        }
        foreach (var s in sets)
        {
            if (s.Name == name)
            {
                return s;
            }
        }

        return ret;
    }

    public static List<Point3> GetPolylinePoints(Polyline line)
    {
        var pts = new List<Point3>(); // line.GetEnumerator() as List<Point3> );
        var it = line.GetEnumerator();
        while (it.MoveNext())
        {
            pts.Add(it.Current);
        }
        return pts;
    }

    public static List<Point3> GetPolylineSetPoints(PolylineSet lines)
    {
        var pts = new List<Point3>();
        foreach (var pt in lines.Points)
        {
            pts.Add(pt);
        }
        return pts;
    }

    public static PointProperty GetPointProperty(PointSet pointSet, string name)
    {
        PointProperty x = null;

        foreach (var p in pointSet.Properties)
        {
            if (p.Name == name)
            {
                x = p;
            }
        }

        return x;
    }

    public static PointProperty GetOrCreatePointProperty(PointSet pointSet, string name, Template t = null) //Template.NullObject)
    {
        PointProperty x = null;

        foreach (var p in pointSet.Properties)
        {
            if (p.Name == name)
            {
                if (t == null || t == Template.NullObject || t == p.Template)
                {
                    x = p;
                    break;
                }
            }
        }

        if (x == null)
        {
            if (t == Template.NullObject)
            {
                t = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
            }

            using (var trans = DataManager.NewTransaction())
            {
                trans.Lock(pointSet);
                x = pointSet.CreateProperty(t, name);
                trans.Commit();
            }
        }

        return x;
    }

    public static bool SetPropertyValues(PointProperty p, double[] values)
    {
        var records = p.Records;
        List<PointPropertyRecord> recordsList = p.Records.ToList();

        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(p);
            p.SetRecordValues(records, values);
            trans.Commit();
        }

        return true;
    }

    public static bool IsInside3DBox(Point3 pt, double[] limits)
    {
        return IsInsideBox(pt, limits, 3);
    }

    public static bool IsInside2DBox(Point3 pt, double[] limits)
    {
        return IsInsideBox(pt, limits, 2);
    }

    private static bool IsInsideBox(Point3 pt, double[] limits, int DIMS)
    {
        bool retValue = true;
        double[] r = { pt.X, pt.Y, pt.Z };
        for (int d = 0; d < DIMS; d++)
            retValue &= ((r[d] > limits[2 * d]) && (r[d] < limits[2 * d + 1]));
        return retValue;
    }

    #endregion lines and points
};

public class BoreholeTools
{
    public static bool IsPointInSegment(Point3 p, Point3 s1, Point3 s2)
    {
        double tolerance = 0.000001;
        Vector3 po = p - s1;
        Vector3 r12 = s2 - s1;
        var sLength = (Vector3.Dot(r12, r12));

        double projection = Vector3.Dot(po, r12);///sLength;// / Math.Sqrt(Vector3.Dot(r12, r12));
        if (projection / sLength < -tolerance) return false;
        projection *= projection;
        projection /= sLength;// Vector3.Dot(r12, r12);
        return ((projection >= -tolerance) && (projection <= sLength * (1.0 + tolerance)));
    }

    public static List<Point3> GetTrayectoryPoints(Borehole w)
    {
        return w.Trajectory.TrajectoryPolylineRecords.Select(t => t.Point).ToList();
    }

    //intersections
    //dirty but works !!
    public static List<Tuple<FracturePatch, double>> intersectBoreholeFractureNetworkV3(Borehole well, FractureNetwork dfn, double minMD = 0, double maxMD = 999999999)
    {
        List<Tuple<FracturePatch, double>> patches = new List<Tuple<FracturePatch, double>>();
        /*
        Polyline3 line = well.Trajectory.Polyline as Slb.Ocean.Geometry.Polyline3;
        List<Point3> pts = new List<Point3>();
        IEnumerator<Point3> it = line.GetEnumerator();
        while (it.MoveNext()) { pts.Add(it.Current); }
        PetrelLogger.InfoOutputWindow("We've got a number of points here of " + pts.Count);

        if (pts.Count < 1) return patches;

        PetrelLogger.InfoOutputWindow(well.Layer + " " + dfn.Layer + " " + dfn.FracturePatchCount + " " + pts.Count);

        //all of this is just to reduce the computation time A LOT
        double xmin = pts[0].X, xmax = xmin, ymin = pts[0].Y, ymax = ymin, zmax = pts[0].Z, zmin = zmax;
        foreach (Point3 p in pts)
        {
            xmin = (p.X < xmin ? p.X : xmin); ymin = (p.Y < ymin ? p.Y : ymin); zmin = (p.Z < zmin ? p.Z : zmin);
            xmax = (p.X > xmax ? p.X : xmax); ymax = (p.Y > ymax ? p.Y : ymax); zmax = (p.Z > zmax ? p.Z : zmax);
        }
        double maxLength = 0.0;
        foreach (FracturePatch f in dfn.FracturePatches)
        {
            double aux = Vector3.Dot(f.MajorAxis, f.MajorAxis);
            maxLength = (aux > maxLength ? aux : maxLength);
        }
        maxLength = Math.Sqrt(maxLength);

        double x = well.Transform(Domain.MD, minMD, Domain.X); // Access by MD, no point in specifying limit.
        double y = well.Transform(Domain.MD, minMD, Domain.Y);
        double z = well.Transform(Domain.MD, minMD, Domain.ELEVATION_DEPTH);
        double x1 = well.Transform(Domain.MD, maxMD, Domain.X); // Access by MD, no point in specifying limit.
        double y1 = well.Transform(Domain.MD, maxMD, Domain.Y);
        double z1 = well.Transform(Domain.MD, maxMD, Domain.ELEVATION_DEPTH);

        double minZMD = Math.Min(z, z1);
        double maxZMD = Math.Max(z, z1);
        double minYMD = Math.Min(y, y1);
        double maxYMD = Math.Max(y, y1);
        double minXMD = Math.Min(x, x1);
        double maxXMD = Math.Max(x, x1);
        if (zmax > maxZMD) zmax = maxZMD;
        if (zmin < minZMD) zmin = minZMD;
        if (ymax > maxYMD) ymax = maxYMD;
        if (ymin < minYMD) ymin = minYMD;
        if (xmax > maxXMD) xmax = maxXMD;
        if (xmin < minXMD) xmin = minXMD;

        xmin -= maxLength; xmax += maxLength; zmin -= maxLength; zmax += maxLength; ymin -= maxLength; ymax += maxLength;

        //up to here.
        PetrelLogger.InfoOutputWindow("xlimits: " + xmin + " " + xmax);
        PetrelLogger.InfoOutputWindow("ylimits: " + ymin + " " + ymax);
        PetrelLogger.InfoOutputWindow("zlimits: " + zmin + " " + zmax);
        PetrelLogger.InfoOutputWindow("trace: " + maxLength);

        Point3 center = null;
        List<FracturePatch> list = new List<FracturePatch>();

        int yyy = 0;
        using (IProgress p = PetrelLogger.NewProgress(5000, 11000))
        {
            p.SetProgressText("Finding intersections");

            foreach (FracturePatch f in dfn.FracturePatches)
            {
                //double progress = 100.00*(double)(yyy++) / ((double)(dfn.FracturePatchCount));
                //p.ProgressStatus = (int)(progress);//100 *(int)( ((double)yyy / (double)dfn.FracturePatchCount));

                //this goes with the previous lines to reduce computational time.
                center = f.Center;
                //if ((center.X < xmin) || (center.X > xmax) || (center.Y < ymin) || (center.Y > ymax) || (center.Z < zmin) || (center.Z > zmax))
                //continue;

                Direction3 normal = new Direction3(Vector3.Cross(f.MinorAxis, f.MajorAxis));
                Plane3 plane = new Plane3(f.Center, normal);

                for (int i = 0; i < pts.Count - 1; i++)
                {
                    //if ((pts[i].Z > maxZMD) && (pts[i + 1].Z > maxZMD)) continue;
                    //if ((pts[i].Z < minZMD) && (pts[i + 1].Z < minZMD)) continue;

                    //bool closeEnough = false;
                    Vector3 dr = new Vector3(f.Center - pts[i]);
                    Vector3 r12 = new Vector3(pts[i + 1] - pts[i]);
                    double l1 = Vector3.Dot(dr, r12);
                    double l3 = dr.X * dr.X + dr.Y * dr.Y + dr.Z * dr.Z;

                    if (l3 - l1 * l1 < 0.25 * Vector3.Dot(f.MajorAxis, f.MajorAxis)) //closeEnough = true;
                    {
                        Segment3 segment = new Segment3(pts[i + 1], pts[i]);
                        Point3 interSection = Plane3Extensions.Intersect(plane, segment);
                        if ((interSection != Point3.Null) && (ProjectTools.isPointInFracturePatch(interSection, f)))
                        {
                            double md = getWellMDAtPosition(well, interSection);
                            Tuple<FracturePatch, double> item = new Tuple<FracturePatch, double>(f, md);
                            System.Console.WriteLine("added dip = " + item.Item1.Dip);
                            patches.Add(item);
                            break;
                        }
                    }
                }//points loop
            }//fracs loop
        }//progress bar
        */

        /* this was commented before
        List<FracturePatch> fracs = new List<FracturePatch>();
        List<double> mds = new List<double>();
        for (int i = 0; i < patches.Count; i++) { fracs.Add(patches[i].Item1); mds.Add(patches[i].Item2); }

        //THIS IS A PATCH. WE WILL NOW SORT THE TUPLES SO THE MD IS INCREASING.
        for (int passes = 0; passes < mds.Count - 1; passes++)
        {
            for (int j = 0; j < mds.Count - passes - 1; j++)
            {
                int comparison = 1;// = DateTime.Compare( x[j].Date, x[j+1].Date);
                double md1 = mds[j];//.Item2;
                double md2 = mds[j + 1];//.Item2;

                FracturePatch ff1 = fracs[j];//.Item1;
                FracturePatch ff2 = fracs[j + 1];//.Item1;

                //double z1 = x[j].MD, z2 = x[j + 1].MD;
                if (md1 < md2) comparison = 0;

                if (comparison > 0)
                {
                    //WellLogSample hold = x[j];
                    //x[j] = x[j + 1];
                    //x[j + 1] = hold;
                    double hold = mds[j];//.Item2;
                    mds[j] = mds[j + 1];//.Item2;
                    mds[j + 1] = hold;

                    FracturePatch fhold = fracs[j];//.Item2;
                    fracs[j] = fracs[j + 1];//.Item2;
                    fracs[j + 1] = fhold;
                }
            }
        }  //Bubble Sorting finished
        */

        /*
        patches.Clear();
        for (int i = 0; i < mds.Count; i++)
        {
            patches.Add(new Tuple<FracturePatch, double>(fracs[i], mds[i]));
        }
        */
        // PetrelLogger.InfoOutputWindow("About to return here a count of patches of " + patches.Count);
        //  foreach (Tuple<FracturePatch, double> t in patches)
        //      System.Console.WriteLine("returning frac index = " + t.Item1.Index);

        return patches;
    }//body

    public static bool isPointInFracturePatch(Point3 p, FracturePatch f1)
    {
        if (p == Point3.Null) return false;
        if (f1 == null) return false;

        Vector3 minorF1 = f1.MinorAxis;
        Vector3 majorF1 = f1.MajorAxis;
        bool returnvalue = false;

        Vector3 v = new Vector3(p.X - f1.Center.X, p.Y - f1.Center.Y, p.Z - f1.Center.Z);
        Vector3 normal = Vector3.Cross(f1.MinorAxis, f1.MajorAxis).ToNormalized();

        if (Math.Abs(Vector3.Dot(v, normal)) > 0.1) return false;

        double d1 = Vector3.Dot(v, majorF1);
        double d2 = Vector3.Dot(v, minorF1);
        d1 = d1 / (f1.TraceLength * f1.TraceLength);
        d2 = d2 * f1.ShapeFactor * f1.ShapeFactor / (f1.TraceLength * f1.TraceLength);
        if ((d1 <= 0.5001f) && (d1 >= -0.5001f) && (d2 <= 0.5001f) && (d2 >= -0.5001f)) { returnvalue = true; }

        return returnvalue;
    }

    //related, private
    public static float GetWellMDAtPosition(Borehole b, Point3 p)
    {
        //get the pos at md = 0
        Point3 p1;
        float foundMD = (float)(b.MDRange.Min);
        double x = b.Transform(Domain.MD, foundMD, Domain.X); // Access by MD, no point in specifying limit.
        double y = b.Transform(Domain.MD, foundMD, Domain.Y);
        double z = b.Transform(Domain.MD, foundMD, Domain.ELEVATION_DEPTH);
        p1 = new Point3(x, y, z);

        //System.Console.WriteLine("pt ad md = 0 " + p1 );
        bool keepLooking = true;
        int counter = 0;
        double distance = p1.Distance(p);
        double prevdistance = distance;
        double sign = 1.0;
        if (distance < 0.01) keepLooking = false;

        while (keepLooking)
        {
            counter += 1;
            foundMD += (float)(0.9 * distance * sign);

            x = b.Transform(Domain.MD, foundMD, Domain.X); // Access by MD, no point in specifying limit.
            y = b.Transform(Domain.MD, foundMD, Domain.Y);
            z = b.Transform(Domain.MD, foundMD, Domain.ELEVATION_DEPTH);
            p1 = new Point3(x, y, z);

            distance = p1.Distance(p);
            if (distance < 0.005) { keepLooking = false; } //close enough
            if (foundMD > b.MDRange.Max) { keepLooking = false; } //doesnt get better than this
            if (counter > 20) { keepLooking = false; } //dont try to hard, get an approx

            if (distance > prevdistance) sign = -1;
            else sign = 1; //PetrelTreeService.GetObject("/Inputs/Points 1");

            if (Math.Abs(distance - prevdistance) < 0.01) { keepLooking = false; } //further corrections will not be better
            prevdistance = distance;
        }

        return foundMD;
    }//getMD

    public static WellLog GetOrCreateLog(string intendedName, Borehole well, Template templateInput, string subFolder = null)
    {
        var template = Template.NullObject;
        if (templateInput == null)
        {
            template = PetrelProject.WellKnownTemplates.MiscellaneousGroup.General;
        }
        else
        {
            template = templateInput;
        }

        var wellRoot = WellRoot.Get(PetrelProject.PrimaryProject);
        var found = false;
        var rootFolder = wellRoot.LogVersionCollection;
        WellLogVersion newVersion = null;

        if (rootFolder.GetUniqueName(intendedName) != intendedName)
        {
            var allVersions = wellRoot.AllWellLogVersions;
            foreach (var logVersion in allVersions)
            {
                if (logVersion.Name == intendedName && (template == Template.NullObject || logVersion.Template == template))
                {
                    found = true;
                    newVersion = logVersion;
                    break;
                }
            }
        }

        if (!found)
        {
            using (var trans = DataManager.NewTransaction())
            {
                var rootLvColl = wellRoot.LogVersionCollection;
                trans.Lock(rootLvColl);
                newVersion = rootLvColl.CreateWellLogVersion(intendedName, template);
                trans.Commit();
            }
        } /*
                else
                {
                    foreach (WellLogVersion wellLogVersion in wellRoot.AllWellLogVersions)
                    {
                        if (wellLogVersion.Layer == intendedName) { newVersion = wellLogVersion; break; }
                    }
                }*/

        var xx = WellLog.NullObject;
        if (!well.Logs.CanCreateWellLog(newVersion))
        {
            xx = well.Logs.GetWellLog(newVersion);
        }
        else
        {
            using (var trans = DataManager.NewTransaction())
            {
                trans.Lock(well);
                xx = well.Logs.CreateWellLog(newVersion);
                trans.Commit();
            }
        }
        return xx;
    } //getorcreateLog

    private static void sortSamples(List<WellLogSample> x)
    {
        for (var passes = 0; passes < x.Count - 1; passes++)
        {
            for (var j = 0; j < x.Count - passes - 1; j++)
            {
                var comparison = 1; // = DateTime.Compare( x[j].Date, x[j+1].Date);
                double z1 = x[j].MD, z2 = x[j + 1].MD;
                if (z1 < z2)
                {
                    comparison = 0;
                }

                if (comparison > 0)
                {
                    var hold = x[j];
                    x[j] = x[j + 1];
                    x[j + 1] = hold;
                }
            }
        } //Bubble Sorting finished
    }

    private static void sortSamples(List<DictionaryWellLogSample> x)
    {
        for (var passes = 0; passes < x.Count - 1; passes++)
        {
            for (var j = 0; j < x.Count - passes - 1; j++)
            {
                var comparison = 1; // = DateTime.Compare( x[j].Date, x[j+1].Date);
                double z1 = x[j].MD, z2 = x[j + 1].MD;
                if (z1 < z2)
                {
                    comparison = 0;
                }

                if (comparison > 0)
                {
                    var hold = x[j];
                    x[j] = x[j + 1];
                    x[j + 1] = hold;
                }
            }
        } //Bubble Sorting finished
    }

    public static void ReplaceLog(Borehole well, WellLog log, List<float> values, List<float> mds, bool sort = true)
    {
        if (values.Count != mds.Count)
        {
            return;
        }
        if (values.Count == 0)
        {
            return;
        }
        if (!log.IsWritable)
        {
            return;
        }

        var valuesCorrected = new List<WellLogSample>();

        for (var i = 0; i < values.Count; i++)
        {
            var sample = new WellLogSample(mds[i], (float)values[i]);
            valuesCorrected.Add(sample);
        }

        ////////
        //   PetrelLogger.InfoOutputWindow("Log = " + log.Layer);
        //for (int k = 0; k < 10; k++)
        //PetrelLogger.InfoOutputWindow("++Sorted samples berofe sorting are " + valuesCorrected[k].MD);

        if (sort)
        {
            sortSamples(valuesCorrected);
        }
        // for (int k = 0; k < valuesCorrected.Count; k++)
        //     PetrelLogger.InfoOutputWindow(log.Layer + " --Sorted samples after sorting are " + valuesCorrected[k].MD);

        var somethingIsWrong = false;
        try
        {
            for (var k = 0; k < valuesCorrected.Count - 1; k++)
            {
                if (valuesCorrected[k + 1].MD < valuesCorrected[k].MD)
                {
                    PetrelLogger.InfoOutputWindow("wrong k =" + k + " " + valuesCorrected[k].MD + " " + valuesCorrected[k + 1].MD);
                }

                if (double.IsNaN(valuesCorrected[k].MD))
                {
                    somethingIsWrong = true;
                }
                //PetrelLogger.InfoOutputWindow("Nan in k =" + k + " = "); ;

                if (double.IsInfinity(valuesCorrected[k].MD))
                {
                    somethingIsWrong = true;
                }
                // PetrelLogger.InfoOutputWindow("inf Nan in k =" + k + " = "); ;

                if (double.IsNaN(valuesCorrected[k].Value))
                {
                    somethingIsWrong = true;
                }
                // PetrelLogger.InfoOutputWindow("Nan in k =" + k + " = "); ;

                if (double.IsInfinity(valuesCorrected[k].Value))
                {
                    somethingIsWrong = true;
                }
                // PetrelLogger.InfoOutputWindow("inf Nan in k =" + k + " = "); ;
            }
            if (somethingIsWrong)
            {
                PetrelLogger.InfoOutputWindow("Something is wrong for log " + log.Name);
            }
            if (!somethingIsWrong)
            {
                using (var trans = DataManager.NewTransaction())
                {
                    //  PetrelLogger.InfoOutputWindow("Entering in the transaction for " + log.Layer);
                    trans.Lock(log);
                    log.Samples = valuesCorrected;
                    //log.Samples.SetSamples(0.0, valuesCorrected);
                    //   PetrelLogger.InfoOutputWindow("Committing the transaction");

                    trans.Commit();
                }
                //   PetrelLogger.InfoOutputWindow("end of the transaction");
            }
        } //try
        catch (Exception e)
        {
            PetrelLogger.InfoOutputWindow("Failure here [1] \n" + e.ToString());
        }

        //PetrelLogger.InfoBox("Leaving routine here 1 ");
    } //replace Log

    public static DictionaryWellLog GetOrCreateDictionaryLog(string intendedName, Borehole well, DictionaryTemplate templateInput)
    {
        var template = templateInput;

        var wellRoot = WellRoot.Get(PetrelProject.PrimaryProject);
        var found = false;
        var rootFolder = wellRoot.LogVersionCollection;
        if (rootFolder.GetUniqueName(intendedName) != intendedName)
        {
            found = true;
        }

        DictionaryWellLogVersion newVersion = null;
        if (!found)
        {
            using (var trans = DataManager.NewTransaction())
            {
                var rootLvColl = wellRoot.LogVersionCollection;
                trans.Lock(rootLvColl);
                newVersion = rootLvColl.CreateDictionaryWellLogVersion(intendedName, template);
                trans.Commit();
            }
        }
        else
        {
            foreach (var wellLogVersion in wellRoot.AllDictionaryWellLogVersions) //AllWellLogVersions)
            {
                if (wellLogVersion.Name == intendedName)
                {
                    newVersion = wellLogVersion;
                    break;
                }
            }
        }

        var xx = DictionaryWellLog.NullObject;
        if (!well.Logs.CanCreateDictionaryWellLog(newVersion))
        {
            xx = well.Logs.GetDictionaryWellLog(newVersion);
        }
        else
        {
            using (var trans = DataManager.NewTransaction())
            {
                trans.Lock(well);
                xx = well.Logs.CreateDictionaryWellLog(newVersion);
                trans.Commit();
            }
        }
        return xx;
    } //getorcreateLog

    public static void ReplaceDictionaryLog(Borehole well, DictionaryWellLog log, List<int> values, List<float> mds, bool sorting = false)
    {
        if (values.Count != mds.Count)
        {
            return;
        }
        if (values.Count == 0)
        {
            return;
        }

        var valuesCorrected = new List<DictionaryWellLogSample>();

        for (var i = 0; i < values.Count; i++)
        {
            var sample = new DictionaryWellLogSample(mds[i], values[i]);
            valuesCorrected.Add(sample);
        }

        ////////
        if (sorting)
        {
            sortSamples(valuesCorrected);
        }
        using (var trans = DataManager.NewTransaction())
        {
            trans.Lock(log);
            log.Samples = valuesCorrected;
            trans.Commit();
        }

        /////
    } //replace Log

    public static Borehole findWellByName(String name, BoreholeCollection bc)
    {
        Borehole toReturn = Borehole.NullObject;
        foreach (Borehole b in bc)
            if (b.Name == name) { toReturn = b; break; }
        return toReturn;
    }

    public static Borehole findWellByName(String name)
    {
        Borehole toReturn = Borehole.NullObject;
        if (!WellRoot.Get(PetrelProject.PrimaryProject).HasBoreholeCollection) return toReturn;

        //primary collection
        BoreholeCollection bc = WellRoot.Get(PetrelProject.PrimaryProject).BoreholeCollection;
        toReturn = findWellByName(name, bc);
        if (toReturn != null) return toReturn;

        foreach (BoreholeCollection x in bc.BoreholeCollections)
        {
            toReturn = findWellByName(name, x);
            if (toReturn != null) return toReturn;
        }
        return toReturn;
    }

    public static List<Borehole> GetAllWells(BoreholeCollection col)
    {
        List<Borehole> wells = new List<Borehole>();
        //BoreholeCollection root = WellRoot.Get(PetrelProject.PrimaryProject).BoreholeCollection;
        foreach (Borehole b in col)//.BoreholeCollections)
            wells.Add(b);

        foreach (BoreholeCollection secondary in col.BoreholeCollections)
            wells.AddRange(GetAllWells(secondary));

        return wells;
    }

    public static PointWellLog GetPointWellLog(Borehole w, PointWellLogVersion wellLogVersion)
    {
        PointWellLog toReturn = null;
        var pointWellLogs = w.Logs.PointWellLogs;
        foreach (PointWellLog log in pointWellLogs)
        {
            if (log.WellLogVersion.Droid == wellLogVersion.Droid) //this guy has the selected data in the PointWellLog  = log
            {
                toReturn = log;
            }
        }

        return toReturn;
    }

    public static PointWellLog GetOrCreatePointWellLog(Borehole well, string name)
    {
        //is it already one with the name name?
        LogVersionCollection rootFolder = WellRoot.Get(PetrelProject.PrimaryProject).LogVersionCollection;
        PointWellLogVersion pointLogVersion = null;
        bool found = false;
        foreach (PointWellLogVersion x in rootFolder.PointWellLogVersions)
            if (x.Name == name) { pointLogVersion = x; found = true; }

        //it doesnt exist, lets create one
        if (!found)
        {
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(rootFolder);
                pointLogVersion = rootFolder.CreatePointWellLogVersion(name);
                trans.Commit();
            }
        }//!found

        //return null;

        PointWellLog xx = PointWellLog.NullObject;
        /*foreach(  PointWellLog pwl in well.Logs.AllPointWellLogVersions.PointWellLogs)
        {if(pwl.WellLogVersion.Layer==name)
         xx = pwl;//well.Logs.GetWellLog(pointLogVersion );
        }*/

        if (!well.Logs.CanCreatePointWellLog(pointLogVersion))
        {
            foreach (PointWellLog pwl in well.Logs.PointWellLogs)
            {
                if (pwl.WellLogVersion.Name == name)
                    xx = pwl;
            }
            PetrelLogger.InfoOutputWindow("It is in the borehole");
        }
        else
        {
            PetrelLogger.InfoOutputWindow("It is NOT in the borehole");
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(well);
                trans.Lock(pointLogVersion);
                xx = well.Logs.CreatePointWellLog(pointLogVersion);
                trans.Commit();
            }
        }

        PetrelLogger.InfoOutputWindow("PointWellLog name = " + xx);
        PetrelLogger.InfoOutputWindow("WellLogVersion name = " + xx.WellLogVersion.Name);
        PetrelLogger.InfoOutputWindow("Well name = " + xx.Borehole.Name);
        //PetrelLogger.InfoOutputWindow("xx name = " + );

        return xx;
    }

    public static List<Borehole> GetBoreholesWithPointLog(PointWellLogVersion pointWellLogVersion)
    {
        BoreholeCollection wellsRoot = WellRoot.Get(PetrelProject.PrimaryProject).BoreholeCollection;
        List<Borehole> allWells = BoreholeTools.GetAllWells(wellsRoot);
        List<Borehole> toReturn = new List<Borehole>();

        foreach (Borehole probeWell in allWells)
        {
            var pointWellLogs = probeWell.Logs.PointWellLogs;

            foreach (PointWellLog log in pointWellLogs)
            {
                if (log.WellLogVersion.Droid == pointWellLogVersion.Droid) //this guy has the selected data in the PointWellLog  = log
                {
                    toReturn.Add(probeWell);
                }
            }
        }

        return toReturn;
    }

    public static void SetPointWellLogMDs(PointWellLog log, List<double> mds)
    {
        List<PointWellLogSample> samples = new List<PointWellLogSample>();
        foreach (double md in mds) samples.Add(new PointWellLogSample(md));

        using (ITransaction trans = DataManager.NewTransaction())
        {
            WellPointPropertyCollection wppc = log.WellLogVersion.PropertyCollection;
            trans.Lock(wppc);
            trans.Lock(log);
            trans.Lock(log.WellLogVersion);

            log.PointWellLogSamples = samples;
            trans.Commit();
        }
    }

    public static void SetPointWellLogAttribute(PointWellLog log, string name, List<float> values, Template t)
    {
        IEnumerable<WellPointProperty> em = log.WellLogVersion.PropertyCollection.Properties;
        bool found = false;
        WellPointProperty prop = null;
        foreach (WellPointProperty p in em)
        {
            if (p.Name == name) { found = true; prop = p; }
        }

        PointWellLogVersion pwlv = log.WellLogVersion as PointWellLogVersion;
        IEnumerable<WellPointProperty> x = pwlv.PropertyCollection.Properties;

        WellPointPropertyCollection wppc = log.WellLogVersion.PropertyCollection;
        if (!found)
        {
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(wppc);
                trans.Lock(log);
                trans.Lock(log.WellLogVersion);
                if (!found) prop = wppc.CreateProperty(t, name);
                trans.Commit();
            }//trans
        }

        using (ITransaction trans = DataManager.NewTransaction())
        {
            trans.Lock(log);
            trans.Lock(wppc);
            trans.Lock(prop);
            trans.Lock(log.WellLogVersion);

            var mySamples = log.PointWellLogSamples.ToArray();
            for (int i = 0; i < values.Count; i++)
            {
                var nativeIndex = mySamples[i].NativeIndex;
                log.GetPropertyAccessAtIndex(nativeIndex).SetPropertyValue(prop, values[i]);
            }
            trans.Commit();
        }
    }//body

    public static void SetPointWellLogAttributeDouble(PointWellLog log, string name, List<double> values, Template t)
    {
        IEnumerable<WellPointProperty> em = log.WellLogVersion.PropertyCollection.Properties;
        bool found = false;
        WellPointProperty prop = null;
        foreach (WellPointProperty p in em)
        {
            if (p.Name == name) { found = true; prop = p; }
        }

        PointWellLogVersion pwlv = log.WellLogVersion as PointWellLogVersion;
        IEnumerable<DictionaryWellPointProperty> x = pwlv.PropertyCollection.DictionaryProperties;

        WellPointPropertyCollection wppc = log.WellLogVersion.PropertyCollection;
        if (!found)
        {
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(wppc);
                trans.Lock(log);
                trans.Lock(log.WellLogVersion);
                if (!found) prop = wppc.CreateProperty(t, name);
                trans.Commit();
            }//trans
        }

        using (ITransaction trans = DataManager.NewTransaction())
        {
            trans.Lock(log);
            trans.Lock(wppc);
            trans.Lock(prop);
            trans.Lock(log.WellLogVersion);

            for (int i = 0; i < values.Count; i++)
                log.GetPropertyAccessAtIndex(i).SetPropertyValue(prop, (float)values[i]);
            trans.Commit();
        }
    }//body

    public static void SetPointWellLogAttribute(PointWellLog log, string name, List<double> mds, List<float> values, Template t)
    {
        SetPointWellLogMDs(log, mds);
        SetPointWellLogAttribute(log, name, values, t);
    }//bod

    public static void SetFracturePointData(Borehole well, string name, List<double> mds, List<float> dips, List<float> azimuths)
    {
        PointWellLog xx = GetOrCreatePointWellLog(well, name);
        //int nSamples = xx.SampleCount;
        /*using (ITransaction trans = DataManager.NewTransaction())
        {
            trans.Lock(xx);
            xx.PointWellLogSamples = new List<PointWellLogSample>();
            //for (int i = 0; i < nSamples; i++) xx.RemoveAt(0);
            trans.Commit();
        }*/
        SetPointWellLogMDs(xx, mds);
        SetPointWellLogAttribute(xx, "Dip angle", dips, PetrelProject.WellKnownTemplates.GeometricalGroup.DipAngle);
        SetPointWellLogAttribute(xx, "Dip azimuth", azimuths, PetrelProject.WellKnownTemplates.GeometricalGroup.DipAzimuth);
    }

    public static Point3 getPositionAtWellMD(Borehole b, double md)
    {
        double x = b.Transform(Domain.MD, md, Domain.X); // Access by MD, no point in specifying limit.
        double y = b.Transform(Domain.MD, md, Domain.Y);
        double z = b.Transform(Domain.MD, md, Domain.ELEVATION_DEPTH);
        Point3 p = new Point3(x, y, z);
        return p;

        //return new Vector3( x,y,z );
    }

    public static Collection getCollectionByName(string name, Project prj = null)
    {
        if (prj == null) prj = PetrelProject.PrimaryProject;
        Collection col = Collection.NullObject;
        foreach (Collection c in prj.Collections) if (c.Name.Equals(name)) col = c;
        return col;
    }

    public static Collection GetOrCreateCollectionByName(string name, Project prj = null)
    {
        Collection col = getCollectionByName(name, prj);
        if (col == null)
        {
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(prj);
                col = prj.CreateCollection(name); trans.Commit();
            }
        }
        return col;
    }
}//class

public class DFNTools
{
    public static void DeleteFractures(FractureNetwork fn)
    {
        if (fn.FractureSetCount < 1) return;

        foreach (FractureSet set in fn.FractureSets)
        {
            using (ITransaction trans = DataManager.NewTransaction())
            {
                trans.Lock(set);
                set.Delete();
                trans.Commit();
            }
        }
    }

    public static bool isPointInFracturePatch(Point3 p, FracturePatch f1)
    {
        if (p == Point3.Null) return false;
        if (f1 == null) return false;

        Vector3 minorF1 = f1.MinorAxis;
        Vector3 majorF1 = f1.MajorAxis;
        bool returnvalue = false;

        Vector3 v = new Vector3(p.X - f1.Center.X, p.Y - f1.Center.Y, p.Z - f1.Center.Z);
        double d1 = Vector3.Dot(v, majorF1);
        double d2 = Vector3.Dot(v, minorF1);
        d1 = d1 / (f1.TraceLength * f1.TraceLength);
        d2 = d2 * f1.ShapeFactor * f1.ShapeFactor / (f1.TraceLength * f1.TraceLength);
        if ((d1 <= 0.50f) && (d1 >= -0.50f) && (d2 <= 0.50f) && (d2 >= -0.50f)) { returnvalue = true; }

        return returnvalue;
    }

    public static FractureNetwork GetOrCreateFractureNetwork(string name)
    {
        FractureNetwork dfn = FractureNetwork.NullObject;

        IFractureNetworkAccess access = PetrelProject.PrimaryProject.GetFractureNetworkAccess();
        foreach (FractureNetwork f in access.FractureNetworks)
            if (name == f.Name) dfn = f;

        if (dfn == null)
        {
            using (ITransaction trans = DataManager.NewTransaction())
            {
                PetrelLogger.InfoOutputWindow("Fracture network not found. Creating one ");
                Project proj = PetrelProject.PrimaryProject;
                trans.Lock(proj);
                dfn = proj.CreateFractureNetwork(name, Domain.ELEVATION_DEPTH);
                trans.Commit();
            }
        }
        return dfn;
    }

    public static void CopyPatches(List<FracturePatch> patches, FractureNetwork dfn2)
    {
        using (ITransaction trans = DataManager.NewTransaction())
        {
            trans.Lock(dfn2);
            foreach (FracturePatch patch in patches)
                dfn2.CreateFracturePatch(patch);
            trans.Commit();
        }

        /*FractureNetwork dfn1 = patches[0].FractureNetwork;
        List<FracturePatchProperty> props = DFNTools.getProperties(dfn1);
        List<double> values = new List<double>();

        foreach (FracturePatchProperty p in props)
        {
            FracturePatchPropertyRecord [] recs = p.Records.ToArray();

            values.Clear();
            foreach (FracturePatch f in patches)
            values.Add( recs[f.Index].Value);

            FracturePatchProperty p2 = DFNTools.getOrCreateProperty(dfn2, p.Layer, p.Template);
            DFNTools.setValues(dfn2, p2, values);
        }*/
    }

    public static FracturePatch[] GetPatchesArray(FractureNetwork dfn)
    {
        FracturePatch[] arr = new FracturePatch[dfn.FracturePatchCount];
        int index = 0;
        foreach (FracturePatch p in dfn.FracturePatches)
            arr[index++] = p;
        return arr;
    }

    public static float DistanceToRegularHeightFieldSurface(Point3 p, object surface)
    {
        var s = surface as RegularHeightFieldSurface;
        var i2 = s.IndexAtPosition(new Point3(p.X, p.Y, p.Z));
        return i2 != null ? (float)(s[(int)i2.I, (int)i2.J] - p.Z) : float.NaN;
    }

    //SIGNED distance, zsurface - z point. If negative, the point is above the surface.
    public static float DistanceToStructuredSurface(Point3 p, object surface)
    {
        var s = surface as StructuredSurface;
        var i2 = s.IndexAtPosition(new Point3(p.X, p.Y, p.Z));
        return i2 != null ? (float)(s.PositionAtIndex(i2).Z - p.Z) : float.NaN;
    }

    public static bool IntersectSurface(FracturePatch f, RegularHeightFieldSurface s)
    {
        //it intersects if the center is above/below and one point is below/above
        bool crossesSurface = false;
        float centerDistance = DistanceToRegularHeightFieldSurface(f.Center, s);
        if (!float.IsNaN(centerDistance))
        {
            int centersign = Math.Sign(centerDistance);
            var pts = f.IndexedTriangleMesh.Vertices;

            foreach (var p in pts)
            {
                float distancePt = DistanceToRegularHeightFieldSurface(p, s);
                if ((!float.IsNaN(distancePt)) && (Math.Sign(distancePt) != centersign))
                {
                    crossesSurface = true;
                    break;
                }
            }
        }

        return crossesSurface;
    }
};

//};