using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using System;

namespace Gigamodel
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    [ModuleAppearance(typeof(ModuleAppearance))]
    public class GigaModelModule : IModule
    {
        //private Gigamodel.BigDataCalculator m_bigdatacalculatorInstance;
        //private Gigamodel.EnhanceSeismic m_enhanceseismicInstance;

        private Gigamodel.GigamodelLogSeismic m_gigamodellogseismicInstance;
        private Gigamodel.GigaModelBuildProcess m_buildprocessInstance;

        public GigaModelModule()
        {
        }

        #region IModule Members

        public void Initialize()
        {
            DataManager.WorkspaceEvents.Opened += this.WorkspaceOpened;
            DataManager.WorkspaceEvents.Closing += this.WorkspaceClosing;
            DataManager.WorkspaceEvents.Closed += this.WorkspaceClosed;

            // TODO:  Add Module.Initialize implementation
        }

        public void Integrate()
        {
            // Register Gigamodel.BigDataCalculator
            //m_bigdatacalculatorInstance = new Gigamodel.BigDataCalculator();
            //PetrelSystem.ProcessDiagram.Add(m_bigdatacalculatorInstance, "Large-scale Geomechanics (experimental)");
            // Register Gigamodel.EnhanceSeismic
            //m_enhanceseismicInstance = new Gigamodel.EnhanceSeismic();
            //PetrelSystem.ProcessDiagram.Add(m_enhanceseismicInstance, "Plug-ins");
            // Register Gigamodel.GigamodelLogSeismic
            m_gigamodellogseismicInstance = new Gigamodel.GigamodelLogSeismic();
            PetrelSystem.ProcessDiagram.Add(m_gigamodellogseismicInstance, "Large-scale Geomechanics (experimental)");
            // Register Gigamodel.GFMProjectCreate
            // Register GigaModelBuildProcess
            m_buildprocessInstance = new Gigamodel.GigaModelBuildProcess();
            PetrelSystem.ProcessDiagram.Add(m_buildprocessInstance, "Large-scale Geomechanics (experimental)");
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
        private void WorkspaceOpened( object sender, WorkspaceEventArgs args )
        {
            // TODO:  Add Workspace Opened eventhandler implementation
        }

        /// <summary>
        /// IModule interface does not define this method.
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and is called every time before Petrel closes a project.
        /// </summary>
        private void WorkspaceClosing( object sender, WorkspaceCancelEventArgs args )
        {
            // TODO:  Add Workspace Closing eventhandler implementation
        }

        /// <summary>
        /// IModule interface does not define this method.
        /// It is an eventhandler method, which is subscribed in the Initialize() method above,
        /// and is called every time after Petrel closed a project.
        /// </summary>
        private void WorkspaceClosed( object sender, WorkspaceEventArgs args )
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
            // Unregister Gigamodel.BigDataCalculator
            //PetrelSystem.ProcessDiagram.Remove(m_bigdatacalculatorInstance);
            // Unregister Gigamodel.EnhanceSeismic
            //PetrelSystem.ProcessDiagram.Remove(m_enhanceseismicInstance);
            // Unregister Gigamodel.GigamodelLogSeismic
            //PetrelSystem.ProcessDiagram.Remove(m_gigamodellogseismicInstance);
            // Unregister Gigamodel.GFMProjectCreate
            // Unregister Gigamodel.GigaModelBuildProcess
            PetrelSystem.ProcessDiagram.Remove(m_buildprocessInstance);
            DataManager.WorkspaceEvents.Opened -= this.WorkspaceOpened;
            DataManager.WorkspaceEvents.Closing -= this.WorkspaceClosing;
            DataManager.WorkspaceEvents.Closed -= this.WorkspaceClosed;

            // TODO:  Add Module.Disintegrate implementation
        }

        #endregion IModule Members

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add Module.Dispose implementation
        }

        #endregion IDisposable Members
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

    #endregion ModuleAppearance Class
}