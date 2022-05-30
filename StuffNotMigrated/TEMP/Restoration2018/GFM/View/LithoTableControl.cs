using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Slb.Ocean.Petrel.UI;
using Restoration.GFM;
using Restoration.GFM.OceanUtils;

namespace Restoration
{
    public partial class LithoTableControl : UserControl
    {
        LithoData _lithoData = new LithoData();

               public event EventHandler TableChanged;
        public event EventHandler ControlChanged;
       

        public LithoTableControl()
        {
            InitializeComponent();

            lithoTableMoveDown.Image = PetrelImages.DownArrow;
            lithoTableMoveUp.Image = PetrelImages.UpArrow;
            lithoTableDelete.Image = PetrelImages.Cancel;

            DisplayCompact = false;

            this.LithoTable.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.LithoTable_CellEndEdit);
            this.addErosionalStep.Click += new System.EventHandler(this.lithoTableMoveUp_Click);
            this.lithoTableDelete.Click += new System.EventHandler(this.lithoTableMoveUp_Click);
            this.lithoTableMoveDown.Click += new System.EventHandler(this.lithoTableMoveUp_Click);
            this.lithoTableMoveUp.Click += new System.EventHandler(this.lithoTableMoveUp_Click);

           

        }

        public string ModelName
        {
            get; set;
        }

        public string BaseName
        {
            set
            {
                basementBox.Text = value;
                basementBox.Image = basementBox.Text != string.Empty ? PetrelImages.Horizon : null;
            }

            get
            {
                return basementBox.Text;// return Project.BaseName;
            }

        }

        public bool DisplayCompact
        {
            get
            {
                return _compact;
            }
            set
            {
                _compact = value;
                addErosionalStep.Visible = !(_compact);
                lithoTableDelete.Visible = !(_compact);
                lithoTableMoveDown.Visible = !(_compact);
                lithoTableMoveUp.Visible = !(_compact);

        
            }
        }

        private bool _compact;

        public void ClearTable()
        {
            LithoTable.Rows.Clear();
            LithoTable.Columns.Clear();
        }

        public LithoData LithoData
        {
            get
            {

                return _lithoData;
            }
            set
            {
                _lithoData = value;
                PopulateTable();
            }
        }

 


        private void PopulateTable()//LithoData data)
        {

            ClearTable();

            foreach (string name in LayerLithoData.VisiblePropertyNames)
                LithoTable.Columns.Add(name, name);

            foreach (LayerLithoData row in LithoData.Layers)
            {
                string[] values = row.VisiblePropertyValues;
                LithoTable.Rows.Add(values);

            }

            for (int n = 0; n < LithoTable.RowCount; n++)
            {
                LithoTable.Rows[n].Cells[0].Style.BackColor = ProjectTools.GetRandomColor(n); ;

            }
        }


        private void LithoTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int column = e.ColumnIndex;
            ;
            ;
            string[] visiblePropertyNames = LayerLithoData.VisiblePropertyNames;
            string propertyChanged = visiblePropertyNames[column];
            string newValue = (string)(LithoTable.Rows[row].Cells[column].Value);

            LayerLithoData l = LithoData.Layers[row];
            l.SetValue(propertyChanged, newValue);


            List<KeyValuePair<double, double>> dvt = l.GetDVTTable();
            string droid = GFMOceanUtils.PublishFunction(ModelName, l.Layer + "_YM_dvt", dvt);
            l.YoungsModulusHardeningFunctionDroid = droid;

            string[] values = LithoData.Layers[row].VisiblePropertyValues;
            LithoTable.Rows[row].SetValues(values);


            TableChanged?.Invoke(this, EventArgs.Empty);

          


        }


        private void lithoTableMoveUp_Click(object sender, EventArgs e)
        {
            Button b = (Button)(sender);

            if (b == lithoTableMoveUp)
            {
                MessageBox.Show("Not implemeted yet....Wait!!");
            }
            else if (b == lithoTableMoveDown)
            {
                MessageBox.Show("Not implemeted yet....Wait!!");
            }
            else if (b == lithoTableDelete)
            {

                var rows = LithoTable.SelectedRows;
                if (rows.Count >= 1)
                {
                    int row = rows[0].Index;
                    LithoData.Layers.RemoveAt(row);
                    LithoTable.Rows.RemoveAt(row);
                }


            }
            else if (b == addErosionalStep)
            {
                MessageBox.Show("Not implemeted yet....Wait!!");

                //AddGeologicalStepDialog dlg = new AddGeologicalStepDialog();
                //DialogResult r = dlg.ShowDialog();
                //string entered = dlg.NameEntered;

                //if (entered != string.Empty)
                //{
                //    LayerLithoData l = new LayerLithoData();
                //    l.Layer = dlg.NameEntered;
                //    LithoData.Layers.Add(l);
                //    GenerateDVTFunction(l);
                //    PopulateTable();
                //}
            }
            else {; }

            ControlChanged?.Invoke(this, EventArgs.Empty);
        }

        private void LithoTableControl_Load(object sender, EventArgs e)
        {

        }




        //private void loadLithoTableButton_Click(object sender, EventArgs e)
        //{
        //    //FromVisageLithoFile
        //    ControlChanged?.Invoke(this, EventArgs.Empty);
        //    TableChanged?.Invoke(this, EventArgs.Empty);
        //}


    }
}
