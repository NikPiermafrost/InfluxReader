using System;
using System.Collections.Generic;
using System.Text;

namespace InfluxReaderBlazor.Shared
{
    public class ValueModel
    {
        public string EntityName { get; set; }
        public List<FloatModel> Values { get; set; }
    }
}
