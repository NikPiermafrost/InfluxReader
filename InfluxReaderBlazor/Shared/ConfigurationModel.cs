using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfluxReaderBlazor.Shared
{
    public class ConfigurationModel
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
        public string Hostname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Vhost { get; set; }
        public int Port { get; set; }
    }
}
