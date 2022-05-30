using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.CommonData
{


    class CellMapper
    {

        public CellMapper(Vector3[] items, double threshold)
        {
            Items = items;
            Threshold = threshold;
            MapItems();
        }

        public Vector3[] Items { get { return _items; } set { _items = value; dirty = true; } }

        public double Threshold { get { return _threshold; } set { _threshold = value; dirty = true; } }


        public void MapItems()
        {


            double[] x = _items.Select(t => t.X).ToArray();
            double[] y = _items.Select(t => t.Y).ToArray();
            double[] z = _items.Select(t => t.Z).ToArray();

            min = new double[] { x.Min(), y.Min(), z.Min() };
            max = new double[] { x.Max(), y.Max(), z.Max() };

            double characteristicLength = max[0] - min[0];
            for (int d = 1; d < 3; d++)
                characteristicLength = Math.Max(characteristicLength, max[d] - min[d]);



            double delta = characteristicLength * 0.00001;
            for (int d = 0; d < 3; d++)
            {
                min[d] -= delta;
                max[d] += delta;
            }

            cells = new int[] { 0, 0, 1 };
            invWidth = new double[] { 0.0, 0.0, 0.0 };
            nCells = 1;



            bool keepTrying = true;
            int trials = 0;

            while (keepTrying)
            {
                cellSize = Threshold * 2.0;
                nCells = 1;
                for (int d = 0; d < 3; d++)
                {
                    double length = max[d] - min[d];
                    cells[d] = (int)(length / cellSize);


                    if (cells[d] <= 1)
                    {

                        min[d] -= cellSize;
                        max[d] += cellSize;
                        length = max[d] - min[d];
                        cells[d] = (int)(length / cellSize);
                    }
                    invWidth[d] = (1.0 * cells[d] / length);

                    nCells *= cells[d];

                }

                trials += 1;

                double nCellsOverflow = (int)(2147483591 * 0.75);
                if (nCellsOverflow < nCells)
                {
                    Threshold = Threshold * 2;
                }
                else
                {
                    keepTrying = false;
                }

                if (trials > 3)
                {
                    keepTrying = false;
                    throw (new Exception("Number of cells to big for the given threshold in CellMapping"));
                }

            }



            //cell map 
            nCells = cells[0] * cells[1] * cells[2];
            int N = _items.Count();
            cellList = Enumerable.Repeat(-1, nCells + _items.Count() + 1).ToArray();



            for (int n = 0; n < _items.Count(); n++)
            {

                int cx = (int)((x[n] - min[0]) * invWidth[0]);
                int cy = (int)((y[n] - min[1]) * invWidth[1]);
                int cz = (int)((z[n] - min[2]) * invWidth[2]);
                int c = cx + cy * cells[0] + cz * (cells[0] * cells[1]) + N;

                cellList[n] = cellList[c];
                cellList[c] = n;
            }

            dirty = false;




        }


        public List<int> FindNeighbours(Vector3 p)
        {
            if (dirty)
                MapItems();

            Dictionary<int, float> indexedDistances = new Dictionary<int, float>();

            //the cell of the point 
            int cx = (int)((p.X - min[0]) * invWidth[0]);
            int cy = (int)((p.Y - min[1]) * invWidth[1]);
            int cz = (int)((p.Z - min[2]) * invWidth[2]);
            int c = cx + cy * cells[0] + cz * (cells[0] * cells[1]);

            int N = _items.Count();

            int[] iofx = { 0, 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] iofy = { 0, 0, 1, 1, 1, 0, -1, -1, -1 };
            int[] iofz = { 0, 1, -1 };// 0, 0,  0,  0,  0,  0,  0    };

            List<int> indicesToReturn = new List<int>();

            //scan neighbour cells 
            //z-planes neighbour
            for (int k = 0; k <= 2; k++)
            {
                int zOffset = iofz[k];
                //neighbour cells in the x,y, plane
                for (int offset = 0; offset < 9; offset++)
                {
                    int cx2 = cx + iofx[offset];
                    int cy2 = cy + iofy[offset];
                    int cz2 = cz + zOffset;

                    if ((cx2 < 0) || (cx2 >= cells[0])) {; }// edge cell, do nothing 
                    else
                    if ((cy2 < 0) || (cy2 >= cells[1])) {; }// edge cell, do nothing 
                    else
                    if ((cz2 < 0) || (cz2 >= cells[2])) {; }// edge cell, do nothing 

                    else
                    {
                        int c2 = cx2 + cy2 * cells[0] + cz2 * (cells[0] * cells[1]);

                        int j2 = cellList[c2 + N];
                        while (j2 > -1)
                        {
                            Vector3 dr = p - _items[j2];
                            double squaredDistance = dr * dr;

                            //they are close 
                            if (squaredDistance < Threshold * Threshold)
                            {
                                if (!indicesToReturn.Contains(j2))
                                    indicesToReturn.Add(j2);
                            }


                            j2 = cellList[j2];
                        }

                    }


                }
            }


            return indicesToReturn;
        }

        public List<KeyValuePair<float, int>> FindOrderedNeighbourDistances(Vector3 p)
        {
            Dictionary<int, float> aux = FindNeighbourDistances(p);
            List<KeyValuePair<float, int>> l = new List<KeyValuePair<float, int>>();

            List<int> x = FindNeighbours(p);

            foreach (KeyValuePair<int, float> item in aux.OrderBy(key => key.Value))
                l.Add(new KeyValuePair<float, int>(item.Value, item.Key));


            return l;
        }

        public Dictionary<int, float> FindNeighbourDistances(Vector3 p)
        {
            if (dirty)
                MapItems();

            Dictionary<int, float> indexedDistances = new Dictionary<int, float>();

            //the cell of the point 
            int cx = (int)((p.X - min[0]) * invWidth[0]);
            int cy = (int)((p.Y - min[1]) * invWidth[1]);
            int cz = (int)((p.Z - min[2]) * invWidth[2]);
            int c = cx + cy * cells[0] + cz * (cells[0] * cells[1]);

            int N = _items.Count();

            int[] iofx = { 0, 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] iofy = { 0, 0, 1, 1, 1, 0, -1, -1, -1 };
            int[] iofz = { 0, 1, -1 };// 0, 0,  0,  0,  0,  0,  0    };



            //scan neighbour cells 
            //z-planes neighbour
            for (int k = 0; k <= 2; k++)
            {
                int zOffset = iofz[k];
                //neighbour cells in the x,y, plane
                for (int offset = 0; offset < 9; offset++)
                {
                    int cx2 = cx + iofx[offset];
                    int cy2 = cy + iofy[offset];
                    int cz2 = cz + zOffset;

                    if ((cx2 < 0) || (cx2 >= cells[0])) {; }// edge cell, do nothing 
                    else
                    if ((cy2 < 0) || (cy2 >= cells[1])) {; }// edge cell, do nothing 
                    else
                    if ((cz2 < 0) || (cz2 >= cells[2])) {; }// edge cell, do nothing 

                    else
                    {
                        int c2 = cx2 + cy2 * cells[0] + cz2 * (cells[0] * cells[1]);

                        int j2 = cellList[c2 + N];
                        while (j2 > -1)
                        {
                            Vector3 dr = p - _items[j2];
                            double squaredDistance = dr * dr;

                            //they are close 
                            if (squaredDistance < Threshold * Threshold)
                            {
                                if (!indexedDistances.Keys.Contains(j2))
                                    indexedDistances.Add(j2, (float)squaredDistance);
                            }


                            j2 = cellList[j2];
                        }

                    }


                }
            }


            return indexedDistances;
        }


        double[] min;
        double[] max;


        Vector3[] _items;
        double _threshold;
        int[] cells = { 0, 0, 1 };
        double[] invWidth = { 0.0, 0.0, 0.0 };
        int nCells = 1;
        int[] cellList;
        double cellSize;

        bool dirty;
    }
}
