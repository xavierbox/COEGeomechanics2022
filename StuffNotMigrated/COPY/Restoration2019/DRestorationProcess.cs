using System;
using Restoration.Controllers;
using Restoration.Model;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace Restoration
{
    /// <summary>
    /// This class is a standalone Petrel _process, cannot be added to a Workflow.
    /// </summary>
    class DRestorationProcess : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public DRestorationProcess() : base("Restoration Stress-Fracture Analysis")
        {
            Model = new FractureModel();
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the _process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
         
            DRestorationProcessUI ui = new DRestorationProcessUI(this);

            //ui.Size = new System.Drawing.Size(750, 650);
            //ui.MaximumSize = new System.Drawing.Size(750, 650);
            //ui.MinimumSize = new System.Drawing.Size(750, 650);

            FracturePredictionController = new FracturePredictionController(ui, Model);
            return ui;

        }

        private FractureModel Model { get; set; }


        private FracturePredictionController FracturePredictionController { get; set; }


        /// <summary>
        /// Runs when the _process is activated in Petrel.
        /// </summary>
        protected override sealed void OnActivateCore()
        {
            base.OnActivateCore();
        }
        /// <summary>
        /// Runs when the _process is deactivated in Petrel.
        /// </summary>
        protected override sealed void OnDeactivateCore()
        {
            base.OnDeactivateCore();
        }

        /// <summary>
        /// Gets the unique identifier for this Process.
        /// </summary>
        protected override string UniqueIdCore
        {
            get
            {
                return "992aae44-cc99-45bb-ae18-38ff84ca5cb6";
            }
        }

        #endregion

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
            get
            {

                return PetrelImages.FaultModel;
                //return PetrelImages.Modules;
            }
            private set
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }
        #endregion

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the DRestorationProcess
        /// </summary>
        public IDescription Description
        {
            get { return DRestorationProcessDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the DRestorationProcess.
        /// Contains Layer, Shorter description and detailed description.
        /// </summary>
        public class DRestorationProcessDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static DRestorationProcessDescription instance = new DRestorationProcessDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static DRestorationProcessDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of DRestorationProcess
            /// </summary>
            public string Name
            {
                get { return "Stress/Fracture Analysis"; }
            }
            /// <summary>
            /// Gets the short description of DRestorationProcess
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of DRestorationProcess
            /// </summary>
            public string Description
            {
                get { return ""; }
            }

            #endregion
        }
        #endregion
    }
}