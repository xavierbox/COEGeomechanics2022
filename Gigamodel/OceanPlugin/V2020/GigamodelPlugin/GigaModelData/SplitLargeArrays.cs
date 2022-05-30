using System;
using System.IO;
using System.Linq;

namespace Gigamodel.GigaModelData
{
    public class SplitSeismicCubeBytes
    {
        public SplitSeismicCubeBytes( int parts = 4 )
        {
            _parts = new byte[parts][];
        }

        public int NumberChunks { get { return _parts.Count(); } }

        //indexer
        public byte[] this[int idx1]
        {
            get { return _parts[idx1]; }
            set { _parts[idx1] = value; }
        }

        public byte this[int idx1, int idx2]
        {
            get { return _parts[idx1][idx2]; }
            set { _parts[idx1][idx2] = value; }
        }

        private byte[][] _parts;
    }

    public class SplitCube<T>  //where T : T? according to MS, everything except Nullable<T>. Great!
    {
        protected T[][] _parts;
        private int _trLength;

        protected SplitCube() //only derived classes can construct it.
        {
            _parts = null;   //!!!!!!! somehow this works pretty well...
        }                    //maybe because the type is knwon at compile time via specialization

        //be careful with this. Dont remove ever the protected qualifier

        //indexers
        public T[] this[int idx1]
        {
            get { return idx1 >= _parts.Count() ? null : _parts[idx1]; }
            set { _parts[idx1] = value; }
        }

        public T this[int idx1, int idx2]
        {
            get { return _parts[idx1][idx2]; }
            set { _parts[idx1][idx2] = value; }
        }

        public int TraceLength
        {
            get { return _trLength; }
            protected set
            {
                _trLength = value;
            }
        }

        #region sizequeries

        public int NumberChunks
        {
            get
            {
                return _parts != null ? _parts.Count() : 0;
            }
        }

        public int SizeOfChunk( int n )
        {
            return _parts != null ? _parts[n].Count() : 0;
        }

        public long TotalSize
        {
            get
            {
                long totSize = 0;
                for (int n = 0; n < NumberChunks; n++) totSize += SizeOfChunk(n);
                return totSize;
            }
        }

        #endregion sizequeries
    };

    [Serializable()]
    public class SplitCubeFloats : SplitCube<float>
    {
        //this is better constructor and the prefferred
        public SplitCubeFloats( int nij, int nk, int maxFloatsPerChunk = 200000000 ) : base()
        {
            Split(nij, nk, maxFloatsPerChunk);
        }

        public SplitCubeFloats() : base()
        {
        }

        public string Ordering { get; set; }

        public float[] GetPart( int n )
        {
            return _parts[n];
        }

        public void Split( int nij, int nk, int maxFloatsPerChunk = 200000000 )
        {
            //first guess a reasonable split for floats
            int tracesPerChunk = Math.Min(nij, (int)(0.75 * maxFloatsPerChunk / nk));
            tracesPerChunk = Math.Max(1, tracesPerChunk); //this is needed for maxFloatsPerChunk very small

            int nChunks = nij / (tracesPerChunk);
            _parts = new float[nChunks][];
            TraceLength = nk;

            //adjustment if possible/needed.  Elemnt no. in parts will be multiple of trace-length nk
            //and all the parts will be pretty close in size if not identical.
            int lastChunkTraces = nij - (nChunks - 1) * tracesPerChunk;
            tracesPerChunk += lastChunkTraces - tracesPerChunk <= 0 ? 0 : ((lastChunkTraces - tracesPerChunk) / (nChunks));

            while (lastChunkTraces - tracesPerChunk > 0)
            {
                tracesPerChunk += (lastChunkTraces - tracesPerChunk) / (nChunks);
                lastChunkTraces = nij - (nChunks - 1) * tracesPerChunk;
                ;
            }

            //allocate all the memory of the chunks at once
            for (int n = 0; n < nChunks - 1; n++)
            {
                _parts[n] = new float[tracesPerChunk * nk];
            }
            _parts[nChunks - 1] = new float[(nij - tracesPerChunk * (nChunks - 1)) * nk];
            ;
            //this takes about 70 milliseconds for 4GB (about 1 billion floats)
            //tested in MANY different combinarions of the input parameters
        }

