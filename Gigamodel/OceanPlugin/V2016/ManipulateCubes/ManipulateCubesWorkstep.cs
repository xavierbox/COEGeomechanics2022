using System;

using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using System.Collections.Generic;

namespace ManipulateCubes
{
    /// <summary>
    /// This class contains all the methods and subclasses of the ManipulateCubesWorkstep.
    /// Worksteps are displayed in the workflow editor.
    /// </summary>
    class ManipulateCubesWorkstep : Workstep<ManipulateCubesWorkstep.Arguments>, IExecutorSource, IAppearance, IDescriptionSource
    {
        #region Overridden Workstep methods

        /// <summary>
        /// Creates an empty Argument instance
        /// </summary>
        /// <returns>New Argument instance.</returns>

        protected override ManipulateCubesWorkstep.Arguments CreateArgumentPackageCore(IDataSourceManager dataSourceManager)
        {
            return new Arguments(dataSourceManager);
        }
        /// <summary>
        /// Copies the Arguments instance.
        /// </summary>
        /// <param name="fromArgumentPackage">the source Arguments instance</param>
        /// <param name="toArgumentPackage">the target Arguments instance</param>
        protected override void CopyArgumentPackageCore(Arguments fromArgumentPackage, Arguments toArgumentPackage)
        {
            DescribedArgumentsHelper.Copy(fromArgumentPackage, toArgumentPackage);
        }

        /// <summary>
        /// Gets the unique identifier for this Workstep.
        /// </summary>
        protected override string UniqueIdCore
        {
            get
            {
                return "eece106a-b983-423c-83d7-05eb9712ca26";
            }
        }
        #endregion

        #region IExecutorSource Members and Executor class

        /// <summary>
        /// Creates the Executor instance for this workstep. This class will do the work of the Workstep.
        /// </summary>
        /// <param name="argumentPackage">the argumentpackage to pass to the Executor</param>
        /// <param name="workflowRuntimeContext">the context to pass to the Executor</param>
        /// <returns>The Executor instance.</returns>
        public Slb.Ocean.Petrel.Workflow.Executor GetExecutor(object argumentPackage, WorkflowRuntimeContext workflowRuntimeContext)
        {
            return new Executor(argumentPackage as Arguments, workflowRuntimeContext);
        }

        public class Executor : Slb.Ocean.Petrel.Workflow.Executor
        {
            Arguments arguments;
            WorkflowRuntimeContext context;

            public Executor(Arguments arguments, WorkflowRuntimeContext context)
            {
                this.arguments = arguments;
                this.context = context;
            }

            public override void ExecuteSimple()
            {
                // TODO: Implement the workstep logic here.
            }
        }

        #endregion

        /// <summary>
        /// ArgumentPackage class for ManipulateCubesWorkstep.
        /// Each public property is an argument in the package.  The name, type and
        /// input/output role are taken from the property and modified by any
        /// attributes applied.
        /// </summary>
        public class Arguments : DescribedArgumentsByReflection
        {

            public Dictionary<string, SplitSeismicCube> splitCubes = new Dictionary<string, SplitSeismicCube>();
            public List<DateTime> pressureDates = new List<DateTime>();
            //float undefinedCellsOffset = 0.0f;
            //float undefinedCellsGradient = 0.0f;
            //string pressureModelName = "DefaultName";
            //bool selectedAsNew = true;

            public Arguments()
                : this(DataManager.DataSourceManager)
            {
            }

            public Arguments(IDataSourceManager dataSourceManager)
            {
            }

            private Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCollection seismicCollection;
            private Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube seismicCube1;
            private Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube seismicCube2;
            private Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube seismicCube3;
            private Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube seismicCube4;
            private Object anObject;

            public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCollection SeismicCollection
            {
                internal get { return this.seismicCollection; }
                set { this.seismicCollection = value; }
            }

            public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube SeismicCube1
            {
                internal get { return this.seismicCube1; }
                set { this.seismicCube1 = value; }
            }

            public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube SeismicCube2
            {
                internal get { return this.seismicCube2; }
                set { this.seismicCube2 = value; }
            }

            public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube SeismicCube3
            {
                internal get { return this.seismicCube3; }
                set { this.seismicCube3 = value; }
            }

            public Slb.Ocean.Petrel.DomainObject.Seismic.SeismicCube SeismicCube4
            {
                internal get { return this.seismicCube4; }
                set { this.seismicCube4 = value; }
            }

            public Object AnObject
            {
                internal get { return this.anObject; }
                set { this.anObject = value; }
            }


        }

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
        /// Gets the description of the ManipulateCubesWorkstep
        /// </summary>
        public IDescription Description
        {
            get { return ManipulateCubesWorkstepDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the ManipulateCubesWorkstep.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class ManipulateCubesWorkstepDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private static ManipulateCubesWorkstepDescription instance = new ManipulateCubesWorkstepDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static ManipulateCubesWorkstepDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of ManipulateCubesWorkstep
            /// </summary>
            public string Name
            {
                get { return "ManipulateCubesWorkstep"; }
            }
            /// <summary>
            /// Gets the short description of ManipulateCubesWorkstep
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of ManipulateCubesWorkstep
            /// </summary>
            public string Description
            {
                get { return ""; }
            }

            #endregion
        }
        #endregion

        public class UIFactory : WorkflowEditorUIFactory
        {
            /// <summary>
            /// This method creates the dialog UI for the given workstep, arguments
            /// and context.
            /// </summary>
            /// <param name="workstep">the workstep instance</param>
            /// <param name="argumentPackage">the arguments to pass to the UI</param>
            /// <param name="context">the underlying context in which the UI is being used</param>
            /// <returns>a Windows.Forms.Control to edit the argument package with</returns>
            protected override System.Windows.Forms.Control CreateDialogUICore(Workstep workstep, object argumentPackage, WorkflowContext context)
            {
                return new ManipulateCubesWorkstepUI((ManipulateCubesWorkstep)workstep, (Arguments)argumentPackage, context);
            }
        }




    }
}