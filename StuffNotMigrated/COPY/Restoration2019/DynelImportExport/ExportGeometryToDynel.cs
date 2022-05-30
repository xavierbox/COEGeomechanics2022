using System;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace Restoration
{
    /// <summary>
    /// This class is a standalone Petrel process, cannot be added to a Workflow.
    /// </summary>
    class ExportGeometryToDynel : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public ExportGeometryToDynel() : base("Export Geometry To Restoration")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            return new ExportGeometryToDynelUI(this);
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
                return "b17156c0-18d8-4788-883b-ba76ece706cb";
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
            get { return PetrelImages.Case; }
            private set
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }
        #endregion

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the ExportGeometryToDynel
        /// </summary>
        public IDescription Description
        {
            get { return ExportGeometryToDynelDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the ExportGeometryToDynel.
        /// Contains Layer, Shorter description and detailed description.
        /// </summary>
        public class ExportGeometryToDynelDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static ExportGeometryToDynelDescription instance = new ExportGeometryToDynelDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static ExportGeometryToDynelDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of ExportGeometryToDynel
            /// </summary>
            public string Name
            {
                get { return "Export Geometry To Restore"; }
            }
            /// <summary>
            /// Gets the short description of ExportGeometryToDynel
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of ExportGeometryToDynel
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