using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CQUCloudWeb.Models;
using System.Diagnostics;

namespace CQUCloudWeb.Controllers
{
    public class AIController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FaceDetection()
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
        public IActionResult PedestrianDetection()
        {
            return View();
        }
        public IActionResult CarDetection()
        {
            return View();
        }
        public IActionResult ImageEnhance()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}