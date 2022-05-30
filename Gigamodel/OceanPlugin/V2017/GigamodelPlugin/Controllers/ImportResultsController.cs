using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gigamodel.Data;
using System.IO;
using Gigamodel.VisageUtils;
using Gigamodel.OceanUtils;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.DomainObject;
using Gigamodel.Services;

namespace Gigamodel.Controllers
{
    internal class ImportResultsController
    {
        //GigaModelDataModel model;
        GigaModelProcessUI _view;

        public ImportResultsController(GigaModelProcessUI views)//, GigaModelDataModel data)
        {
            //model = data;
            _view = views;
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            _view.ImportRequestEvent += new System.EventHandler<ImportResultsEventArgs>(this.ImportResultsHandler);

            _view.resultsControl.ResultsFolderSelected += new System.EventHandler<StringEventArgs>(this.SimFolderSelectionHandler);

            _view.resultsControl.ListCasesRequestEvent += new System.EventHandler( this.ListCasesHandler);
            //view.ResultsFolderSelected += new System.EventHandler<StringEventArgs>(this.simFolderSelectionHandler);
            //view.resultsControl.ResultsFolderSelected += new System.EventHandler<StringEventArgs>(this.simFolderSelectionHandler);
        }

        private string ReadReferenceDroidFromFile(string file)
        {
            try
            {
                return System.IO.File.ReadAllText(file).Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public void SimFolderSelectionHandler(object sender, StringEventArgs ev)
        {
            DirectoryInfo d = new DirectoryInfo(ev.Value);
            FileInfo[] files = d.GetFiles("*.X*");         //Getting X files only. 


            /*
            string check = "://1d9a2dd1-dd1d-4676-a92e-6057e64d33c2/f0488f4d-b215-4daf-a2d2-8868eb884be2";

            try
            {
                object o = Slb.Ocean.Core.DataManager.Resolve(new Slb.Ocean.Core.Droid(check));

                SeismicCube c = (SeismicCube)o;
                ;

            }
            catch
            {
                ;
            }*/



            _view.ClearResultsControl();
            if ((files == null) || (files.Count() < 1))
            {
        
                MessageService.ShowMessage("No results available for the selected case", MessageType.INFO);
                return;
            }

            var data = EclipseReader.GetKeywords(files[0].FullName);

            //figure out the case name. 
            string caseName = Path.GetFileNameWithoutExtension(files[0].Name);

            //tell the view what to display. 
            _view.resultsControl.DisplayAvailableResultsForCase(
                EclipseReader.GetKeywords(files[0].FullName).Keys.ToArray(),                                        //property names
                Math.Max(0, files.Select(t => Path.GetFileNameWithoutExtension(t.Name) == caseName).Count() ),   //time steps
                caseName,                                                                                           //case name
                ReadReferenceDroidFromFile(d.FullName + "\\" + caseName + ".Droid"));                               //ref droid

        }

        public void ImportResultsHandler(object sender, ImportResultsEventArgs ev)
        {


            try
            {
                DirectoryInfo d = new DirectoryInfo(ev.FolderName);
                FileInfo[] files = d.GetFiles(ev.CaseName + ".X" + ev.TimeStep.ToString("D4"));         //Getting X files only. 

                if (files.Count() <= 0)
                {
                    MessageService.ShowError("The X file requested couldnt be found.");
                    return;

                }

                //load the dates file and if not, then set the number of dates = number of x files. 

                //data in the file; 
                Dictionary<string, KeywordDescription> mapKeywords = EclipseReader.GetKeywords(files[0].FullName);




                //these are the ones selected by the user. 
                List<KeywordDescription> userSelected = mapKeywords.Where(t => ev.PropertyNames.Contains(t.Key)).Select(w => w.Value).ToList();

                //a guess of the templates of the cubes that need to be produced.
                List<Template> resTemplates = TemplateUtils.TemplateForNamesSimilarTo(userSelected.Select(t => t.Name).ToList());

                //the list of cubes produced
                List<SeismicCube> cubesCreated = SeismicCubesUtilities.XFileToSeismicCubes(ev.ReferenceCube, userSelected, files[0].FullName, resTemplates);

                //now we need to sort of organize this and put the cubes in a sub-folder inside the seismic collection
                //of the reference cube. The sub-collection would be called as the caseName with the suffix of the time step. 
                //SeismicCollection col = ev.ReferenceCube.SeismicCollection;
            }
            catch(Exception e )
                {
                var what = e.ToString();
                ;

            }
        }

        public void ListCasesHandler( object sender, EventArgs e )
        {
          
            DirectoryInfo d = new DirectoryInfo(GigaModelDataDefinitions.SimExportFolder);
             _view.resultsControl.ListOfCases = d.GetDirectories().Select(t=>t.Name).ToList();

          

        }

    }
}