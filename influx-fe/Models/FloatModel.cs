using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace influx_fe.Models
{
    public class FloatModel
    {
        public DateTime Time { get; set; }
        public float Value { get; set; }
    }
}
