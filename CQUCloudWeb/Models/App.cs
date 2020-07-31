
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CQUCloudWeb.Models
{
    public enum AIFunction
    {
        [Display(Name = "车辆检测")]
        CarDetection,
        [Display(Name = "人脸检测")]
        FaceDetection,
        [Display(Name = "动物属性识别")]
        AttributePrediction,
        [Display(Name = "图像增强")]
        ImageEnhance,
        [Display(Name = "行人检测")]
        PredestrianDetection,
        [Display(Name = "")]
        Custom
    }
    public class App
    {
        [Key]
        public string AppId { get; set; }


        [StringLength(20), Required, Display(Name = "应用名称")]
        public string AppName { get; set; }


        [Editable(false), StringLength(450), Display(Name = "用户ID")]
        public string UserNameID { get; set; }

        [ForeignKey("Algorithm"), Display(Name = "API ID")]
        public string AlgorithmRefId { get; set; }

        public Algorithm Algorithm { get; set; }

        [Editable(false), Display(Name = "API Key")]
        public string APIKey { get; set; }

        [Editable(false), DataType(DataType.Date), Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [Editable(false), DataType(DataType.Date), Display(Name = "最后调用时间")]
        public DateTime LastCallTime { get; set; }

        [Editable(false), Display(Name = "本月调用次数")]
        public long MonthCallCount { get; set; }

        [Editable(false), Display(Name = "启用状态")]
        public bool State { get; set; }

        [Display(Name = "调用功能")]
        public AIFunction AIFunction { get; set; }
        public string AlgorithmName
        {
            get
            {
                switch (this.AIFunction)
                {
                    case AIFunction.CarDetection:
                        return "车辆检测";
                    case AIFunction.FaceDetection:
                        return "人脸检测";
                    case AIFunction.AttributePrediction:
                        return "动物属性预测";
                    case AIFunction.ImageEnhance:
                        return "图像增强";
                    case AIFunction.PredestrianDetection:
                        return "行人检测";
                    default:
                        if (this.Algorithm != null)
                        {
                            return this.Algorithm.AlgorithmName;
                        }
                        else
                        {
                            return "自定义";
                        }
                }
            }
        }
        public string CallLink
        {
            get
            {
                switch (this.AIFunction)
                {
                    case AIFunction.CarDetection:
                        return "http://cqucloud.free.qydev.com/api/CarDetection/" + APIKey;
                    case AIFunction.FaceDetection:
                        return "http://cqucloud.free.qydev.com/api/FaceDetection/" + APIKey;
                    case AIFunction.AttributePrediction:
                        return "http://cqucloud.free.qydev.com/api/AttributePrediction/" + APIKey;
                    case AIFunction.ImageEnhance:
                        return "http://cqucloud.free.qydev.com/api/ImageEnhance/" + APIKey;
                    case AIFunction.PredestrianDetection:
                        return "http://cqucloud.free.qydev.com/api/PredestrianDetection/" + APIKey;
                    default:
                        if (this.Algorithm != null)
                        {
                            return "http://cqucloud.free.qydev.com/api/MicroService/" + this.Algorithm.AlgorithmId+"/"+this.APIKey;
                        }
                        else
                        {
                            return "无";
                        }
                }
            }

        }
    }
}
