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
    public class WriteOrderController : Controller
    {
        private readonly IWritePlc _writePlc;
        public WriteOrderController(IWritePlc writePlc)
        {
            _writePlc = writePlc;
        }


        [HttpPost]
        public async Task<IActionResult> GetList([FromBody]ProductionOrder productionOrder)
        {
            try{
            

            var (returnWrite, stringErro) = await _writePlc.WriteOrder(productionOrder);

            if(!returnWrite)
            {
                return StatusCode(500, stringErro);
            }

            return Ok(returnWrite);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
    }
}