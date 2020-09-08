using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxReaderBlazor.Shared
{
    public class SimulationtoRabbitValues
    {
        public long Dateref { get; set; }
        public long TimeStamp { get; set; }
        public List<string> Entities { get; set; }
        public string SimId { get; set; }
    }
}
