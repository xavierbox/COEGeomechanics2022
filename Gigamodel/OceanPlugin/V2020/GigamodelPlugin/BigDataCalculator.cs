using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using System;

namespace Gigamodel
{
    /// <summary>
    /// This class is a standalone Petrel process, cannot be added to a Workflow.
    /// </summary>
    internal class BigDataCalculator : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public BigDataCalculator() : base("BigDataCalculator")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            return new BigDataCalculatorUI(this);
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
                return "4d3fdb68-dd0b-41da-9af4-04479d9b829a";
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
        /// Gets the description of the BigDataCalculator
        /// </summary>
        public IDescription Description
        {
            get { return BigDataCalculatorDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the BigDataCalculator.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class BigDataCalculatorDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static BigDataCalculatorDescription instance = new BigDataCalculatorDescription();

            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static BigDataCalculatorDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of BigDataCalculator
            /// </summary>
            public string Name
            {
                get { return "BigDataCalculator"; }
            }

            /// <summary>
            /// Gets the short description of BigDataCalculator
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }

            /// <summary>
            /// Gets the detailed description of BigDataCalculator
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