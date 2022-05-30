using System;
using System.Drawing;
using System.Windows.Forms;

using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.DomainObject;
using System.Linq;
using System.Collections.Generic;


namespace Restoration
{
    /// <summary>
    /// This class is the user interface which forms the focus for the capabilities offered by the _process.  
    /// This often includes UI to set up arguments and interactively run a batch part expressed as a workstep.
    /// </summary>
    partial class VRestorationUI : UserControl
    {
        /// <summary>
        /// Contains the _process instance.
        /// </summary>
        private VRestorationProcess _process;

        /// <summary>
        /// Initializes a new instance of the <see cref="VRestorationUI"/> class.
        /// </summary>
        public VRestorationUI(VRestorationProcess process)
        {
            InitializeComponent();
            ConnectEvents();
            _process = process;

        }

        private void ConnectEvents()
        {

        }

        private List<string> GetSurfaceNamesRecursive(Collection col)
        {
            List<string> names = col.RegularHeightFieldSurfaces.Select(t => t.Name).ToList();
            foreach (Collection c in col.Collections)
                names.AddRange(GetSurfaceNamesRecursive(c));

            return names;
        }

        private void HorizonsDrop_DragDrop(object sender, DragEventArgs e)
        {
            Collection coll = e.Data.GetData(typeof(Collection)) as Collection;

            var item1 = coll[0];
            Slb.Ocean.Petrel.DomainObject.Shapes.TriangleMesh item2 = item1 as Slb.Ocean.Petrel.DomainObject.Shapes.TriangleMesh;

          
            foreach (var x in coll)
            {
                var x2 = x as Slb.Ocean.Petrel.DomainObject.Shapes.TriangleMesh; 
                var x3 =  x as  Slb.Ocean.Geometry.IndexedTriangleMesh;

                var i3 = x.IsGood;//.ImplementationName;
            }




            if (coll == null)
            {
                Restoration.Services.MessageService.ShowMessage("Please drop a Petrel folder containing the Present day surfaces", Restoration.Services.MessageType.ERROR);
                horizonsPresentationBox.Text = "Please drop a Petrel folder containing the Present day surfaces";
                horizonsPresentationBox.Image = Slb.Ocean.Petrel.UI.PetrelImages.Cancel;///// Error; 
                modelNameTextBox.Text = "";
                return;
            }

            PopulateLithoTable(  GetHorizonNames(coll) ); 

        }

        private void PopulateLithoTable( List<string> names )
        {
            lithoDataGridView.Rows.Clear();

            foreach (string name in names)
            {
                int index = lithoDataGridView.Rows.Add();
                lithoDataGridView.Rows[index].Cells[0].Value = name;
                lithoDataGridView.Rows[index].Cells[1].Value = 5.6;
                lithoDataGridView.Rows[index].Cells[2].Value = 5.6;
                lithoDataGridView.Rows[index].Cells[3].Value = 5.6;
            }
        }

        private List<string> GetHorizonNames( Collection coll )
        {
          
          

            //it is valid if only contains surfaces or if it has a folder containing only surfaces. All with the preffix PresentDay  
            Collection[] subFolders = coll.Collections.Where(t => t.Name.ToLower() == "presentday").ToArray();
            Collection folderTocheck = null;
            if (subFolders.Count() > 0)
            {
                folderTocheck = subFolders.ElementAt(0);
            }
            else
                folderTocheck = coll;


            //take the first one only
            List<string> allNames = GetSurfaceNamesRecursive(folderTocheck);
            char[] sep = new char[] { '_' };
            Dictionary<string, int> names = new Dictionary<string, int>();
            foreach (string s in allNames)
            {
                //split it in "_" 
                var words = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);

                foreach (string partialName in words)
                {
                    if ((partialName.Length<2) || (System.Text.RegularExpressions.Regex.IsMatch(partialName, @"^[0-9]+$")))
                    {
                    }
                    else
                    {
                        if (partialName.ToLower() == "presentday")
                        {
                            ;
                        }
                        else if (!names.Keys.Contains(partialName))
                        {
                            names.Add(partialName, 0);
                        }
                    
                        else
                        {
                            Restoration.Services.MessageService.ShowMessage("The names are not correct. Two or more horizons are interpreted with the name  " + partialName);
                            return new List<string>();
                        }
                    }
                }

          
            }//allnames

            horizonsPresentationBox.Text = coll.Name;
            horizonsPresentationBox.Image = Slb.Ocean.Petrel.UI.PetrelImages.Horizon;///// Error; 
            modelNameTextBox.Text = coll.Name; 


            //if we leave it like this, then the ordering is exactly as they appear in the folder checked. 
            //we will attempt an ordering by counting the items in any subfolder for which we have stored a key. 
            foreach (Collection c in coll.Collections)
                {
                    string [] words = c.Name.Split(sep, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    string firstWord = words.Count() > 0 ? words[0] : "";
                    
                    if (names.Keys.Contains(firstWord)) names[ firstWord ] = c.RegularHeightFieldSurfaceCount;
                }

            List<string> horizonNames = names.Keys.ToList();//.OrderBy(t => t.Value).Select(t=>t.Key).Reverse().ToList();


            return horizonNames; 

            //}
            ;


            //bool valid = (coll != null);

            //if (valid)
            //{ 
            //}
            ////find a collection called "presentday"



            //if (valid)
            //{
            //    //look for the 

            //    List<string> allNames = GetSurfaceNamesRecursive(coll);
            //    char[] sep = new char[] { '_' };
            //    foreach (string s in allNames)
            //    {
            //        //split it in "_" 
            //        var words = s.Split(sep, StringSplitOptions.RemoveEmptyEntries);

            //        if (!names.Contains(word[0]))
            //        names.Add(word);
            //    }
            //}



























            //var surfaces = coll.Surfaces; 

            ////Slb.Ocean.Petrel.DomainObject.Shapes.Tr


            //var q = e.Data.GetData(typeof(object));// as Slb.Ocean.Petrel.DomainObject.Shapes.Surface;

            //// coll.
            //Slb.Ocean.Data.Hosting.IDomainObject item = coll[0];

            //var name = item.ToString();


            //var obj  = e.Data.GetData(typeof(Object));// as Collection;

            //var o2 = obj.ToString();// as Slb.Ocean.Petrel.DomainObject.Shapes.; 

            //if (valid)
            //{
            //    horizonsPresentationBox.Text = coll.Name;
            //    horizonsPresentationBox.Image = Slb.Ocean.Petrel.UI.PetrelImages.Horizon;



            //}
            //if(!valid)
            //{
            //    horizonsPresentationBox.Text = "Please drop a folder from the Input tab, containing the triangular surfaces for the horizons";
            //    horizonsPresentationBox.Image = Slb.Ocean.Petrel.UI.PetrelImages.RowDelete_32;
            //}

        }


        private void PopulateLithoTable()
        {

        }
    }
}
