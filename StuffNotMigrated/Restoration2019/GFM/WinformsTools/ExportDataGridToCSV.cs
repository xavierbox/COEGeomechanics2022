using System.Windows.Forms;

namespace Restoration.GFM.WinformsTools
{
    public class WinformsTools
    {
        private static void ExportDataGridToCSV( DataGridView table, string fileName )
        {
            //using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileName))
            //{
            //    file.WriteLine(string.Join(",", simd1DResultsChart.Series.Select(t => t.Name)));

            //    string line = "";
            //    for (int nc = 0; nc < table.ColumnCount; nc ++)
            //    {
            //        line += ( table.Columns[nc].HeaderText + ( nc == table.ColumnCount -1 ? "":",") );
            //    }
            //    file.WriteLine(line);

            //    for (int nr = 0; nr < fakeDataGridView.Rows.Count; nr++)
            //    {
            //        var r = fakeDataGridView.Rows[nr];
            //        line = "";

            //        for (int c = 0; c < fakeDataGridView.ColumnCount; c++)
            //            line += (r.Cells[c] + (c == fakeDataGridView.ColumnCount - 1 ? "" : ","));
            //        file.WriteLine(line);

            //    }
            //}
        }
    }
}