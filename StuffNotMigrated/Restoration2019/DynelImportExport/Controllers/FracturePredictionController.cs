
using Restoration;
using CommonData;
using Restoration.Model;
using Restoration.OceanUtils;
using Slb.Ocean.Petrel.DomainObject.PillarGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//_ui.ClearEvent += (sender, evt) =>
//{
//    _model = null;
//};


//_ui.MapFracturesToGridClicked += (sender, evt) =>
//{

//    if (_model != null)
//    {
//        _ui.MapFractureDataInGrid(_model);
//    }
//};


namespace Restoration.Controllers
{


    class ImportPointStressController
    {

        public ImportPointStressController(ImportPointStressUI ui)
        {
            ui.DynelResultsFileSelected += (sender, evt) =>
            {
                bool isValid = DynelParser.ValidFile(evt.Value);
                ui.AcceptOrRejectSelectedDynelfile(isValid, evt.Value, isValid == false ? "" : "Invalid Dynel results file");
            };

 
            ui.ImportStressesFromDynelToGridClicked += (sender, evt) =>
            {

                if (!DynelParser.ValidFile(evt.DataFileName))
                {
                    ui.AcceptOrRejectSelectedDynelfile(false, evt.DataFileName, "Invalid Dynel results file");
                    return;
                }

                Dictionary<string, List<double>> data = DynelParser.ParseFile(evt.DataFileName, evt.Traslation);

                if (data == null)
                {
                    ui.AcceptOrRejectSelectedDynelfile(false, evt.DataFileName, "The file appears to be a Dynel file but couldnt be parsed");
                    return;
                }

                //get the data from the UI in a non-ocean format.
                try
                {
                    ui.DisplayStress(data);
                }
                catch { 
                }

            };
        }



    };


    class FracturePredictionController
    {
        private DRestorationProcessUI _ui;
        private FractureModel _model;


        public FracturePredictionController(DRestorationProcessUI ui, FractureModel model)
        {
            _ui = ui;
            _model = model;

            ConnectController();
        }

        private void ConnectController()
        {

            ////file selected as input 
            //_ui.DynelResultsFileSelected += (sender, evt) =>
            //{
            //    bool isValid = DynelParser.ValidFile(evt.Value);
            //    _ui.AcceptOrRejectSelectedDynelfile(isValid, evt.Value, isValid == false ? "" : "Invalid Dynel results file");
            //};

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //_ui.ImportStressesFromDynelToGridClicked += (sender, evt) =>
            //{

            //    if (!DynelParser.ValidFile(evt.DataFileName))
            //    {
            //        _ui.AcceptOrRejectSelectedDynelfile(false, evt.DataFileName, "Invalid Dynel results file");
            //        return;
            //    }

            //    Dictionary<string, List<double>> data = DynelParser.ParseFile(evt.DataFileName, evt.Traslation);

            //    if (data == null)
            //    {
            //        _ui.AcceptOrRejectSelectedDynelfile(false, evt.DataFileName, "The file appears to be a Dynel file but couldnt be parsed");
            //        return;
            //    }

            //    //get the data from the UI in a non-ocean format.
            //    _ui.DisplayStress(data);
            //};


            _ui.FracturePredictionFromStressClicked += (sender, evt) =>
            {
                FractureModel model = FracturePredictor.PredictFracturesFromStress(evt.StressTensor, evt.FrictionAngle, evt.Locations, evt.ApplyFiltering);
                _ui.DisplayFractureModel(model);
            };


            _ui.CompareModelAndWellDataClicked += (sender, evt) =>
            {

                


                //both are almost the same type of object. 
                WellFractureDataModel observedModel = evt.ObservedData;
                FractureModel predictedModel = evt.PredictedModel;
                Console.WriteLine("Observed model (well data) ");
                var wellFracs = observedModel.WellFractures;
                foreach (var key in wellFracs.Keys)
                Console.WriteLine("well " + key + " count fracs " + wellFracs[key].Count);


                CompareFractureOrientationInModels(observedModel, predictedModel);


                //remove from the list of observed fractures those that are not in a cell with stress defined or in the k-layer range selected 
                //foreach (string wellName in observedModel.WellFractures.Keys)
                //{
                //    List<Fracture> filtered = observedModel.WellFractures[wellName].Where(t => t.Extension != null).ToList();
                //    observedModel.WellFractures[wellName] = filtered;
                //}


                //for each well, Get all the non-repeated Locations. For each location, figure out the closest dynel prediction (s)
                //store everything as an WellFractureDataModel and return this to the ui so it can figure out what to display. 
                _ui.DisplayWellComparison(observedModel);


            };



        }

