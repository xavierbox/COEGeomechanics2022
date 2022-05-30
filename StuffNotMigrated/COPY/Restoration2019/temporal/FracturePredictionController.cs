using FractureDrivers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractureDrivers.Controllers
{
    class FracturePredictionController
    {
        private FractureDriversProcessUI _ui;

        private FractureModel _model;



        public FracturePredictionController(FractureDriversProcessUI ui, FractureModel  model)
        {
            _ui = ui;
            _model = model;

            ConnectController();
        }

        private void ConnectController()
        {
            //file selected as input 
            _ui.DynelResultsFileSelected += (sender, evt) =>
            {
                bool isValid = DynelParser.ValidFile(evt.Value);
                _ui.AcceptOrRejectSelectedDynelfile(isValid, evt.Value, isValid == false ? "" : "Invalid Dynel results file");
            };


            //the apply button was clicked 
            _ui.ProcessDynelResultsFileClicked += (sender, evt) =>
            {
                if(!DynelParser.ValidFile(evt.Value))
                _ui.AcceptOrRejectSelectedDynelfile(false, evt.Value, "Invalid Dynel results file");

                else
                {
                    Dictionary<string, List<double>> data = DynelResultsFileSelectionHandler( evt );
                    if (data == null)
                        _ui.AcceptOrRejectSelectedDynelfile(false, evt.Value, "The file appears to be a Dynel file but couldnt be parsed");
                    else
                    {
                        //get the data from the UI in a non-ocean format.
                        //observed fractures. the id of the well they were observed, orientation, etc... List<Fracture> ....

                        //update the model
                        _model =  FracturePredictor.PredictFractures( data  );
                        
                        //compute well intersections and return another model for the ui to display as well intersected

                        //return the data to the ui for display 
                        //_ui.Display3DModel( _model );
                        //_ui.DisplayWellFractures( modelIntersected );


                    }

                }


            };
        }

        public static void UpdateFracturesModel(Dictionary<string, List<float>> data, FractureModel model)
        {

            model.Clear();

        }

    
        public void DisConnectController()
        {
        }

        private Dictionary<string, List<double>> DynelResultsFileSelectionHandler(StringEventArgs ev)
        {
            string error; 
            Dictionary<string, List<double>> dataInFile = DynelParser.ParseFile( ev.Value, out error );

            if (dataInFile != null)
                return dataInFile;
            else
                return null; 
            



        }




    };

}
