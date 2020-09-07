using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfluxReader.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitSender : ControllerBase
    {
        private readonly IRabbitSender _rabbitSender;
        public RabbitSender(IRabbitSender rabbitSender)
        {
            _rabbitSender = rabbitSender;
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public IActionResult Post([FromBody] string DataForRabbit)
        {
            try
            {
                _rabbitSender.SendReplayDataToRabbit(DataForRabbit);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
