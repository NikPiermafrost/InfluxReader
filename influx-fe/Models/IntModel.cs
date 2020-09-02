using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace influx_fe.Models
{
    public class IntModel
    {
        public DateTime Time { get; set; }
        public int Value { get; set; }
    }
}
