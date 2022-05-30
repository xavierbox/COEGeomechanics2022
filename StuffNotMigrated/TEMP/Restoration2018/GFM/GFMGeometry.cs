using Restoration.Controllers;
using Restoration.GFM;
using Restoration.GFM.Services;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using System;

namespace Restoration
{
    /// <summary>
    /// This class is a standalone Petrel process, cannot be added to a Workflow.
    /// </summary>
    internal class GFMGeometry : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GFMGeometry() : base("The Awesome Forward Modelling Settings")
        {
            Model = GFMSerializer.GetPersistedModel();
        }

        public GFMProject Model { get; set; }

        private GFMController Controller { get; set; }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            GFMGeometryUI ui = new GFMGeometryUI(this);
            Controller = new GFMController(ui, Model, new GFMSerializer());

            return ui;
        }

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
                return "61bf88ce-6f76-483b-b661-210fe50aa765";
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
        /// Gets the description of the GFMGeometry
        /// </summary>
        public IDescription Description
        {
            get { return GFMGeometryDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the GFMGeometry.
        /// Contains Layer, Shorter description and detailed description.
        /// </summary>
        public class GFMGeometryDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static GFMGeometryDescription instance = new GFMGeometryDescription();

            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static GFMGeometryDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of GFMGeometry
            /// </summary>
            public string Name
            {
                get { return "Project and Simulation Setup"; }
            }

            /// <summary>
            /// Gets the short description of GFMGeometry
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }

            /// <summary>
            /// Gets the detailed description of GFMGeometry
            /// </summary>
            public string Description
            {
                get { return ""; }
            }

            #endregion IDescription Members
        }

        #endregion IDescriptionSource Members
    }
}