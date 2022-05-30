using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gigamodel.Data
{
    class Utils
    {
        public static string DateToString(DateTime t)
        {
            return t.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"));
        }

    }
}
