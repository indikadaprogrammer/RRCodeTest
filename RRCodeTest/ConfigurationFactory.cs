using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RRCodeTest
{
    public class ConfigurationFactory
    {

        private IConfigurator configurator = null;

        public ConfigurationFactory(IConfigurator configurator)
        {
            this.configurator = configurator; 
        }

        public string GetConnectionString()
        {
            return this.configurator.GetConnectionString();
        }
    }
}
