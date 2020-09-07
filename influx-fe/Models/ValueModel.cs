using System;
using System.Collections.Generic;
using System.Text;

namespace influx_fe.Models
{
    public class ValueModel
    {
        public string EntityName { get; set; }
        public List<FloatModel> Values { get; set; }
    }
}
