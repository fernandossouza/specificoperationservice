using specificoperationservice.Model.SpecificPhase;
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
    public class InterlevelController : Controller
    {
        private readonly IInterlevelDb interlevelDb;
        public InterlevelController(IInterlevelDb interlevelDb){
            this.interlevelDb = interlevelDb;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TagEndPointModel tag)
        {            
            var specificParameterCreated = await interlevelDb.Write(tag.value, tag.address, tag.workstation);


            return Created($"api/specificPhase/{specificParameter.thingGroupId}", specificParameter);

        }

    }
}