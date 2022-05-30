using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class RazorMock {

    public int Value { get; set; } = 4;

    public float[] Values { get; set; } = null;

    public string Text { get; set; } = "I am a text field"; 

};


namespace JsonTestsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RazorMock mock1 = new RazorMock();
            mock1.Value = 22;
            mock1.Values = Enumerable.Range(0, 100).Select(t => (float)(t)).ToArray();

            string output = JsonConvert.SerializeObject(mock1);


            Console.Write(output );
        }

    }
}
