using System;
using Slb.Ocean.Core;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace COEGeomechanicsPlugin
{
    /// <summary>
    /// This class will control the lifecycle of the Module.
    /// The order of the methods are the same as the calling order.
    /// </summary>
    public class COEGeomechanicsModule1 : IModule
    {
    private FracSimulator m_fracsimulatorInstance;
    private CompletionQualityProcess m_completionqualityprocessInstance;
       //private Process1 m_process1Instance;

        public COEGeomechanicsModule1()
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
            // TODO:  Add COEGeomechanicsModule1.Initialize implementation
        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the not UI related components.
        /// (eg: datasource, plugin)
        /// </summary>
        public void Integrate()
        {
        // Register COEGeomechanicsPlugin.FracSimulator
            m_fracsimulatorInstance = new FracSimulator();
            PetrelSystem.ProcessDiagram.Add(m_fracsimulatorInstance, "COEGeomechanics");

        // Register COEGeomechanicsPlugin.CompletionQualityProcess
            m_completionqualityprocessInstance = new CompletionQualityProcess();
            PetrelSystem.ProcessDiagram.Add(m_completionqualityprocessInstance, "COEGeomechanics");

        }

        /// <summary>
        /// This method runs once in the Module life. 
        /// In this method, you can do registrations of the UI related components.
        /// (eg: settingspages, treeextensions)
        /// </summary>
        public void IntegratePresentation()
        {

            // TODO:  Add COEGeomechanicsModule1.IntegratePresentation implementation
        }

        /// <summary>
        /// This method runs once in the Module life.
        /// right before the module is unloaded. 
        /// It usually happens when the application is closing.
        /// </summary>
        public void Disintegrate()
        {
        // Unregister COEGeomechanicsPlugin.FracSimulator
            PetrelSystem.ProcessDiagram.Remove(m_fracsimulatorInstance);
        // Unregister COEGeomechanicsPlugin.CompletionQualityProcess
            PetrelSystem.ProcessDiagram.Remove(m_completionqualityprocessInstance);
        // Unregister COEGeomechanicsPlugin.Process1
            //PetrelSystem.ProcessDiagram.Remove(m_process1Instance);
            // TODO:  Add COEGeomechanicsModule1.Disintegrate implementation
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            // TODO:  Add COEGeomechanicsModule1.Dispose implementation
        }

        #endregion

    }


}