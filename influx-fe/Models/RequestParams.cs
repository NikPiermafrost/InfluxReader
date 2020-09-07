using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace influx_fe.Models
{
    public class RequestParams
    {
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public List<string> Entities { get; set; }
    }
}
