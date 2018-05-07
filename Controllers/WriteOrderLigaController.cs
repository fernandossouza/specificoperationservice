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
    public class WriteOrderLigaController : Controller
    {
        private readonly IWriteLigaPlc _writeLigaPlc;
        public WriteOrderLigaController(IWriteLigaPlc writeLigaPlc)
        {
            _writeLigaPlc = writeLigaPlc;
        }

        [HttpPost("start")]
        public async Task<IActionResult> PostStart(int posicaoForno)
        {
            if(posicaoForno<=0)
                return NotFound();

            var (result,stringErro) = await _writeLigaPlc.IniciaForno(posicaoForno);

            if(result)
                return Ok();

            return StatusCode(500, stringErro);
        } 

        [HttpPost("finalize")]
        public async Task<IActionResult> PostFinalize(int posicaoForno)
        {
            if(posicaoForno<=0)
                return NotFound();

            var (result,stringErro) = await _writeLigaPlc.FinalizaForno(posicaoForno);

            if(result)
                return Ok();

            return StatusCode(500, stringErro);
        } 

        [HttpPost("enable")]
        public async Task<IActionResult> PostEnable(int posicaoForno)
        {
            if(posicaoForno<=0)
                return NotFound();

            var (result,stringErro) = await _writeLigaPlc.HabilitaForno(posicaoForno);

            if(result)
                return Ok();

            return StatusCode(500, stringErro);
        } 
    }
}