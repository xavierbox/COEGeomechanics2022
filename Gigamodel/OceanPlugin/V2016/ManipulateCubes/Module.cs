using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace ManipulateCubes
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    [ModuleAppearance(typeof(ModuleAppearance))]
    public class Module : IModule
    {
        private Process m_manipulatecubesworkstepInstance;
        public Module()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region IModule Members

        /// <summary>
        /// This method runs once in the Module life; when it loaded into the petrel.
        /// This method called first.
        /// </summary>
        public void Initialize()
        {
            DataManager.WorkspaceEvents.Opened += this.WorkspaceOpened;
            DataManager.WorkspaceEvents.Closing += this.WorkspaceClosing;
            DataManager.WorkspaceEvents.Closed += this.WorkspaceClosed;

            // TODO:  Add Module.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
            // Register ManipulateCubes.ManipulateCubesWorkstep
            ManipulateCubes.ManipulateCubesWorkstep manipulatecubesworkstepInstance = new ManipulateCubes.ManipulateCubesWorkstep();
            PetrelSystem.WorkflowEditor.AddUIFactory<ManipulateCubes.ManipulateCubesWorkstep.Arguments>(new ManipulateCubes.ManipulateCubesWorkstep.UIFactory());
            PetrelSystem.WorkflowEditor.Add(manipulatecubesworkstepInstance);
            m_manipulatecubesworkstepInstance = new Slb.Ocean.Petrel.Workflow.WorkstepProcessWrapper(manipulatecubesworkstepInstance);
            PetrelSystem.ProcessDiagram.Add(m_manipulatecubesworkstepInstance, "Plug-ins");

            // TODO:  Add Module.Integrate implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {

            // TODO:  Add Module.IntegratePresentation implementation
        }

        /// <summary>
        /// IModule interface does not define this method. 
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and is called every time when Petrel creates or loads a project.
        /// </summary>
        private void WorkspaceOpened(object sender, WorkspaceEventArgs args)
        {

            // TODO:  Add Workspace Opened eventhandler implementation
        }

        /// <summary>
        /// IModule interface does not define this method. 
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and is called every time before Petrel closes a project.
        /// </summary>
        private void WorkspaceClosing(object sender, WorkspaceCancelEventArgs args)
        {
            // TODO:  Add Workspace Closing eventhandler implementation
        }

        /// <summary>
        /// IModule interface does not define this method. 
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and is called every time after Petrel closed a project.
        /// </summary>
        private void WorkspaceClosed(object sender, WorkspaceEventArgs args)
        {
            // TODO:  Add Workspace Closed eventhandler implementation
        }

        /// <summary>
        /// This method runs once in the Module life.
        /// right before the module is unloaded. 
        /// It usually happens when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
            // Unregister ManipulateCubes.ManipulateCubesWorkstep
            PetrelSystem.WorkflowEditor.RemoveUIFactory<ManipulateCubes.ManipulateCubesWorkstep.Arguments>();
            PetrelSystem.ProcessDiagram.Remove(m_manipulatecubesworkstepInstance);
            DataManager.WorkspaceEvents.Opened -= this.WorkspaceOpened;
            DataManager.WorkspaceEvents.Closing -= this.WorkspaceClosing;
            DataManager.WorkspaceEvents.Closed -= this.WorkspaceClosed;

            // TODO:  Add Module.Disintegrate implementation
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add Module.Dispose implementation
        }

        #endregion

    }

    #region ModuleAppearance Class

    /// <summary>
    /// Appearance (or branding) for a Slb.Ocean.Core.IModule.
    /// This is associated with a module using Slb.Ocean.Core.ModuleAppearanceAttribute.
    /// </summary>
    internal class ModuleAppearance : IModuleAppearance
    {
        /// <summary>
        /// Description of the module.
        /// </summary>
        public string Description
        {
            get { return "Module"; }
        }

        /// <summary>
        /// Display name for the module.
        /// </summary>
        public string DisplayName
        {
            get { return "Module"; }
        }

        /// <summary>
        /// Returns the name of a image resource.
        /// </summary>
        public string ImageResourceName
        {
            get { return null; }
        }

        /// <summary>
        /// A link to the publisher or null.
        /// </summary>
        public Uri ModuleUri
        {
            get { return null; }
        }
    }

    #endregion
}