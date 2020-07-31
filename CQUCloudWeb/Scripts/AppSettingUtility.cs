using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQUCloudWeb.Models;
using Microsoft.Extensions.Configuration;

namespace CQUCloudWeb.Scripts
{
    
    
    public class AppSettingUtility
    {
        public static string PreDataPath;
        public static string RootPath;
        //
        public static string Host;
        public static string UserID;
        public static string Password;
        //
        public static int MaxImageSize;
        public static int MaxNIFTISize;
        public static int MaxVideoSize;
        public static int MaxCodeSize;
        //
        public static int MaxAccountAlgorithmNum;
        public static int MaxAccountAppNum;
        public static int MaxAlgorithmNum;
        //
        
        public static void Initial(IConfiguration configuration)
        {
            PreDataPath = configuration["CloudWebSetting:PreDataPath"];
            LoadSSHConfig(configuration["CloudWebSetting:SSHConnection"]);
            RootPath = configuration["CloudWebSetting:RootPath"];
            MaxImageSize = int.Parse(configuration["CloudWebSetting:MaxImageSize"]);//缺少异常处理
            MaxNIFTISize = int.Parse(configuration["CloudWebSetting:MaxNIFTISize"]);
            MaxVideoSize = int.Parse(configuration["CloudWebSetting:MaxVideoSize"]);
            MaxCodeSize = int.Parse(configuration["CloudWebSetting:MaxCodeSize"]);
            MaxAccountAlgorithmNum = int.Parse(configuration["CloudWebSetting:MaxAccountAlgorithmNum"]);
            MaxAccountAppNum = int.Parse(configuration["CloudWebSetting:MaxAccountAppNum"]);
            MaxAlgorithmNum = int.Parse(configuration["CloudWebSetting:MaxAlgorithmNum"]);
        }
        private static void LoadSSHConfig(string str)
        {
            string[] pairs = str.Split(";");
            Host = pairs[0].Split("=")[1];
            UserID = pairs[1].Split("=")[1];
            Password = pairs[2].Split("=")[1];
        }
    }
}
