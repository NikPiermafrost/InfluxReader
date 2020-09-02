using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfluxReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IInfluxDataAccessService _srv;
        public DataController(IInfluxDataAccessService srv)
        {
            _srv = srv;
        }

        [ProducesResponseType(typeof(List<StringModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("LoremIpsum")]
        public async Task<IActionResult> GetLorem()
        {
            try
            {
                var res = await _srv.GetStringEntries();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [ProducesResponseType(typeof(List<BoolModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("IsLorem")]
        public async Task<IActionResult> GetBools()
        {
            try
            {
                var res = await _srv.GetBoolEntries();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [ProducesResponseType(typeof(List<FloatModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("RandomFloat")]
        public async Task<IActionResult> GetFloats()
        {
            try
            {
                var res = await _srv.GetFloatEntries();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [ProducesResponseType(typeof(List<IntModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("RandomInt")]
        public async Task<IActionResult> GetInts()
        {
            try
            {
                var res = await _srv.GetIntEntries();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
