using System;
using System.Collections.Generic;
using System.Text;

namespace top.jieguone.influxdb.core
{
    public class InfluxdbTable
    {
        [Tag]
        public string Address { get; set; }
        public int Value { get; set; }

        public DateTime time { get; set; }
    }
}
