using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using System.Linq;

namespace top.jieguone.influxdb.core
{
    public class InfluxdbHelper<T> where T : InfluxdbTable
    {
        private readonly string _url = null;
        private readonly string _userName = null;
        private readonly string _passWD = null;
        private readonly string _dbName = null;
        public InfluxdbHelper(string url, string dbName, string userName, string passWD)
        {
            _url = url;
            _userName = userName;
            _passWD = passWD;
            _dbName = dbName;
        }

        public void CreateDB(string tablename)
        {
            //TODO q=CREATE DATABASE testdb
            var back = HttpHelper.HttpGetAsync($"{_url}/query?db={_dbName}&u={_userName}&p={_passWD}&q=CREATE DATABASE {tablename}").Result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql">select * from "TestTable" limit 2 offset 2</param>
        /// <returns></returns>
        public IList<T> Query(string sql)
        {
            var back = HttpHelper.HttpGetAsync($"{_url}/query?db={_dbName}&u={_userName}&p={_passWD}&q={sql}").Result;
            IList<string> columns = null;
            List<List<string>> vals = null;

            var respdata = JsonConvert.DeserializeObject<InfluxdbQueryResponse>(back);
            var tmpresults = respdata.results.FirstOrDefault();
            if (tmpresults != null)
            {
                var tmpdataseries = tmpresults.series.FirstOrDefault();
                if (tmpdataseries != null)
                {
                    columns = tmpdataseries.columns;
                    vals = tmpdataseries.values;
                }
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in vals)
            {
                sb.Append(",{");

                for (int i = 0; i < item.Count; i++)
                {
                    sb.Append($"\"{columns[i]}\":\"{item[i]}\"");
                    if (i < item.Count - 1)
                    {
                        sb.Append(",");
                    }
                }

                sb.Append("}");
            }
            var strjson = sb.ToString();
            strjson = "[" + strjson.Substring(1) + "]";

            IList<T> ts = JsonConvert.DeserializeObject<List<T>>(strjson);

            return ts;
        }

        public void Write(T tables)
        {
            string tagstr = "";
            string field = "";

            Type t = typeof(T);
            var tableName = t.Name;

            foreach (PropertyInfo pi in t.GetProperties())
            {
                if (pi.Name == "time")
                {
                    continue;
                }

                var isttager = false;
                foreach (var item in pi.CustomAttributes)
                {
                    var tag = item.ToString();
                    if (tag.Contains("top.jieguone.influxdb.core.TagAttribute"))
                    {
                        isttager = true;
                    }
                }
                if (isttager)
                {
                    tagstr += $",{pi.Name}={pi.GetValue(tables).ToString()}";
                }
                else
                {
                    field += $",{pi.Name}={pi.GetValue(tables).ToString()}";
                }
            }

            tagstr = tagstr.Substring(1);
            field = field.Substring(1);

            var back = HttpHelper.HttpPostAsync($"{_url}/write?db={_dbName}&u={_userName}&p={_passWD}", $"{tableName},{tagstr} {field}", "application/x-www-form-urlencoded", 5, null).Result;

            if (!string.IsNullOrEmpty(back))
            {
                throw new Exception(back);
            }
        }
    }
}
