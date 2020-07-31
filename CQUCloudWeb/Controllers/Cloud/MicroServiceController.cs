using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Internal;
using System.Collections;
using Newtonsoft.Json;
using CQUCloudWeb.Scripts;
using Microsoft.Extensions.Options;
namespace CQUCloudWeb.Controllers.Cloud
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicroServiceController : ControllerBase
    {

        // POST:api/MicroService/5
        [HttpPost("{id}", Name = "MicroServicePost")]
        public string Post(string id, [FromForm] IFormCollection formCollection)
        {
            return "Not allowed";
        }
    }
}