        public void CompareFractureOrientationInModels(WellFractureDataModel observedModel, FractureModel predictedModel)
        {
            /*
             * algorithm:
             * First create a cell-map for all the items in the predicted model. This is the largest dataset.
             * then for each item in the observed model, find all the neighbours using the cellmap. From these, select the closest per type
             * Then do the cost function for each.
             * 
             * where do we store it? Better if in an object, that the UI can easily understand. 
             
             */
            Fracture[] predictedFractures = predictedModel.Fractures.ToArray();
            Vector3[] locations = predictedModel.Fractures.Select(t => t.Location).ToArray();// FracturesAtLocations.Keys.ToArray();
            CellMapper cellMap = new CellMapper(locations, 1000);
            ////HERE, the 1000 needs to go away. Cannot be hard coded. 


            Dictionary<string, List<Fracture>> WellFractures = observedModel.WellFractures;
            foreach (string wellName in WellFractures.Keys)
            {
                
                //these are the observed at the well
                List<Fracture> wellFracs = WellFractures[wellName];
                for (int n = 0; n < wellFracs.Count; n++)
                {
                    Fracture observedFracture = wellFracs[n];
                    Vector3 observedFracLocation = observedFracture.Location;
                    Vector3 observedFracNormal = observedFracture.Orientation.Normal;

                    List<KeyValuePair<float, int>> sortedIndexedDistancesModelToObserved = cellMap.FindOrderedNeighbourDistances(observedFracLocation);
                    //<distance 1 , item number at that distance> <. distance 2, item number at that distance..>, items are frac locations that can be converted to fracs (next line) 

                    //in this list, we have the model fracs, sorted by distance, that are neighbours to the observed fracture. 
                    List<Fracture> sortedNeighbourFractures = sortedIndexedDistancesModelToObserved.Select(t => predictedFractures[t.Value]).ToList();



                    //version 1, more general, also slower. A Lot of data 
                    List<FractureType> neighbourTypesFound = new List<FractureType>();
                    if (sortedNeighbourFractures.Count > 0)
                    {
                        FractureCostFunction observedFracCostFunction = new FractureCostFunction();
                        observedFracCostFunction.ReferenceType = (int)observedFracture.Type;

                        foreach (Fracture frac in sortedNeighbourFractures)
                        {
                            if (neighbourTypesFound.Contains(frac.Type)) //ignore repeated types. We already process the closest before since the list is sorted by distance 
                            {
                                ;
                            }
                            else
                            {
                                neighbourTypesFound.Add(frac.Type);
                                Vector3 modelNnormal = frac.Orientation.Normal;
                                float costFunctionAux = (float)(modelNnormal * observedFracNormal);

                                float costFunction = 1.0f - (costFunctionAux * costFunctionAux);


                                observedFracCostFunction.AddOrReplaceCostFunction((int)frac.Type, costFunction);

                                if ((frac.Type == FractureType.Shear1) || (frac.Type == FractureType.Shear2))
                                {
                                    float shearCost = observedFracCostFunction.GetCostForType((int)FractureType.Shear);
                                    if ((float.IsNaN(shearCost)) || (costFunction < shearCost))
                                        observedFracCostFunction.AddOrReplaceCostFunction((int)FractureType.Shear, costFunction);
                                }
                            }


                            //Vector3 modelNnormal = frac.Orientation.Normal;
                            //float costFunction = (float)(modelNnormal * observedFracNormal);

                            //float cost = observedFracCostFunction.GetCostForType((int)frac.Type);
                            //if (costFunction < cost)
                            //    observedFracCostFunction.AddOrReplaceCostFunction((int)frac.Type, cost);


                            //if ((frac.Type == FractureType.Shear2) || (frac.Type == FractureType.Shear1))
                            //{
                            //}

                        }
                        //we store all of this in the fracture model.

                        //this is the weird normalization that Romain does. Why not just 1 - n1*n2. Why needs to be squared ? 
                        Dictionary<int, float> aux = new Dictionary<int, float>();
                        foreach (int key in observedFracCostFunction.Costs.Keys)
                        {
                            var val = observedFracCostFunction.Costs[key];
                            aux.Add(key, val);// 1.0f - val * val);
                        }
                        observedFracCostFunction.Costs = aux;
                        observedFracture.Extension = observedFracCostFunction;

                    }

                    //we need to remove this fracture from the observed ones because we dont have stress to compare it with 
                    else
                    {
                        observedFracture.Extension = null;
                    }

                    //version 2. Only computes the cost for the same type of fracture as the observed one. 


                    //if (sortedNeighbourFractures.Count > 0)
                    //{  //var sameTypeFracs = sortedNeighbourFractures.Where(t => (int)(t.Type) == (int)observedFracture.Type);
                    //if (sameTypeFracs.Count() > 0)
                    //{
                    //    double minimumCost = double.MaxValue;
                    //    foreach (var f in sameTypeFracs)
                    //    {
                    //        Vector3 modelNnormal = f.Orientation.Normal;
                    //        double costFunction = modelNnormal * observedFracNormal;
                    //        minimumCost = Math.Min(costFunction, minimumCost);
                    //    }

                    //    //so we know the type-cost of the observed fracture. It is minConst 
                    //    observedFracCostFunction.AddOrReplaceCostFunction( (int)observedFracture.Type, minimumCost ); 
                    //}
                    //}


                }//frtacs in well 

            }//well 



        }//body 



    };

}
