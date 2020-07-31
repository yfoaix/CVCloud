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
    public class ImageEnhanceController : ControllerBase
    {
        private const string folderPath = "AIAPI/ImageEnhance/";
        private const string imagePath = "Image/";
        private const string outputPath = "Output/";
        private const string processPath = "ImageEnhancer.py";
        private const string pythonPath = "/home/geyongxin/anaconda3/envs/cloudweb/bin/python";
        // GET: api/PedestrainDetection/5
        [HttpGet("{name}", Name = "ImageEnhanceGet")]
        public string Get(string name)
        {
            string path = AppSettingUtility.PreDataPath + "ImageEnhance/" + name + ".txt";
            return HttpGet.GetPreData(path);
        }

        // POST: api/PedestrainDetection
        [HttpPost("{id}", Name = "ImageEnhancePost")]
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
                hashtable["base64"] = result.output;
                hashtable["used_ticks"] = DateTime.Now.Ticks - now;
                return JsonConvert.SerializeObject(hashtable);
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
            string downLoadPath = folderPath + outputPath + imageName;
            return SSHProcess.processor.UploadRunDownloadToStr(uploadStream, uploadPath, pythonPath, folderPath + processPath, "--path " + imagePath + imageName + " --output " + outputPath + imageName, downLoadPath);
        }
    }
}
