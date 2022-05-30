using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace Restoration
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    [ModuleAppearance(typeof(RestorationModuleAppearance))]
    public class RestorationModule : IModule
    {
      
        private Restoration.GFMGeometry m_gfmgeometryInstance;
        private Restoration.GFM.GFMPostProcess m_gfmpostprocessInstance;


        private Restoration.ExportGeometryToDynel m_exportgeometrytodynelInstance;
         private Restoration.ImportPointStress m_importpointstressInstance;
         private Restoration.DRestorationProcess m_drestorationprocessInstance;
        public RestorationModule()
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

            // TODO:  Add RestorationModule.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
          
            // Register Restoration.GFMGeometry
            m_gfmgeometryInstance = new Restoration.GFMGeometry();
            PetrelSystem.ProcessDiagram.Add(m_gfmgeometryInstance, "Geomechanics Forward Modelling");// "Plug-ins");
                                                                                                     // Register Restoration.ExportGeometryToDynel

            // Register Restoration.GFM.GFMPostProcess
            m_gfmpostprocessInstance = new Restoration.GFM.GFMPostProcess();
            PetrelSystem.ProcessDiagram.Add(m_gfmpostprocessInstance, "Geomechanics Forward Modelling");




            m_exportgeometrytodynelInstance = new Restoration.ExportGeometryToDynel();
            PetrelSystem.ProcessDiagram.Add(m_exportgeometrytodynelInstance, "Restoration Tools");
            // Register Restoration.ImportPointStressProcess
            //   m_importpointstressprocessInstance = new Restoration.ImportPointStressProcess();
            //   PetrelSystem.ProcessDiagram.Add(m_importpointstressprocessInstance, "Plug-ins");


            // Register Restoration.ImportPointStress
            m_importpointstressInstance = new Restoration.ImportPointStress();
            PetrelSystem.ProcessDiagram.Add(m_importpointstressInstance, "Restoration Tools");// "Plug-ins");


            m_drestorationprocessInstance = new Restoration.DRestorationProcess();
            PetrelSystem.ProcessDiagram.Add(m_drestorationprocessInstance, "Restoration Tools"); //"Plug-ins");


            // Register Restoration.LogDFN
            //m_logdfnInstance = new Restoration.LogDFN();
            //PetrelSystem.ProcessDiagram.Add(m_logdfnInstance, "Petrobras Restoration Tools");// "Plug-ins");
            // Register Restoration.VRestoration
            //m_vrestorationInstance = new Restoration.VRestorationProcess();
            //PetrelSystem.ProcessDiagram.Add(m_vrestorationInstance, "Plug-ins");
            // Register Restoration.DRestorationProcess
   //PetrelSystem.ProcessDiagram.Add(m_drestorationprocessInstance, "Petrobras Restoration Tools"); //"Plug-ins");

            // TODO:  Add RestorationModule.Integrate implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {

            // TODO:  Add RestorationModule.IntegratePresentation implementation
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
            // Unregister Restoration.GFM.GFMPostProcess
             PetrelSystem.ProcessDiagram.Remove(m_gfmgeometryInstance);
            PetrelSystem.ProcessDiagram.Remove(m_gfmpostprocessInstance);


            // Unregister Restoration.ExportGeometryToDynel
            PetrelSystem.ProcessDiagram.Remove(m_exportgeometrytodynelInstance);
 
            PetrelSystem.ProcessDiagram.Remove(m_importpointstressInstance);
 
            PetrelSystem.ProcessDiagram.Remove(m_drestorationprocessInstance);
            DataManager.WorkspaceEvents.Opened -= this.WorkspaceOpened;
            DataManager.WorkspaceEvents.Closing -= this.WorkspaceClosing;
            DataManager.WorkspaceEvents.Closed -= this.WorkspaceClosed;

            // TODO:  Add RestorationModule.Disintegrate implementation
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add RestorationModule.Dispose implementation
        }

        #endregion

    }

    #region ModuleAppearance Class

    /// <summary>
    /// Appearance (or branding) for a Slb.Ocean.Core.IModule.
    /// This is associated with a module using Slb.Ocean.Core.ModuleAppearanceAttribute.
    /// </summary>
    internal class RestorationModuleAppearance : IModuleAppearance
    {
        /// <summary>
        /// Description of the module.
        /// </summary>
        public string Description
        {
            get { return "RestorationModule"; }
        }

        /// <summary>
        /// Display name for the module.
        /// </summary>
        public string DisplayName
        {
            get { return "RestorationModule"; }
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