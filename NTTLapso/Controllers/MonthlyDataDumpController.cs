using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.DataDump;
using NTTLapso.Service;
using NTTLapso.Tools;

namespace NTTLapso.Controllers
{
    [ApiController]
    public class MonthlyDataDumpController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<CategoryController> _logger;
        private MonthlyDataDumpService _service;
        public MonthlyDataDumpController(ILogger<CategoryController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new MonthlyDataDumpService(_config);
        }

        [Route("NTTLapso/GetConsolidationData")]
        [HttpGet]
        public async Task<ActionResult> GetConsolidatedEmployees()
        {
            ConsolidationResponse resp = await _service.GetConsolidatedEmployees();
            return resp.Completed ? Ok(resp) : StatusCode(500, resp);
        }

        [Route("NTTLapso/DataDump")]
        [HttpPost]
        public async Task<ActionResult> DataDump()
        {
            DataDumpResponse excelLoadResponse = await _service.LoadDataFromExcels();

            if (!excelLoadResponse.Completed) return StatusCode(500, excelLoadResponse);

            DataDumpResponse consolidationResp = await _service.CreateConsolidation();
            consolidationResp.Log = excelLoadResponse.Log + consolidationResp.Log;

            return (consolidationResp.Completed) ? Ok(consolidationResp) : StatusCode(500, consolidationResp);
        }

        [Route("NTTLapso/MonthlyDataDump")]
        [HttpPost]
        public async Task<ActionResult> MonthlyDataDump()
        {
            CalculateHoursResponse calcResp = await _service.CalculateMonthlyHours();
                
            if(!calcResp.Completed) return StatusCode(500, calcResp);
                  
            DataDumpResponse excelLoadResponse = await _service.LoadDataFromExcels();
            excelLoadResponse.Log = calcResp.Log + excelLoadResponse.Log;

            if(!excelLoadResponse.Completed) return StatusCode(500, excelLoadResponse);

            DataDumpResponse consolidationResp = await _service.CreateConsolidation();
            consolidationResp.Log = excelLoadResponse.Log + consolidationResp.Log;

            return (consolidationResp.Completed) ? Ok(consolidationResp) : StatusCode(500, consolidationResp);
        }
    }
}
