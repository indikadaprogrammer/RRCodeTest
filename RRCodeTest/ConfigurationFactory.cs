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

        /// <summary>
        /// Instantiate a ConfigurationFactory object which can be used 
        /// to configure this library.
        /// </summary>
        /// <param name="configurator">Configurator object that can be used to get relevant information to configure this library</param>
        public ConfigurationFactory(IConfigurator configurator)
        {
            this.configurator = configurator; 
        }

        /// <summary>
        /// Returns the connection string that should be used to access databases 
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            return this.configurator.GetConnectionString();
        }
    }
}
