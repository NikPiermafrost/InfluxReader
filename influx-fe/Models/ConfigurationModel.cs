using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace influx_fe.Models
{
    public class ConfigurationModel
    {
        public string Exchange { get; set; }
        public string RoutingKey { get; set; }
        public string Queue { get; set; }
        public string Hostname { get; set; }
    }
}
