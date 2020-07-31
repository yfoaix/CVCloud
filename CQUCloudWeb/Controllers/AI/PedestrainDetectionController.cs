using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CQUCloudWeb.Scripts;
using CQUCloudWeb.Scripts.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CQUCloudWeb.Controllers.AI
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedestrianDetectionController : ControllerBase
    {
        private const string folderPath = "AIAPI/PedestrianDetection/";
        private const string imagePath = "Image/";
        private const string processPath = "hog.py";
        private const string pythonPath = "/home/geyongxin/anaconda3/envs/cloudweb/bin/python";
        // GET: api/PedestrainDetection/5
        [HttpGet("{name}", Name = "PedestrianDetectionGet")]
        public string Get(string name)
        {
            string path = AppSettingUtility.PreDataPath + "PedestrianDetection/" + name + ".txt";
            return HttpGet.GetPreData(path);
        }

        // POST: api/PedestrainDetection
        [HttpPost("{id}", Name = "PedestrianDetectionPost")]
        public string Post(string id, [FromForm] IFormCollection formCollection)
        {
            long now = DateTime.Now.Ticks;
            Hashtable hashtable = new Hashtable();
            string fileErrorMsg = HttpAsserts.GetImageFileErrorMsg(formCollection);
            if (fileErrorMsg != null)
            {
                hashtable["used_ticks"] = DateTime.Now.Ticks - now;
                hashtable["error_msg"] = fileErrorMsg;
                return JsonConvert.SerializeObject(hashtable);
            }
            FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
            ProcessResult result = null;
            foreach (FormFile file in fileCollection)
            {
                string imageName = file.FileName;
                using (Stream uploadStream = file.OpenReadStream())
                {
                    try
                    {
                        result = RunProcess(uploadStream, imageName);
                    }
                    catch (Exception e)
                    {
                        hashtable["used_ticks"] = DateTime.Now.Ticks - now;
                        hashtable["error_msg"] = e.ToString();
                        return JsonConvert.SerializeObject(hashtable);
                    }
                }
            }
            if (!string.IsNullOrEmpty(result.output))
            {
                string usedTicks = "\"used_ticks\":" + (DateTime.Now.Ticks - now).ToString() + ",";
                string output = result.output;
                return output.Replace("'", "\"").Insert(1, usedTicks);
            }
            else
            {
                hashtable["used_ticks"] = DateTime.Now.Ticks - now;
                hashtable["error_msg"] = result.debug;
                return JsonConvert.SerializeObject(hashtable);
            }

        }
        private ProcessResult RunProcess(Stream uploadStream, string imageName)
        {
            imageName = RandomStringBuilder.ChangeNameToRandomStr(imageName);
            string uploadPath = folderPath + imagePath + imageName;
            return SSHProcess.processor.UploadRun(uploadStream, uploadPath, pythonPath, folderPath + processPath, "--path " + imagePath + imageName);
        }
    }
}