        public void SetTrace( int n, float[] trace )
        {
            int elementStart;// = n * TraceLength;
            int chunk;// = elementStart / SizeOfChunk(0); //all execpt the last chunk are always same size

            int eleStartInChunk;// = elementStart - (chunk) * TraceLength;//inclusive

            //in which chunk is the trace
            elementStart = n * TraceLength;
            chunk = elementStart / SizeOfChunk(0); //all execpt the last chunk are always same size

            //this is buggy, it was a bug
            //eleStartInChunk = elementStart - (chunk) * TraceLength;//inclusive
                                                                   //copy the bytes in the target array.
                                                                   //Buffer.BlockCopy(trace, 0, _parts[chunk], sizeof(float) * eleStartInChunk, TraceLength * sizeof(float));
                                                                   //test this. This seems to be correct.
            eleStartInChunk = elementStart - (chunk) * SizeOfChunk(0);
            Buffer.BlockCopy(trace, 0, _parts[chunk], sizeof(float) * eleStartInChunk, TraceLength * sizeof(float));
        }

        public void GetTrace( int n, ref float[] trace )
        {
            //in which chunk is the trace
            int elementStart = n * TraceLength;
            int chunk = elementStart / SizeOfChunk(0); //all execpt the last chunk are always same size

            int eleStartInChunk = elementStart - (chunk) * SizeOfChunk(0);//inclusive

            //copy the bytes in the target array.
            if (chunk >= _parts.Count())
            {
                ;
            }
            else
                Buffer.BlockCopy(_parts[chunk], sizeof(float) * eleStartInChunk, trace, 0, TraceLength * sizeof(float));

            //https://docs.microsoft.com/en-us/dotnet/api/system.buffer.blockcopy?view=netframework-4.7.2
            //BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);
        }

        public float[] GetTrace( int n )
        {
            //in which chunk is the trace
            int elementStart = n * TraceLength;
            int chunk = elementStart / SizeOfChunk(0); //all execpt the last chunk are always same size

            int eleStartInChunk = elementStart - (chunk) * SizeOfChunk(0); //inclusive
            float[] trace = new float[TraceLength];

            //copy the bytes in the target array.
            Buffer.BlockCopy(_parts[chunk], sizeof(float) * eleStartInChunk, trace, 0, TraceLength * sizeof(float));

            return trace;

            //https://docs.microsoft.com/en-us/dotnet/api/system.buffer.blockcopy?view=netframework-4.7.2
            //BlockCopy(Array src, int srcOffset, Array dst, int dstOffset, int count);
        }

        public static bool Serialize( SplitCubeFloats splitcube, string fileName, bool enforceBigEndian )// = true )
        {
            bool swappBytes = false;
            if ((enforceBigEndian) && (BitConverter.IsLittleEndian)) //i want output as big endian and i am in a windows machine that is little endian
            {
                swappBytes = true;
            }
            /*swappBytes = true; */

            using (BinaryWriter writter = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                byte[] bytes = new byte[sizeof(float) * splitcube[0].Count()];
                for (int n = 0; n < splitcube.NumberChunks; n++)
                {
                    float[] chunk = splitcube[n];
                    int byteSize = chunk.Count() * sizeof(float);
                    if (bytes.Length != byteSize)
                        bytes = new byte[byteSize];
                    Buffer.BlockCopy(chunk, 0, bytes, 0, byteSize);

                    //now swapp the bytes
                    if (swappBytes)
                        ChangeEndianess(ref bytes, sizeof(float));

                    writter.Write(bytes);
                }
                writter.Close();
            }

            return true;
        }

        private static void ChangeEndianess( ref byte[] bytes, int bytesize = sizeof(float) )
        {
            int nwords = bytes.Count() / bytesize;
            for (int n = 0; n < nwords; n++)
            {
                int firstByte = n * bytesize;
                for (int k = 0; k < bytesize / 2; k++)
                {
                    byte tmp = bytes[firstByte + k];
                    bytes[firstByte + k] = bytes[firstByte + bytesize - k - 1];
                    bytes[firstByte + bytesize - k - 1] = tmp;
                }
                ;
            }
        }
    }
}