using System;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace Restoration
{
    /// <summary>
    /// This class is a standalone Petrel _process, cannot be added to a Workflow.
    /// </summary>
    class VRestorationProcess : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public VRestorationProcess() : base("VRestoration")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the _process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            return new VRestorationUI(this);
        }

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
                return "3de07834-5147-4762-b915-5fd433d89b63";
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
        /// Gets the description of the VRestoration
        /// </summary>
        public IDescription Description
        {
            get { return VRestorationDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the VRestoration.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class VRestorationDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static VRestorationDescription instance = new VRestorationDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static VRestorationDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of VRestoration
            /// </summary>
            public string Name
            {
                get { return "VRestoration"; }
            }
            /// <summary>
            /// Gets the short description of VRestoration
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of VRestoration
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