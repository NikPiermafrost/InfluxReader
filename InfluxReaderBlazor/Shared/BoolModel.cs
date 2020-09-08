using AdysTech.InfluxDB.Client.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxReaderBlazor.Shared
{
    public class BoolModel
    {
        [InfluxDBTime]
        public DateTime Time { get; set; }
        [InfluxDBField(nameof(Value))]
        public bool Value { get; set; }
    }
}
