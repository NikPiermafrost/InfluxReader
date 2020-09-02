﻿using AdysTech.InfluxDB.Client.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class StringModel
    {

        [InfluxDBTime]
        public DateTime Time { get; set; }
        [InfluxDBField(nameof(Value))]
        public string Value { get; set; }
    }
}
