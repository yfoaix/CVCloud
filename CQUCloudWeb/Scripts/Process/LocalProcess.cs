using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
namespace CQUCloudWeb.Scripts
{
    public class LocalProcess
    {
        public static int waitTime = -1;
        public static ProcessResult Run(string path, string arg)
        {
            ProcessResult result = new ProcessResult();
            try
            {
                using (Process process = new Process())
                {
                    ProcessStartInfo startInfo = process.StartInfo;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = path;
                    startInfo.CreateNoWindow = true;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.Arguments = arg;
                    process.Start();
                    result.output = process.StandardOutput.ReadToEnd();
                    result.debug = process.StandardError.ReadToEnd();
                    result.returnNum = process.ExitCode;
                    if (waitTime < 0)
                    {
                        process.WaitForExit();
                    }
                    else if (waitTime > 0)
                    {
                        process.WaitForExit(waitTime);
                    }
                }
            }
            catch (Exception e)
            {
                result.debug += e.ToString();
                result.returnNum = -1;
            }
            return result;
        }
        //async static string AsyncRun(string path, string arg, Action() , Action() )
        //{

        //}
    }
}
