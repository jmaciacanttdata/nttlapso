using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.DataDump;
using NTTLapso.Service;
using NTTLapso.Tools;

namespace NTTLapso.Controllers
{
    [ApiController]
    [Route("NTTLapso/MonthlyDataDump")]
    public class MonthlyDataDumpController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<MonthlyDataDumpController> _logger;
        private MonthlyDataDumpService _service;
        public MonthlyDataDumpController(ILogger<MonthlyDataDumpController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
            _service = new MonthlyDataDumpService(_config);
        }

        [Route("CreateLeaderIncurredHours")]
        [HttpPost]
        public async Task<ActionResult> CreateLeaderIncurredHours()
        {
            var resp = await _service.CreateLeaderRemainingHours();
            return (resp.Completed) ? Ok(resp) : StatusCode(500, resp);
        }
 
        [Route("GetLeaderIncurredHours")]
        [HttpGet]
        public async Task<ActionResult> GetLeaderRemainingHours(string? leaderId, string? employeeId, string? service)
        {
            var resp = await _service.GetLeaderRemainingHours(leaderId, employeeId, service);
            return StatusCode(resp.StatusCode, resp);
        }

        [Route("GetRemainingIncurredHours")]
        [HttpGet]
        public async Task<ActionResult> GetRemainingIncurredHours(string? userId)
        {
            string year = ((DateTime.Now.Month == 1) ? DateTime.Now.Year - 1 : DateTime.Now.Year).ToString();
            string month = ((DateTime.Now.Month == 1) ? 12 : DateTime.Now.Month - 1).ToString();

            var resp = await _service.GetEmployeeRemainingHours(month, year, userId);
            return StatusCode(resp.StatusCode, resp);
        }

        [Route("GetTotalIncurredHours")]
        [HttpGet]
        public async Task<ActionResult> GetTotalIncurredHours(string? userId)
        {
            string year = ((DateTime.Now.Month == 1) ? DateTime.Now.Year - 1 : DateTime.Now.Year).ToString();
            string month = ((DateTime.Now.Month == 1) ? 12 : DateTime.Now.Month - 1).ToString();

            var resp = await _service.GetTotalIncurredHours(month, year, userId);
            return StatusCode(resp.StatusCode, resp);
        }

        [Route("GetIncurredHours")]
        [HttpGet]
        public async Task<ActionResult> GetIncurredHours(string? userId)
        {
            string year = ((DateTime.Now.Month == 1) ? DateTime.Now.Year - 1 : DateTime.Now.Year).ToString();
            string month = ((DateTime.Now.Month == 1) ? 12 : DateTime.Now.Month - 1).ToString();

            var resp = await _service.GetIncurredHours(month, year, userId);
            return StatusCode(resp.StatusCode, resp);
        }

        [Route("GetConsolidationData")]
        [HttpGet]
        public async Task<ActionResult> GetConsolidatedEmployees()
        {
            ConsolidationResponse resp = await _service.GetConsolidatedEmployees();
            return resp.Completed ? Ok(resp) : StatusCode(500, resp);
        }

        [Route("CalculateMonthlyIncurredHours")]
        [HttpPost]
        public async Task<ActionResult> CalculateMonthlyIncurredHours()
        {
            CalculateHoursResponse calcResp = await _service.CalculateMonthlyHours();
            return (calcResp.Completed) ? Ok(calcResp) : StatusCode(500, calcResp);
        }

        [Route("DataDump")]
        [HttpPost]
        public async Task<ActionResult> DataDump()
        {
            DataDumpResponse excelLoadResponse = await _service.LoadDataFromExcels();

            if (!excelLoadResponse.Completed) return StatusCode(500, excelLoadResponse);

            DataDumpResponse consolidationResp = await _service.CreateConsolidation();
            consolidationResp.Log = excelLoadResponse.Log + consolidationResp.Log;

            if (!consolidationResp.Completed) return StatusCode(500, consolidationResp);

            DataDumpResponse leaderRemainingHoursResp = await _service.CreateLeaderRemainingHours();
            leaderRemainingHoursResp.Log = consolidationResp.Log + leaderRemainingHoursResp.Log;
            leaderRemainingHoursResp.NumConsolidate = consolidationResp.NumConsolidate;

            return (leaderRemainingHoursResp.Completed) ? Ok(leaderRemainingHoursResp) : StatusCode(500, leaderRemainingHoursResp);
        }

        [Route("MonthlyDataDump")]
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

            if (!consolidationResp.Completed) return StatusCode(500, consolidationResp);

            DataDumpResponse leaderRemainingHoursResp = await _service.CreateLeaderRemainingHours();
            leaderRemainingHoursResp.Log = consolidationResp.Log + leaderRemainingHoursResp.Log;
            leaderRemainingHoursResp.NumConsolidate = consolidationResp.NumConsolidate;

            return (leaderRemainingHoursResp.Completed) ? Ok(leaderRemainingHoursResp) : StatusCode(500, leaderRemainingHoursResp);
        }
    }
}
