using System;
using System.Collections.Generic;
using System.Text;

namespace top.jieguone.influxdb.core
{
    public class SeriesItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> columns { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<List<string>> values { get; set; }
    }

    public class ResultsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int statement_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SeriesItem> series { get; set; }
    }

    public class InfluxdbQueryResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ResultsItem> results { get; set; }
    }
}
