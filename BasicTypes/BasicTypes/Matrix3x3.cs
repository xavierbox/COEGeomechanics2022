using System;

namespace CommonData
{
    public class Matrix3x3// : IEnumerable<double>
    {
        private double[,] _vals;

        public Matrix3x3()
        {
        }

        public Matrix3x3(double[,] vals)
        {
            if (!(vals.GetLength(0) == 3 && vals.GetLength(1) == 3))
                throw new ArgumentOutOfRangeException("Input array must be of size 3x3", "vals");
            _vals = vals;
        }

        public static Vector3 operator *(Matrix3x3 m, Vector3 vec)
        {
            return new Vector3(
                m._vals[0, 0] * vec.X + m._vals[0, 1] * vec.Y + m._vals[0, 2] * vec.Z,
                m._vals[1, 0] * vec.X + m._vals[1, 1] * vec.Y + m._vals[1, 2] * vec.Z,
                m._vals[2, 0] * vec.X + m._vals[2, 1] * vec.Y + m._vals[2, 2] * vec.Z);
        }

        public void set(int i, int j, double v)
        {
            _vals[i, j] = v;
        }

        public static Matrix3x3 operator *(Matrix3x3 m, Matrix3x3 other)
        {
            return new Matrix3x3(new double[,]
            {
                {   //Row 0
                    m._vals[0, 0] * other._vals[0, 0] +
                    m._vals[0, 1] * other._vals[1, 0] +
                    m._vals[0, 2] * other._vals[2, 0],

                    m._vals[0, 0] * other._vals[0, 1] +
                    m._vals[0, 1] * other._vals[1, 1] +
                    m._vals[0, 2] * other._vals[2, 1],

                    m._vals[0, 0] * other._vals[0, 2] +
                    m._vals[0, 1] * other._vals[1, 2] +
                    m._vals[0, 2] * other._vals[2, 2]
                },
                {
                    //Row 1
                    m._vals[1, 0] * other._vals[0, 0] +
                    m._vals[1, 1] * other._vals[1, 0] +
                    m._vals[1, 2] * other._vals[2, 0],

                    m._vals[1, 0] * other._vals[0, 1] +
                    m._vals[1, 1] * other._vals[1, 1] +
                    m._vals[1, 2] * other._vals[2, 1],

                    m._vals[1, 0] * other._vals[0, 2] +
                    m._vals[1, 1] * other._vals[1, 2] +
                    m._vals[1, 2] * other._vals[2, 2]
                },
                {
                    //Row 2
                    m._vals[2, 0] * other._vals[0, 0] +
                    m._vals[2, 1] * other._vals[1, 0] +
                    m._vals[2, 2] * other._vals[2, 0],

                    m._vals[2, 0] * other._vals[0, 1] +
                    m._vals[2, 1] * other._vals[1, 1] +
                    m._vals[2, 2] * other._vals[2, 1],

                    m._vals[2, 0] * other._vals[0, 2] +
                    m._vals[2, 1] * other._vals[1, 2] +
                    m._vals[2, 2] * other._vals[2, 2]
                }
            });
        }

        public double this[int i, int j]
        {
            get
            {
                if (i < 0 || j < 0 || i > 2 || j > 2)
                    throw new IndexOutOfRangeException();
                return _vals[i, j];
            }
        }

        public double GetValueAt(int index)
        {
            int i = index / _vals.GetLength(0);
            int j = index % _vals.GetLength(0);

            return _vals[i, j];
        }

        /// <summary>
        /// matrix is stored in column symmetric storage, i.e.
        /// matrix = { m11, m12, m22, m13, m23, m33, m14, m24, m34, m44... }
        public double[] SymetricMatrix
        {
            get
            {
                return new double[]
               {
                    _vals[0, 0], _vals[0, 1], _vals[1, 1], _vals[0, 2], _vals[1, 2], _vals[2, 2]
                };
            }
        }

        public void SetValueAt(int index, double value)
        {
            int i = index / _vals.GetLength(0);
            int j = index % _vals.GetLength(0);

            _vals[i, j] = value;
        }

        public Matrix3x3 Invert()
        {
            double det = _vals[0, 0] * _vals[1, 1] * _vals[2, 2] + _vals[0, 1] * _vals[1, 2] * _vals[2, 0] + _vals[0, 2] * _vals[1, 0] * _vals[2, 1] - _vals[0, 0] * _vals[1, 2] * _vals[2, 1] - _vals[0, 1] * _vals[1, 0] * _vals[2, 2] - _vals[0, 2] * _vals[1, 1] * _vals[2, 0];

            if (det != 0)
            {
                double detInv = 1.0 / det;

                return new Matrix3x3(new double[,]
                {
                    {   //Row 0
                        (_vals[1, 1] * _vals[2, 2] - _vals[1, 2] * _vals[2, 1]) * detInv,
                        (_vals[0, 2] * _vals[2, 1] - _vals[0, 1] * _vals[2, 2]) * detInv,
                        (_vals[0, 1] * _vals[1, 2] - _vals[0, 2] * _vals[1, 1]) * detInv
                    },
                    {
                        //Row 1
                        (_vals[1, 2] * _vals[2, 0] - _vals[1, 0] * _vals[2, 2]) * detInv,
                        (_vals[0, 0] * _vals[2, 2] - _vals[0, 2] * _vals[2, 0]) * detInv,
                        (_vals[0, 2] * _vals[1, 0] - _vals[0, 0] * _vals[1, 2]) * detInv
                    },
                    {
                        //Row 2
                        (_vals[1, 0] * _vals[2, 1] - _vals[1, 1] * _vals[2, 0]) * detInv,
                        (_vals[0, 1] * _vals[2, 0] - _vals[0, 0] * _vals[2, 1]) * detInv,
                        (_vals[0, 0] * _vals[1, 1] - _vals[0, 1] * _vals[1, 0]) * detInv
                    }
                });
            }
            else
            {
                return null;
            }
        }

        public override bool Equals(object obj)
        {
            Matrix3x3 other = obj as Matrix3x3;
            if (other == null) return false;

            if (_vals[0, 0] != other._vals[0, 0]) return false;
            if (_vals[0, 1] != other._vals[0, 1]) return false;
            if (_vals[0, 2] != other._vals[0, 2]) return false;

            if (_vals[1, 0] != other._vals[1, 0]) return false;
            if (_vals[1, 1] != other._vals[1, 1]) return false;
            if (_vals[1, 2] != other._vals[1, 2]) return false;

            if (_vals[2, 0] != other._vals[2, 0]) return false;
            if (_vals[2, 1] != other._vals[2, 1]) return false;
            if (_vals[2, 2] != other._vals[2, 2]) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return (int)(_vals[0, 0] * _vals[1, 1] * _vals[2, 2] + _vals[0, 1] * _vals[1, 2] * _vals[2, 0] + _vals[0, 2] * _vals[1, 0] * _vals[2, 1] - _vals[0, 0] * _vals[1, 2] * _vals[2, 1] - _vals[0, 1] * _vals[1, 0] * _vals[2, 2] - _vals[0, 2] * _vals[1, 1] * _vals[2, 0]);
        }

        private double[] flatArray
        {
            get
            {
                double[] flatArr = new double[_vals.Length];
                int index = 0;
                for (int i = 0; i < _vals.GetLength(0); i++)
                {
                    for (int j = 0; j < _vals.GetLength(1); j++)
                    {
                        flatArray[index] = _vals[i, j];

                        index++;
                    }
                }
                return flatArr;
            }
        }

        /*
        public IEnumerator<double> GetEnumerator()
        {
            return flatArray.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        */
    };
}