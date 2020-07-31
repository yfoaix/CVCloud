using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public class CarDetectionController : ControllerBase
    {
        private const string folderPath = "AIAPI/CarDetection/";
        private const string videoPath = "Video/";
        private const string processPath = "CarDetection";
        private const int maxSize = 50 * 1024 * 1024;//50mb
        // GET: api/CarDetection/1
        [HttpGet("{name}", Name = "CarDetectionGet")]
        public string Get(string name)
        {
            string path = AppSettingUtility.PreDataPath + "CarDetection/" + name + ".txt";
            return HttpGet.GetPreData(path);
        }

        // POST: api/CarDetection
        [HttpPost("{id}", Name = "CarDetectionPost")]
        public string Post(string id, [FromForm] IFormCollection formCollection)
        {
            long now = DateTime.Now.Ticks;

            Hashtable hashtable = new Hashtable();
            string fileErrorMsg = HttpAsserts.GetVideoFileErrorMsg(formCollection);
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
                string videoName = file.FileName;
                using (Stream uploadStream = file.OpenReadStream())
                {
                    try
                    {
                        result = RunProcess(uploadStream, videoName);
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
                string output = FormatOutput(result);
                return output.Insert(1, usedTicks);
            }
            else
            {
                hashtable["used_ticks"] = DateTime.Now.Ticks - now;
                hashtable["error_msg"] = result.debug;
                return JsonConvert.SerializeObject(hashtable);
            }
        }
        private string FormatOutput(ProcessResult processResult)
        {
            StringReader stringReader = new StringReader(processResult.output);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{\"cars\":{");
            string line;
            bool sign = false;
            int lastFrame = -1;
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line.IndexOf("[INFO]")!=-1)
                {
                    continue;
                }
                string[] percent = Regex.Split(line, "% ");
                int frame = int.Parse(Regex.Split(percent[0], " Car:")[0]);
                float rate = float.Parse(Regex.Split(percent[0], " Car:")[1]);
                string rect = percent[1].Replace("(", "");
                rect = rect.Replace(")", "");
                string[] pointstr = Regex.Split(rect, ", ");
                int[] points = new int[4];
                points[0] = int.Parse(pointstr[0]);
                points[1] = int.Parse(pointstr[1]);
                points[2] = int.Parse(pointstr[2]);
                points[3] = int.Parse(pointstr[3]);

                if (sign)
                {
                    if (frame != lastFrame)
                    {
                        stringBuilder.Append("]");
                    }
                    stringBuilder.Append(",");
                }
                else
                {
                    sign = true;
                }
                if (frame == lastFrame)
                {
                    stringBuilder.Append("{");
                }
                else
                {
                    stringBuilder.Append("\"");
                    stringBuilder.Append(frame);
                    stringBuilder.Append("\":[{");
                }
                stringBuilder.Append("\"confidence\":");
                stringBuilder.Append(rate);
                stringBuilder.Append(",");
                stringBuilder.Append("\"rect\":[");
                stringBuilder.Append(points[0]);
                stringBuilder.Append(",");
                stringBuilder.Append(points[1]);
                stringBuilder.Append(",");
                stringBuilder.Append(points[2]);
                stringBuilder.Append(",");
                stringBuilder.Append(points[3]);
                stringBuilder.Append("]}");
                lastFrame = frame;
            }
            stringBuilder.Append("]}}");
            stringReader.Close();
            return stringBuilder.ToString();
        }
        private ProcessResult RunProcess(Stream uploadStream, string videoName)
        {
            videoName = RandomStringBuilder.ChangeNameToRandomStr(videoName);
            string uploadPath = folderPath + videoPath + videoName;
            return SSHProcess.processor.UploadRun(uploadStream, uploadPath, folderPath + processPath, "--video " + videoPath + videoName);
        }
    }
}
