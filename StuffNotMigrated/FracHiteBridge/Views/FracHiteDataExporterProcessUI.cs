using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.DomainObject.Shapes;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.DomainObject.Well;



using PropertyCollection = Slb.Ocean.Petrel.DomainObject.PillarGrid.PropertyCollection;



using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Slb.Ocean.Basics;
using FrachiteData;

using System.Windows.Forms;

namespace FracHiteBridge
{
  /// <summary>
  /// This class is the user interface which forms the focus for the capabilities offered by the process.  
  /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
  /// </summary>
  partial class FracHiteDataExporterProcessUI : UserControl
  {
    /// <summary>
    /// Contains the process instance.
    /// </summary>
    private FracHiteDataExporterProcess _process;

    Dictionary<string, Property> _propsToExport = new Dictionary<string, Property>();

    /// <summary>
    /// Initializes a new instance of the <see cref="FracHiteDataExporterProcessUI"/> class.
    /// </summary>
    public FracHiteDataExporterProcessUI(FracHiteDataExporterProcess process)
    {
      InitializeComponent();

      DateTime today = DateTime.Now;
      DateTime date2 = new DateTime(2022, 8, 1);
      int result = DateTime.Compare(today, date2);

            if (result < 0)
            {
                ConfigureUIComponents();

                ConnectEvents();

                _process = process;

            }

            }

    private void ConfigureUIComponents()
    {
      _propsDropPresentationBox.AcceptedTypes = new List<Type>() { typeof(PropertyCollection) };
      _propsDropPresentationBox.PlaceHolder = "Drag-drop a property folder from a simple grid";
      _propsDropPresentationBox.ErrorImage = PetrelImages.Cancel;
      ImageList images = new ImageList();
      images.Images.Add(PetrelImages.Property);
      _propsDropPresentationBox.ImageList = images;

      _cancelButton.Image = PetrelImages.Cancel;
      _acceptButton.Image = PetrelImages.Apply;
      _folderButton.Image = PetrelImages.Folder;
    }

    private void ConnectEvents()
    {

      _cancelButton.Click += (sender, evt) => { this.ParentForm.Close(); };
      _acceptButton.Click += (sender, evt) => { ExportData(); };
      _folderButton.Click += (sender, evt) =>
      {
        using (FolderBrowserDialog folderDlg = new FolderBrowserDialog())
        {
          if (folderDlg.ShowDialog() == DialogResult.OK)
            folderPresentationBox.Text = folderDlg.SelectedPath; ;
        }
      };

       _propsDropPresentationBox.ValueChanged += (ender, evt) => 
       {
         _propsToExport.Clear();
         if (_propsDropPresentationBox.Value is PropertyCollection)
         {
           PropertyCollection c = (PropertyCollection)(_propsDropPresentationBox.Value);

           //lets try to find the properties with the required names 
           string[] namesRequired = FrachiteData.FrachiteConfig.RequiredPropertyNamesFromPetrel;
           List<string> namesInFolder = c.Properties.Select(t => t.Name).ToList(), missingProperties = new List<string>();
           IEnumerable<Property> props = c.Properties;

           foreach (string name in namesRequired)
           {
             int index = namesInFolder.IndexOf(name);
             if (index >= 0)
             {
               _propsToExport.Add(name, props.ElementAt(index));
             }
             else
             {
               _propsToExport.Add(name, null);
               missingProperties.Add(name);
             }
           }

           if (missingProperties.Count > 0)
           {
             DisplayErrorInMissingPropertiesDialog(missingProperties);
                   if( _propsDropPresentationBox.Value != null )
                   
             _propsDropPresentationBox.Value = null;
           }

         }
         else
         {
           DisplayErrorInInputDataDialog();
               if (_propsDropPresentationBox.Value != null)
                   _propsDropPresentationBox.Value = null;
         }
       };

    }



 
    private void DisplayErrorInMissingPropertiesDialog(List<string> missingProperties)
    {
      System.Text.StringBuilder msg = new System.Text.StringBuilder("The following properties are missing:\n");
      foreach (string s in missingProperties) msg.Append(s + "\n");
      MessageBox.Show(msg.ToString(), "Missing properties",MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    

    private void DisplayErrorInInputDataDialog()
    {
      MessageBox.Show("Please drop a valid property folder with the needed properties","Input error",MessageBoxButtons.OK,MessageBoxIcon.Error);
    }

    private bool CheckFolder(string name)
    {
      return true;
    }

    private float[] GetPropertyValuesKJIFastes(Property p)
    {
      float[] values = null;
      if (p != Property.NullObject)
      {
        var grid = p.Grid;
        values = new float[grid.NumCellsIJK.K * grid.NumCellsIJK.J * grid.NumCellsIJK.I];
        int counter = 0;
        using (var indexer = p.SpecializedAccess.OpenFastPropertyIndexer())
        {
          for (var k = 0; k < grid.NumCellsIJK.K; k++)
          {
            for (var j = 0; j < grid.NumCellsIJK.J; j++)
              for (var i = 0; i < grid.NumCellsIJK.I; i++)
                values[counter++] = indexer[i, j, k];
          }
        }

      }
      return values;
    }

        private void ExportData()
        {
            string folder = folderPresentationBox.Text;
            if (CheckFolder(folder))
            {

                string fracHiteInternalOptionsFile = Path.Combine(folder, "Options.bin");
                using (TextWriter writer = new StreamWriter(fracHiteInternalOptionsFile))
                {
                    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(FracHiteOptions));
                    ser.Serialize(writer, new FracHiteOptions());
                    writer.Close();
                }

                string binaryDataFile = Path.Combine(folder, "Arrays.bin");
                using (BinaryWriter writer = new BinaryWriter(File.Open(binaryDataFile, FileMode.Create)))
                {
                    Grid grid = _propsToExport[_propsToExport.Keys.ElementAt(0)].Grid;
                    writer.Write((float)grid.NumCellsIJK.I);
                    writer.Write((float)grid.NumCellsIJK.J);
                    writer.Write((float)grid.NumCellsIJK.K);

                    float numberArrays = (_propsToExport.Keys.Count()) * 1.0f;
                    writer.Write(numberArrays);

                    foreach (string name in _propsToExport.Keys)
                        writer.Write(name);

                    //then all the arrays
                    foreach (string s in _propsToExport.Keys)
                    {
                        Property p = _propsToExport[s];
                        float[] array = GetPropertyValuesKJIFastes(p);
                        byte[] bytesOriginal = new byte[array.Length * sizeof(float)];
                        System.Buffer.BlockCopy(array, 0, bytesOriginal, 0, bytesOriginal.Length);
                        writer.Write(bytesOriginal, 0, bytesOriginal.Length);
                    }

                    writer.Close();
                }//arrays 


                string droid = _propsToExport[_propsToExport.Keys.ElementAt(0)].Grid.Droid.ToString();
                File.WriteAllText(Path.Combine(folder, "GridDroid.txt"), droid);

                //var grid = (Grid)(DataManager.Resolve(new Droid(droid)));



                ;


            }//serializer
            MessageBox.Show("Export done", "Operation finished");
            var r = MessageBox.Show("Do you want to launch the simulartor now?", "Launch simulator", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {

                MessageBox.Show("Jajaja...it would be cool wouldnt it?....but it is not implemented yet. You need to do it manually");

            }


        }
    }
}
