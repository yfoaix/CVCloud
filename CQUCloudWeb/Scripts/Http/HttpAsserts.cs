using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQUCloudWeb.Scripts.Http
{
    public partial class HttpAsserts
    {
        private const string imageFileTypes = "jpg,jpeg,png,bmp";
        public static string GetImageFileErrorMsg(IFormCollection formCollection)
        {
            if (formCollection == null)
            {
                return "Form is empty";
            }
            FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
            if (fileCollection == null || fileCollection.Count != 1)
            {
                return "file count error:" + fileCollection.Count;
            }
            long size = 0;
            foreach (FormFile file in fileCollection)
            {
                string name = file.FileName;
                string[] contentType = file.ContentType.Split("/");
                if (contentType.Length <= 1)
                {
                    return "wrong Content-Type:" + file.ContentType;
                }
                else if (contentType[0] != "image" || Array.IndexOf(imageFileTypes.Split(","), contentType[1]) == -1)
                {
                    return "wrong type:" + file.ContentType + ",file type needs to be" + imageFileTypes;
                }
                size += file.Length;
                if (size > AppSettingUtility.MaxImageSize)
                {
                    return "the size of files exceed "+ AppSettingUtility.MaxImageSize/(1024*1024) + "MB";
                }
            }
            return null;
        }
        public static string GetVideoFileErrorMsg(IFormCollection formCollection)
        {
            if (formCollection == null)
            {
                return "Form is empty";
            }
            FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
            if (fileCollection == null || fileCollection.Count != 1)
            {
                return "file count error:" + fileCollection.Count;
            }
            long size = 0;
            foreach (FormFile file in fileCollection)
            {
                string name = file.FileName;
                if (file.ContentType == "video/mpeg4")
                {
                    return "wrong type:" + file.ContentType;
                }
                size += file.Length;
                if (size > AppSettingUtility.MaxVideoSize)
                {
                    return "the size of files exceed " + AppSettingUtility.MaxVideoSize / (1024 * 1024) + "MB";
                }
            }
            return null;
        }
        public static string GetNiftiFileErrorMsg(IFormCollection formCollection)
        {
            if (formCollection == null)
            {
                return "Form is empty";
            }
            FormFileCollection fileCollection = (FormFileCollection)formCollection.Files;
            if (fileCollection == null || fileCollection.Count != 1)
            {
                return "file count error:" + fileCollection.Count;
            }
            long size = 0;
            foreach (FormFile file in fileCollection)
            {
                string name = file.FileName;
                string[] extentions =  name.Split(".");
                if (extentions[extentions.Length-1].ToLower()!="nii")
                {
                    return "wrong type:" + file.ContentType;
                }
                size += file.Length;
                if (size > AppSettingUtility.MaxNIFTISize)
                {
                    return "the size of files exceed " + AppSettingUtility.MaxNIFTISize / (1024 * 1024) + "MB";
                }
            }
            return null;
        }
    }
}
