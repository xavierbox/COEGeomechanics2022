using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using System;

namespace Gigamodel
{
    /// <summary>
    /// This class is a standalone Petrel process, cannot be added to a Workflow.
    /// </summary>
    internal class EnhanceSeismic : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public EnhanceSeismic() : base("EnhanceSeismic")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            return new EnhanceSeismicUI(this);
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
                return "6997bc4f-0ee7-4bc2-8407-d248b021337b";
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
            get { return PetrelImages.Modules; }
            private set
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }

        #endregion IAppearance Members

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the EnhanceSeismic
        /// </summary>
        public IDescription Description
        {
            get { return EnhanceSeismicDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the EnhanceSeismic.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class EnhanceSeismicDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static EnhanceSeismicDescription instance = new EnhanceSeismicDescription();

            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static EnhanceSeismicDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of EnhanceSeismic
            /// </summary>
            public string Name
            {
                get { return "EnhanceSeismic"; }
            }

            /// <summary>
            /// Gets the short description of EnhanceSeismic
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }

            /// <summary>
            /// Gets the detailed description of EnhanceSeismic
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