using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using System;

namespace CompletionQualityCubes
{
    /// <summary>
    /// This class is a standalone Petrel process, cannot be added to a Workflow.
    /// </summary>
    class CompletionQualityCubesProcess : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public CompletionQualityCubesProcess() : base("CompletionQualityCubesProcess")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            return new CompletionQualitySimulation(this);
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
                return "3043b9bd-54dd-46aa-aa04-d6eaed41acf7";
            }
        }

        #endregion

        #region IAppearance Members
        public event EventHandler<TextChangedEventArgs> TextChanged;
        protected void RaiseTextChanged()
        {
            this.TextChanged?.Invoke(this, new TextChangedEventArgs(this));
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
            this.ImageChanged?.Invoke(this, new ImageChangedEventArgs(this));
        }

        public System.Drawing.Bitmap Image
        {
            get { return PetrelImages.Examine_32; }
            private set
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }
        #endregion

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the CompletionQualityCubesProcess
        /// </summary>
        public IDescription Description
        {
            get { return CompletionQualityCubesProcessDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the CompletionQualityCubesProcess.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class CompletionQualityCubesProcessDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static CompletionQualityCubesProcessDescription instance = new CompletionQualityCubesProcessDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static CompletionQualityCubesProcessDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of CompletionQualityCubesProcess
            /// </summary>
            public string Name
            {
                get { return "CompletionQualityCubesProcess"; }
            }
            /// <summary>
            /// Gets the short description of CompletionQualityCubesProcess
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of CompletionQualityCubesProcess
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