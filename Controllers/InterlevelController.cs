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
        [Produces("application/json")]
        public async Task<IActionResult> Post([FromBody] TagEndPointModel tag){                                                
            if(await interlevelDb.Write(tag.value, tag.address, tag.workstation))
                return Ok();
            return BadRequest();    
        }

        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Get(string tag){   
            Console.WriteLine();
            Console.WriteLine("Tag = " + tag);                                             
            Console.WriteLine();
            var value= new{
                value = await interlevelDb.Read(tag)
            };
            return Ok(value);                            
        }
    }
}