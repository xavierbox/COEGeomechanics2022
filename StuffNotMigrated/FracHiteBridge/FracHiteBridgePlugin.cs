using Slb.Ocean.Core;
using System;
using System.Collections.Generic;

namespace FracHiteBridge
{
    public class FracHiteBridgePlugin : Slb.Ocean.Core.Plugin
    {
        public override string AppVersion
        {
            get { return "2020.1"; }
        }

        public override string Author
        {
            get { return "XTeijeiro"; }
        }

        public override string Contact
        {
            get { return "contact@company.info"; }
        }

        public override IEnumerable<PluginIdentifier> Dependencies
        {
            get { return null; }
        }

        public override string Description
        {
            get { return ""; }
        }

        public override string ImageResourceName
        {
            get { return null; }
        }

        public override Uri PluginUri
        {
            get { return new Uri("http://www.pluginuri.info"); }
        }

        public override IEnumerable<ModuleReference> Modules
        {
            get
            {
                yield return new ModuleReference(typeof(FracHiteBridgeModule));

                // Please fill this method with your modules with lines like this:
                //yield return new ModuleReference(typeof(Module));
            }
        }

        public override string Name
        {
            get { return "FracHiteBridge"; }
        }

        public override PluginIdentifier PluginId
        {
            get { return new PluginIdentifier(GetType().FullName, GetType().Assembly.GetName().Version); }
        }

        public override ModuleTrust Trust
        {
            get { return new ModuleTrust("Default"); }
        }
    }
}