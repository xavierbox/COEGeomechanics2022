using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManipulateCubes
{
    public class StringEventArgs : EventArgs
    {
        public StringEventArgs(string s) : base()
        {
            Name = s; 
        }
        public string Name { get; set; }
    }
}
