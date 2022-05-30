using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using System;

namespace FracHiteBridge
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    [ModuleAppearance(typeof(FracHiteBridgeModuleAppearance))]
    public class FracHiteBridgeModule : IModule
    {
        private FracHiteBridge.FractHiteResultsProcess m_fracthiteresultsprocessInstance;
        private FracHiteDataExporterProcess m_frachitebridgeprocessInstance;

        public FracHiteBridgeModule()
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
            // TODO:  Add FracHiteBridgeModule.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life.
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
            // Register FracHiteBridge.FractHiteResultsProcess
            m_fracthiteresultsprocessInstance = new FracHiteBridge.FractHiteResultsProcess();
            PetrelSystem.ProcessDiagram.Add(m_fracthiteresultsprocessInstance, "Plug-ins");
            // Register FracHiteBridge.FracHiteBridgeProcess
            m_frachitebridgeprocessInstance = new FracHiteDataExporterProcess();
            PetrelSystem.ProcessDiagram.Add(m_frachitebridgeprocessInstance, "Plug-ins");

            // TODO:  Add FracHiteBridgeModule.Integrate implementation
        }

        /// <summary>
        /// This method runs once in the Module life.
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {
            // TODO:  Add FracHiteBridgeModule.IntegratePresentation implementation
        }

        /// <summary>
        /// This method runs once in the Module life.
        /// right before the module is unloaded.
        /// It usually happens when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
            // Unregister FracHiteBridge.FractHiteResultsProcess
            PetrelSystem.ProcessDiagram.Remove(m_fracthiteresultsprocessInstance);
            // Unregister FracHiteBridge.FracHiteBridgeProcess
            PetrelSystem.ProcessDiagram.Remove(m_frachitebridgeprocessInstance);
            // TODO:  Add FracHiteBridgeModule.Disintegrate implementation
        }

        #endregion IModule Members

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add FracHiteBridgeModule.Dispose implementation
        }

        #endregion IDisposable Members
    }

    #region ModuleAppearance Class

    /// <summary>
    /// Appearance (or branding) for a Slb.Ocean.Core.IModule.
    /// This is associated with a module using Slb.Ocean.Core.ModuleAppearanceAttribute.
    /// </summary>
    internal class FracHiteBridgeModuleAppearance : IModuleAppearance
    {
        /// <summary>
        /// Description of the module.
        /// </summary>
        public string Description
        {
            get { return "FracHiteBridgeModule"; }
        }

        /// <summary>
        /// Display name for the module.
        /// </summary>
        public string DisplayName
        {
            get { return "FracHiteBridgeModule"; }
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