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
    public class SpecificPhaseController : Controller
    {
        private readonly ISpecificPhaseService _specificPhaseService;
        public SpecificPhaseController(ISpecificPhaseService specificPhaseService)
        {
            _specificPhaseService = specificPhaseService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]SpecificParameter specificParameter)
        {
            var specificParameterCreated = await _specificPhaseService.AddParameter(specificParameter);

            if(specificParameter == null)
            {
                return StatusCode(500,string.Empty); 
            }

            return Created($"api/specificPhase/{specificParameter.thingGroupId}", specificParameter);

        }

         [HttpGet("{phaseId}")]
        public async Task<IActionResult> Get(int phaseId)
        {
            var specificPhase = await _specificPhaseService.GetSpecificPhase(phaseId);

            if(specificPhase == null)
            {
                return NotFound(); 
            }

            return Ok(specificPhase);

        }
    }
}