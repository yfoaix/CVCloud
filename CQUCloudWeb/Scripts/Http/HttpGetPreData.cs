using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUCloudWeb.Scripts.Http
{
    public class HttpGet
    {
        public static string GetPreData(string path)
        {
            long now = DateTime.Now.Ticks;
            Hashtable hashtable = new Hashtable();
            //一个目录里可以有同一名称、不同后缀的文件
            if (!System.IO.File.Exists(path))
            {
                hashtable["used_ticks"] = DateTime.Now.Ticks - now;
                hashtable["error_msg"] = "wrong path:" + path;
                return JsonConvert.SerializeObject(hashtable);
            }
            string responseJson = System.IO.File.ReadAllText(path).Replace("'", "\"");///需完善异常处理
            string usedTicks = "\"used_ticks\":" + (DateTime.Now.Ticks - now).ToString() + ",";
            return responseJson.Insert(1, usedTicks);
        }
    }
}
