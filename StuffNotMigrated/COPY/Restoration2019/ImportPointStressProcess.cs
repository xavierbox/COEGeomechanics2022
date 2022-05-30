using System;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Restoration.Controllers;
using Restoration.Model;

namespace Restoration
{
    /// <summary>
    /// This class is a standalone Petrel process, cannot be added to a Workflow.
    /// </summary>
    class ImportPointStressProcess : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ImportPointStressProcess() : base("ImportPointStressProcess")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            ImportPointStressProcessUI ui = new ImportPointStressProcessUI(this);
            //FracturePredictionController = new FracturePredictionController(ui, Model);
            return ui;
        }

        private FractureModel Model { get; set; }


        FracturePredictionController FracturePredictionController { get; set; }


        /// <summary>
        /// Runs when the process is activated in Petrel.
        /// </summary>
        protected override sealed void OnActivateCore()
        {
            base.OnActivateCore();
        }
        /// <summary>
        /// Runs when the process is deactivated in Petrel.
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
                return "44aa2572-dbac-46b3-b005-ed7a44f94c35";
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
            get { return PetrelImages.Modules; }
            private set
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }
        #endregion

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the ImportPointStressProcess
        /// </summary>
        public IDescription Description
        {
            get { return ImportPointStressProcessDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the ImportPointStressProcess.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class ImportPointStressProcessDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static ImportPointStressProcessDescription instance = new ImportPointStressProcessDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static ImportPointStressProcessDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of ImportPointStressProcess
            /// </summary>
            public string Name
            {
                get { return "ImportPointStressProcess"; }
            }
            /// <summary>
            /// Gets the short description of ImportPointStressProcess
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of ImportPointStressProcess
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