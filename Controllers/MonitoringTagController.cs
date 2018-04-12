using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using specificoperationservice.Model;
using specificoperationservice.Service.Interface;

namespace specificoperationservice.Controllers
{
    [Route("api/[controller]")]
    public class MonitoringTagController : Controller
    {
        private readonly IMonitoringTag _monitoringTagService;

        public MonitoringTagController(IMonitoringTag monitoringTagService)
        {
            _monitoringTagService = monitoringTagService;
        }

        [HttpGet]
        public async Task<IActionResult> Post()
        {
            var (returnBool,returnString) = await _monitoringTagService.ReadTags();
            return Ok();

        }
        
    }
}