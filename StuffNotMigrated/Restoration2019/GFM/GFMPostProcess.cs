using System;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace Restoration.GFM
{
    /// <summary>
    /// This class is a standalone Petrel process, cannot be added to a Workflow.
    /// </summary>
    class GFMPostProcess : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GFMPostProcess() : base("Forward Modelling Processing")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            return new GFMPostProcessUI(this);
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
                return "76f46a0a-fa6d-41a6-ba23-012af7309a31";
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
        /// Gets the description of the GFMPostProcess
        /// </summary>
        public IDescription Description
        {
            get { return GFMPostProcessDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the GFMPostProcess.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class GFMPostProcessDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static GFMPostProcessDescription instance = new GFMPostProcessDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static GFMPostProcessDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of GFMPostProcess
            /// </summary>
            public string Name
            {
                get { return "Simulation Post-Process"; }
            }
            /// <summary>
            /// Gets the short description of GFMPostProcess
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of GFMPostProcess
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