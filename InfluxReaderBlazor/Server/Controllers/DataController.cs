﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataAccess;
using InfluxReaderBlazor.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InfluxReaderBlazor.Server
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

        [HttpGet]
        [ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var res = await _srv.GetDatabaseTables();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
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
                    result.Add(await _srv.GetEntries(new DateTime(qrs.StartDate), new DateTime(qrs.EndDate), entity));
                }
                var trimmed = _srv.TrimInconsistentData(result);
                return Ok(trimmed);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
