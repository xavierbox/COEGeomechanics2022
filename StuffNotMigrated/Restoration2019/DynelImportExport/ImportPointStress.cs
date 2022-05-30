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
    class ImportPointStress : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.yyyy
        /// </summary>
        public ImportPointStress() : base("Import Point Stress")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            //ImportPointStress

            ImportPointStressUI ui = new ImportPointStressUI(this);
            Controller = new ImportPointStressController(ui);
            return ui;
        }

        private FractureModel Model { get; set; }

        ImportPointStressController  Controller { get; set; }

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
                return "9becef98-b61d-438b-bc47-662bc8ace76c";
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
                return PetrelImages.PetrelFile;

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
        /// Gets the description of the ImportPointStress
        /// </summary>
        public IDescription Description
        {//ImportPoi
            get { return ImportPointStressDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the ImportPointStress.
        /// Contains Layer, Shorter description and detailed description.
        /// </summary>
        public class ImportPointStressDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static ImportPointStressDescription instance = new ImportPointStressDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static ImportPointStressDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of ImportPointStress
            /// </summary>
            public string Name
            {
                get { return "Import Point Stress"; }
            }
            /// <summary>
            /// Gets the short description of ImportPointStress
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of ImportPointStress
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