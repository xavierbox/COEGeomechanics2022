using System;

namespace Gigamodel.Data
{
    internal class Utils
    {
        public static string DateToString( DateTime t )
        {
            return t.ToString("G", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES"));
        }
    }
}