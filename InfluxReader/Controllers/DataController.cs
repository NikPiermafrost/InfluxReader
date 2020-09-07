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

        [ProducesResponseType(typeof(List<ValueModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RequestParams qrs)
        {
            try
            {
                var result = new List<ValueModel>();
                foreach (var entity in qrs.Entities)
                {
                    result.Add(await _srv.SelectDataReturn(entity, new DateTime(qrs.StartDate), new DateTime(qrs.EndDate)));
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
