using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CQUCloudWeb.Models
{
    public enum DisplayType
    {
        [Display(Name ="无演示类型")]
        None,
        [Display(Name = "标记")]
        Plotting,
        [Display(Name = "显示属性")]
        Attribute,
        [Display(Name = "对比")]
        Comparing
    }
    public enum EnvironmentType
    {
        [Display(Name = "默认环境")]
        None,
        Opencv,
        Keras,
        Pytorch,
        Caffe
    }
    public class Algorithm
    {
        [Key]
        public string AlgorithmId { get; set; }


        [StringLength(20), Required, Display(Name = "算法名称")]
        public string AlgorithmName { get; set; }

        [StringLength(450), Required, Display(Name = "算法描述")]
        public string Description { get; set; }

        [Editable(false), StringLength(450), Display(Name = "用户ID")]
        public string UserNameID { get; set; }


        [Editable(false), DataType(DataType.Date), Display(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        [Editable(false), DataType(DataType.Date), Display(Name = "最后调用时间")]
        public DateTime LastCallTime { get; set; }

        [Editable(false), Display(Name = "本月调用次数")]
        public long MonthCallCount { get; set; }

        [Editable(false), Display(Name = "启用状态")]
        public bool State { get; set; }


        public ICollection<App> Apps { get; set; }


        [Display(Name = "展示类型")]
        public DisplayType DisplayType { get; set; }

        [Display(Name = "环境")]
        public EnvironmentType EnvironmentType { get; set; }

        [Display(Name = "执行文件")]
        public Byte[] CodeFile { get; set; }

        [Editable(false), Display(Name = "容器ID")]
        public string ContainerID { get; set; }
    }
}