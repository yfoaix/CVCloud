using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Renci.SshNet;
using System.Collections;
using System.IO;
using System.Text;
using CQUCloudWeb.Scripts;
namespace CQUCloudWeb.Scripts
{
    
    public class SSHProcess
    {
        private static SSHProcess _processor = null;
        public static SSHProcess processor
        {
            get
            {
                if (_processor == null)
                {
                    _processor = new SSHProcess(AppSettingUtility.RootPath,
                        AppSettingUtility.Host,
                        AppSettingUtility.UserID,
                        AppSettingUtility.Password);
                }
                return _processor;
            }
        }
        public SSHProcess(string rootPath, string host, string user, string password)
        {
            this.host = host;
            this.user = user;
            this.rootPath = rootPath;
            this.password = password;
        }
        public SSHProcess(string rootPath, string host,int port, string user, string password)
        {
            this.host = host;
            this.user = user;
            this.rootPath = rootPath;
            this.password = password;
            this.port = port;
        }
        private readonly string host = "";
        private readonly string user = "";
        private readonly string rootPath = "";
        private readonly string password = "";
        private readonly int port = 22;
        /// <summary>
        /// 上传、运行、下载文件并以字符串形式读取
        /// </summary>
        /// <param name="uploadStream"></param>
        /// <param name="uploadPath"></param>
        /// <param name="interpreter"></param>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <param name="downloadPath"></param>
        /// <returns></returns>
        public ProcessResult UploadRunDownloadToStr(Stream uploadStream, string uploadPath, string interpreter, string path, string args, string downloadPath)
        {
            string fileName = path;
            string folder = "";
            if (path.IndexOf("/") != -1)
            {
                string[] fileNames = path.Split("/");
                fileName = fileNames[fileNames.Length - 1];
                folder = path.Remove(path.Length - fileName.Length, fileName.Length);
            }
            ProcessResult result = new ProcessResult();
            using (var sftpClient = new SftpClient(host, port, user, password))
            {
                sftpClient.Connect();
                sftpClient.UploadFile(uploadStream, rootPath + uploadPath);
                using (var sshClient = new SshClient(host, port, user, password))
                {
                    sshClient.Connect();
                    var cmd = sshClient.RunCommand("cd " + rootPath + folder + ";" + interpreter + " " + fileName + " " + args);
                    if (!string.IsNullOrWhiteSpace(cmd.Error))
                        result.debug = cmd.Error;
                    if (!string.IsNullOrWhiteSpace(cmd.Result))
                        result.output = cmd.Result;
                    result.output = Convert.ToBase64String(sftpClient.ReadAllBytes(rootPath + downloadPath));
                    sshClient.RunCommand("rm -rf " + rootPath + uploadPath);
                    sshClient.RunCommand("rm -rf " + rootPath + downloadPath);
                }
                

            }
            return result;
        }
        public ProcessResult UploadRun(Stream uploadStream, string uploadPath, string interpreter, string path, string args)
        {
            string fileName = path;
            string folder = "";
            if (path.IndexOf("/") != -1)
            {
                string[] fileNames = path.Split("/");
                fileName = fileNames[fileNames.Length - 1];
                folder = path.Remove(path.Length - fileName.Length, fileName.Length);
            }
            ProcessResult result = new ProcessResult();
            using (var sftpClient = new SftpClient(host,port, user, password))
            {
                sftpClient.Connect();
                sftpClient.UploadFile(uploadStream, rootPath + uploadPath);
            }
            using (var sshClient = new SshClient(host,port, user, password))
            {
                sshClient.Connect();
                var cmd = sshClient.RunCommand("cd " + rootPath + folder + ";" + interpreter + " " + fileName + " " + args);
                if (!string.IsNullOrWhiteSpace(cmd.Error))
                    result.debug = cmd.Error;
                if (!string.IsNullOrWhiteSpace(cmd.Result))
                    result.output = cmd.Result;
                sshClient.RunCommand("rm -rf " + rootPath + uploadPath);
                //result.debug = "cd " + rootPath + folder + ";" + interpreter + " " + fileName + " " + args;
            }
            
            return result;
        }
        public ProcessResult UploadRun(Stream uploadStream, string uploadPath,string path, string args)
        {
            string fileName = path;
            string folder = "";
            if (path.IndexOf("/") != -1)
            {
                string[] fileNames = path.Split("/");
                fileName = fileNames[fileNames.Length - 1];
                folder = path.Remove(path.Length - fileName.Length, fileName.Length);
            }
            ProcessResult result = new ProcessResult();
            using (var sftpClient = new SftpClient(host, port, user, password))
            {
                sftpClient.Connect();
                sftpClient.UploadFile(uploadStream, rootPath + uploadPath);
            }
            using (var sshClient = new SshClient(host, port, user, password))
            {
                sshClient.Connect();
                var cmd = sshClient.RunCommand("cd " + rootPath + folder + ";" + "./" + fileName + " " + args);
                if (!string.IsNullOrWhiteSpace(cmd.Error))
                    result.debug = cmd.Error;
                if (!string.IsNullOrWhiteSpace(cmd.Result))
                    result.output = cmd.Result;
                sshClient.RunCommand("rm -rf " + rootPath + uploadPath);

            }

            return result;
        }
        public ProcessResult Run(string interpreter, string path, string args)
        {
            string fileName = path;
            string folder = "";
            if (path.IndexOf("/") != -1)
            {
                string[] fileNames = path.Split("/");
                fileName = fileNames[fileNames.Length - 1];
                folder = path.Remove(path.Length - fileName.Length, fileName.Length);
            }
            ProcessResult result = new ProcessResult();
            using (var sshClient = new SshClient(host,port, user, password))
            {
                sshClient.Connect();
                var cmd = sshClient.RunCommand("cd " + rootPath + folder + ";" + interpreter + " " + fileName + " " + args);
                if (!string.IsNullOrWhiteSpace(cmd.Error))
                    result.debug = cmd.Error;
                if (!string.IsNullOrWhiteSpace(cmd.Result))
                    result.output = cmd.Result;
            }
            return result;
        }
        public void Delete(string path)
        {
        }

    }
}
