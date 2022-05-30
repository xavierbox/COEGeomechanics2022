using Slb.Ocean.Core;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using System;

namespace Gigamodel
{
    /// <summary>
    /// This class is a standalone Petrel process, cannot be added to a Workflow.
    /// </summary>
    internal class GigamodelLogSeismic : Process, IAppearance, IDescriptionSource
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public GigamodelLogSeismic() : base("Seismic to logs")
        {
        }

        #region Process overrides

        /// <summary>
        /// Creates the UI of the process.
        /// </summary>
        /// <returns>the UI contol</returns>
        protected override System.Windows.Forms.Control CreateUICore()
        {
            return new GigamodelLogSeismicUI(this);
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
                return "a671e795-2b5d-46b9-b197-8ebde2dd7126";
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
            get
            {
                ;
                return PetrelImages.WellPlan;
            }
            //private set
            //{
            //    // TODO: implement set
            //    this.RaiseImageChanged();
            //}
        }

        #endregion IAppearance Members

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the GigamodelLogSeismic
        /// </summary>
        public IDescription Description
        {
            get { return GigamodelLogSeismicDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the GigamodelLogSeismic.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class GigamodelLogSeismicDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static GigamodelLogSeismicDescription instance = new GigamodelLogSeismicDescription();

            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static GigamodelLogSeismicDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of GigamodelLogSeismic
            /// </summary>
            public string Name
            {
                get { return "Seismic to logs"; }
            }

            /// <summary>
            /// Gets the short description of GigamodelLogSeismic
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }

            /// <summary>
            /// Gets the detailed description of GigamodelLogSeismic
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