using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace SeismicPoroelastic
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    [ModuleAppearance(typeof(ModuleAppearance))]
    public class Module : IModule
    {
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
            // Register SeismicPoroelastic.PoroelasticStressAtribute
            PetrelSystem.AddDataSourceFactory(new SeismicPoroelastic.PoroelasticStressAtribute.ArgumentPackageDataSourceFactory());
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
            // Register SeismicPoroelastic.PoroelasticStressAtribute
            if (Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService == null)
                throw new LifecycleException("Required AttributeService is not available.");
            Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService.AddSeismicAttribute(new SeismicPoroelastic.PoroelasticStressAtribute());
            Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService.AddSeismicAttributeUIFactory(new SeismicPoroelastic.PoroelasticStressAtribute.UIFactory());

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
            // Unregister SeismicPoroelastic.PoroelasticStressAtribute
            if (Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService.GetSeismicAttributeUIFactory<SeismicPoroelastic.PoroelasticStressAtribute>() != null)
                Slb.Ocean.Petrel.Seismic.SeismicSystem.SeismicAttributeService.RemoveSeismicAttributeUIFactory<SeismicPoroelastic.PoroelasticStressAtribute.Arguments>();
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