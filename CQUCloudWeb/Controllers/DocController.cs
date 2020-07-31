using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CQUCloudWeb.Controllers
{
    public class DocController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SDK()
        {
            return View();
        }
        public IActionResult API()
        {
            return View();
        }
        public IActionResult CarDetection()
        {
            return View();
        }
        public IActionResult FaceDetection()
        {
            return View();
        }
        public IActionResult PedestrianDetection()
        {
            return View();
        }
        public IActionResult AttributePrediction()
        {
            return View();
        }
        public IActionResult ImageSegmentation()
        {
            return View();
        }
        public IActionResult ImageEnhance()
        {
            return View();
        }
    }
}