using System;
using System.Collections.Generic;
using System.Text;

namespace top.jieguone.influxdb.core
{
    /// <summary>
    /// 索引属性
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class TagAttribute : Attribute
    {
        /// <summary>
        /// 索引属性
        /// </summary>
        public TagAttribute() 
        {
        
        }
    }
}
