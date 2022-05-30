using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileToParse = @"D:\Projects\CurrentProjects\ProgrammingProjects\GigamodelVisage\PetrelProjects\REFERENCEPROJECT2017.sim\NOSTRAINRG\NOSTRAINRG.X0000";
            Dictionary<string, KeywordDescription>  map = EclipseReader.GetKeywords( fileToParse );

            SplitCubeFloats data = new SplitCubeFloats(50,50,10);
            foreach (KeywordDescription prop in map.Values)
            {
                EclipseReader.LoadFloatsAsSplitCube(prop, fileToParse, ref data);
           
            }
        }
    }
}
