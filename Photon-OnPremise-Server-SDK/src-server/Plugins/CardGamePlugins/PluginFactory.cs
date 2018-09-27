using System;
using System.Collections.Generic;
using Photon.Hive.Plugin;
using System.Diagnostics;

namespace CardGamePlugins {

    public class PluginFactory : IPluginFactory {
        public IGamePlugin Create(IPluginHost gameHost, string pluginName, Dictionary<string, string> config, out string errorMsg) {
            PluginBase plugin = new PluginBase();

            switch (pluginName) {
                case CardGameBehaviour.NAME:
                    plugin = new CardGameBehaviour();
                    break;
                default:
                    break;
            }


            if (plugin.SetupInstance(gameHost, config, out errorMsg)) {
                return plugin;
            }
            return null;
        }

    }

}
