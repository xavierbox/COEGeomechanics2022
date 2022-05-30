using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gigamodel.GigaModelData;
using Gigamodel.Data;

namespace Gigamodel.VisageUtils
{
    public enum DataFormatTypesEnum { MESS = 0, NONE = 0, CHAR, INT, REAL, LOGI, DOUBLE, OTHER };

    public class KeywordDescription
    {
        public void SetTypeAndWordSize(string s)
        {
            //INTE standard (4 byte) signed integers
            //REAL single precision(4 byte) floating point reals
            //LOGI standard(4 byte) logicals
            //DOUB double precision (8 byte) floating point reals
            //CHAR characters(handled as 8 - character words

            if (s.ToUpper().Contains("INT")) { Type = DataFormatTypesEnum.INT; WordSize = 4; }
            else if (s.ToUpper().Contains("REAL")) { Type = DataFormatTypesEnum.REAL; WordSize = 4; }
            else if (s.ToUpper().Contains("DOUB")) { Type = DataFormatTypesEnum.DOUBLE; WordSize = 8; }
            else if (s.ToUpper().Contains("MESS")) { Type = DataFormatTypesEnum.MESS; WordSize = 0; }
            else if (s.ToUpper().Contains("CHAR")) { Type = DataFormatTypesEnum.CHAR; WordSize = 8; }
            else if (s.ToUpper().Contains("LOGI")) { Type = DataFormatTypesEnum.LOGI; WordSize = 4; }

            else { Type = DataFormatTypesEnum.OTHER; WordSize = 0; }
        }

        public KeywordDescription(string name, int size, string typeAsString, long loc)
        {
            Name = name;
            Size = size;
            SetTypeAndWordSize(typeAsString);
            ByteLocation = loc;
        }

        public string Name { get; set; }

        public int Size { get; set; }

        public DataFormatTypesEnum Type { get; set; }

        public long ByteLocation { get; set; }

        public int WordSize { get; set; }

    };

    class EclipseReader
    {
        private static KeywordDescription GetDataKeywordNameSizeType(BinaryReader reader)
        {
            reader.ReadBytes(4);

            //var dataArray = new byte[8];
            byte[] dataArray = reader.ReadBytes(8);
            string keyword = Encoding.UTF8.GetString(dataArray, 0, 8);

            dataArray = reader.ReadBytes(4);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(dataArray);
            }
            int size = BitConverter.ToInt32(dataArray, 0);

            dataArray = reader.ReadBytes(4);
            string type = Encoding.UTF8.GetString(dataArray, 0, 4);

            reader.ReadBytes(4);

            return new KeywordDescription(keyword, size, type, reader.BaseStream.Position);
        }

        public static bool LoadFloatsAsSplitCube(KeywordDescription prop, string fileToParse, ref SplitCubeFloats data)
        {
            BinaryReader reader = null;
            try
            {
                reader = new BinaryReader(File.Open(fileToParse, FileMode.Open), Encoding.UTF8);
            }
            catch
            {
                return false;
            }
            Console.WriteLine("Parsing keyword " + prop.Name );
            float maxVal = -1.0e8f, minVal = 1.0e8f, avgValue = 0.0f;
            int countedValues = 0; 
             
            ////////////////////////
            // WORKING BUT WORD BY WORD. This is the only way thanks to the silly breaks everywhere (spaces, commas, etc). 
            int partNumber = 0, nprime = 0; ;
            float[] floatsArray = data[partNumber];
            int dummy = 0;
            byte[] dataArray = new byte[4];

            reader.BaseStream.Seek(prop.ByteLocation, SeekOrigin.Begin);
            for (int n = 0; n < prop.Size; n++)
            {
                dataArray = reader.ReadBytes(4);
                if (BitConverter.IsLittleEndian)
                    Array.Reverse(dataArray);


                var value = BitConverter.ToSingle(dataArray, 0);

                maxVal = Math.Max(maxVal, value);
                minVal = Math.Min(minVal, value);
                countedValues += 1;
                avgValue += value;


                floatsArray[nprime] = BitConverter.ToSingle(dataArray, 0);
                System.Console.WriteLine("filling nprime " + nprime + " in split part "  +partNumber  ); 
                if (n + 1 >= floatsArray.Count())
                {
                    floatsArray = data[(++partNumber)];
                    nprime = 0; 
                }
                if (++dummy == 1000)
                {
                    reader.ReadBytes(8); dummy = 0;
                }

                nprime++;
            }

            avgValue /= (countedValues * 1.0f);
            Console.WriteLine("    Max: " + maxVal + "   Min:  " + minVal + " Average:  " + avgValue);

            reader.Close();

            return true;

        }

        public static Dictionary<string, KeywordDescription> GetKeywords(string fileToParse)
        {
            BinaryReader reader = new BinaryReader(File.Open(fileToParse, FileMode.Open), Encoding.UTF8);
            bool keepReading = reader != null ? true : false;
            Dictionary<string, KeywordDescription> map = new Dictionary<string, KeywordDescription>();

            while (keepReading)
            {
                KeywordDescription key = GetDataKeywordNameSizeType(reader);

                if (key.Size > 0)
                {
                    reader.ReadBytes(4);
                  

                    //////////////////////////////
                    //if (key.Name.Contains("ROCKDISY"))
                    //{
                    //    reader.ReadBytes(8);
                    //    byte[] dataArray = reader.ReadBytes(4);
                    //    if (BitConverter.IsLittleEndian)
                    //    Array.Reverse(dataArray);
                    //    var value = BitConverter.ToSingle(dataArray, 0);
                    //}
                    //    ////////////////////////////////


                    //4 is a space and (key.Size-1)/1000 is the number of sentinel lines. 8 bytes each  
                    int toSkip = 4 + 8 * ((int)((key.Size - 1) / (1000))) + key.WordSize * key.Size;

                    long currentLocation = reader.BaseStream.Position;                   //here the datum starts. 
                    key.ByteLocation = currentLocation;                                  //store it for later use
                    reader.BaseStream.Seek(currentLocation + toSkip, SeekOrigin.Begin);  //jump to the next 

                    if ((key.Name.ToUpper() == "INTEHEAD") || (key.Name.ToUpper() == "DOUBHEAD"))
                    {
                    }
                    else
                    {
                        map.Add(key.Name, key);
                    }

                }
                if ((key.Name == "ENDSOL") || (reader.BaseStream.Position >= reader.BaseStream.Length - 8))
                {
                    keepReading = false;
                }

            }//while


            reader.Close();
            return map;
        }
    };



}
