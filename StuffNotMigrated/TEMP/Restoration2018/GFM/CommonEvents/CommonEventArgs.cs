using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restoration.GFM
{
    public class StringArgs : EventArgs
    {
        public StringArgs( string i) : base()  { Value = i; }

        public string Value { get; set; }

    };

    public class ObjectsArgs : EventArgs
    {
        public ObjectsArgs(object i) : base() { Value = i; }

        public object Value { get; set; }


    };


    public class Args<T>:EventArgs
    {
        public Args( T i ) : base() { Value = i; }

        public T Value { get; set; }
    }



}
