using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;

using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.DomainObject.Well;
using Slb.Ocean.Petrel.Seismic;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Slb.Ocean.Petrel.Basics;
using System.Collections;
using System.Threading.Tasks;
using Slb.Ocean.Basics;
using System.Diagnostics;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject;

namespace ManipulateCubes
{

    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class ManipulateCubesWorkstepUI : UserControl
    {
        //public event EventHandler ModelChanged;

        MaterialsModelsControl matsControl;
        PressureModelsControl pressuresControl;
        BoundaryConditionsControl bConditionsControl;
        SimulationsControl simulationsControl;
        Gigamodel gigaModel;

        private ManipulateCubesWorkstep workstep;

        private ManipulateCubesWorkstep.Arguments args;

        private WorkflowContext context;

        public ManipulateCubesWorkstepUI(ManipulateCubesWorkstep workstep, ManipulateCubesWorkstep.Arguments args, WorkflowContext context)
        {
            InitializeComponent();
            gigaModel = OceanUtilities.GetExistingOrCreateGigamodel();


            this.gigaModelTabControl.TabIndexChanged += new System.EventHandler(this.gigaModelTabControl_TabIndexChanged);
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            this.exportbutton.Click += new System.EventHandler(this.exportbutton_Click);


            imageList1.Images.Clear();
            imageList1.Images.AddRange(new Bitmap[] {
                                       PetrelImages.Models_32,
                                       PetrelImages.Property_32,
                                       PetrelImages.Boundary_32,
                                       PetrelImages.Models_32,
                                       PetrelImages.Apply,
                                       PetrelImages.Cancel,
                                       PetrelImages.Help,
                                       PetrelImages.Info });



            this.tabPage1.ImageIndex = 0;


            matsControl = new MaterialsModelsControl();
            tabPage1.Controls.Add(matsControl);
            matsControl.Dock = DockStyle.Fill;
            //this.ModelChanged += new System.EventHandler( matsControl.UpdateControl );

            matsControl.EditSelectionChangedEvent += this.MaterialSelectedChanged;           


            this.tabPage2.ImageIndex = 1;
            pressuresControl = new PressureModelsControl();
            tabPage2.Controls.Add(pressuresControl);
            pressuresControl.Dock = DockStyle.Fill;
           // this.ModelChanged += new System.EventHandler(pressuresControl.UpdateControl);


            this.tabPage3.ImageIndex = 2;
            bConditionsControl = new BoundaryConditionsControl();
            bConditionsControl.Dock = DockStyle.Fill;
           // this.ModelChanged += new System.EventHandler(bConditionsControl.UpdateControl);


            this.tabPage4.ImageIndex = 3;
            simulationsControl = new SimulationsControl();
            tabPage4.Controls.Add(simulationsControl);
            simulationsControl.Dock = DockStyle.Fill;
           // this.ModelChanged += new System.EventHandler(simulationsControl.UpdateControl);


            saveButton.ImageIndex = 4;
            cancelButton.ImageIndex = 5;


        }
        private void MaterialSelectedChanged(object sender, StringEventArgs e)
        {
            matsControl.DisplayModelItem( OceanUtilities.GetExistingOrCreateGigamodel().MaterialModels.GetOrCreateModel( e.Name ));
        }


        #region UI interaction



        #endregion

        #region Logic to produce Args



        #endregion

        private void exportbutton_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int whichTab = gigaModelTabControl.SelectedIndex;
            string msg = null;
            bool isNew = false;
            Gigamodel model = gigaModel;// OceanUtilities.GetExistingOrCreateGigamodel();

            switch (whichTab)
            {
                case 0:
                    {
                        if (matsControl.IsDataSelectedOk(out msg))
                        {
                            isNew = matsControl.IsSelectedAsNew;
                            matsControl.DisplayModelItem(model.StoreModel(matsControl.GetSelection()));                                
                        }
                        break;
                    }
                case 1:
                    {
                        //model.StoreModel(PressureSelection);
                        break;
                    }
                case 2:
                    {
                        break;
                    }
                case 3:
                    {
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

            if ((msg != null)&&(msg!=string.Empty))
                ShowError(msg);
            else 
            ShowResult("Model was successfully " + (isNew ? " created" : " updated"));



        }

        public void ShowError(string msg)
        {
            MessageBox.Show(msg, "Wrong or incomplete input", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void ShowResult(string msg)
        {
            MessageBox.Show(msg, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            //save state 
            this.ParentForm.Close();
        }

        private void gigaModelTabControl_TabIndexChanged(object sender, EventArgs e)
        {

        }
    };

};

