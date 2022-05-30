using Gigamodel.Controllers;
using Gigamodel.Data;
using Gigamodel.Services;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

using System;

namespace Gigamodel
{
    internal class GigaModelBuildProcess : Slb.Ocean.Petrel.Workflow.Process, IAppearance, IDescriptionSource
    {
        public GigaModelBuildProcess() : base("Giga-model builder")
        {
            InputValidationController = null;
            ImportResultsController = null;
            ExportSimulationController = null;
        }

        private InputValidationController InputValidationController
        {
            get; set;  //private set;
        }

        private ImportResultsController ImportResultsController
        {
            get; set;
        }

        private ExportSimulationController ExportSimulationController
        {
            get; set;
        }

        private GigaModelDataModel Model => ModelCreateDestroySvc.GetOrCreateGigaModelForPetrelProject();

        private GigaModelDataModel PetrelDirtyModel
        {
            get
            {
                return ModelCreateDestroySvc.FindGigaModelInPetrelTree();// GetOrCreateGigaModelForPetrelProject();
            }
        }

        #region Process overrides

        protected override System.Windows.Forms.Control CreateUICore()
        {
            GigaModelProcessUI GigaModelView = new GigaModelProcessUI();

            //if(InputValidationController == null )
            InputValidationController = new InputValidationController(GigaModelView, Model);

            //if (ImportResultsController == null)
            ImportResultsController = new ImportResultsController(GigaModelView);//, Model);

            //if (ExportSimulationController == null)
            ExportSimulationController = new ExportSimulationController(GigaModelView, Model);

            //ModelCreateDestroyController is static, it is more like a service

            GigaModelView.CancelClicked += ( sender, evt ) =>
            {
                bool success = ModelCreateDestroySvc.SerializeGigaModel(PetrelDirtyModel);

                if (!success)
                {
                    MessageService.ShowMessage("Cannot serialize the object. Project will not be saved in disk", MessageType.ERROR);
                }
            };

            return GigaModelView;
        }

        protected override sealed void OnActivateCore()
        {
            base.OnActivateCore();
        }

        protected override sealed void OnDeactivateCore()
        {
            base.OnDeactivateCore();
        }

        protected override string UniqueIdCore
        {
            get
            {
                return "3293e8ff-3209-4dc5-aff9-72bf93c0d517";
            }
        }

        #endregion Process overrides

        #region IAppearance Members

        public event EventHandler<TextChangedEventArgs> TextChanged;

        protected void RaiseTextChanged()
        {
            if (this.TextChanged != null)
                this.TextChanged(this, new TextChangedEventArgs(this));
        }

        public string Text
        {
            get { return Description.Name; }
            private set
            {
                // TODO: implement set
                this.RaiseTextChanged();
            }
        }

        public event EventHandler<ImageChangedEventArgs> ImageChanged;

        protected void RaiseImageChanged()
        {
            if (this.ImageChanged != null)
                this.ImageChanged(this, new ImageChangedEventArgs(this));
        }

        public System.Drawing.Bitmap Image
        {
            get { return PetrelImages.Models; }
            private set
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }

        #endregion IAppearance Members

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the GigaModelBuildProcess
        /// </summary>
        public IDescription Description
        {
            get { return GigaModelBuildProcessDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the GigaModelBuildProcess.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class GigaModelBuildProcessDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static GigaModelBuildProcessDescription instance = new GigaModelBuildProcessDescription();

            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static GigaModelBuildProcessDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of GigaModelBuildProcess
            /// </summary>
            public string Name
            {
                get { return "Giga-model builder"; }
            }

            /// <summary>
            /// Gets the short description of GigaModelBuildProcess
            /// </summary>
            public string ShortDescription
            {
                get { return "Create a 3D Visage model for structured grids takeing as input Seismic cubes"; }
            }

            /// <summary>
            /// Gets the detailed description of GigaModelBuildProcess
            /// </summary>
            public string Description
            {
                get { return "Create a 3D Visage model for structured grids takeing as input Seismic cubes"; }
            }

            #endregion IDescription Members
        }

        #endregion IDescriptionSource Members
    }
}