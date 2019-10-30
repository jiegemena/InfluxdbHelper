using Microsoft.VisualStudio.TestTools.UnitTesting;
using top.jieguone.influxdb.core;
using System;
using System.Collections.Generic;
using System.Text;

namespace top.jieguone.influxdb.core.Tests
{
    [TestClass()]
    public class InfluxdbHelperTests
    {
        [TestMethod()]
        public void WriteTest()
        {
            InfluxdbHelper<TestTable> influxdbHelper = new InfluxdbHelper<TestTable>("http://192.168.1.3:8086", "demo", "admin", "admin123");
            influxdbHelper.Write(new TestTable { 
                Address = "IOS",
                Name = "jiege",
                State = 12,
                Value = 0
            });
        }

        [TestMethod()]
        public void QueryTest()
        {
            string sql = "select * from \"TestTable\" limit 2 offset 2";
            InfluxdbHelper<TestTable> influxdbHelper = new InfluxdbHelper<TestTable>("http://192.168.1.3:8086", "demo", "admin", "admin123");
            influxdbHelper.Query(sql);
        }
    }

    public class TestTable : InfluxdbTable 
    {
        [Tag]
        public string Name { get; set; }
        public int State { get; set; }
    }
}