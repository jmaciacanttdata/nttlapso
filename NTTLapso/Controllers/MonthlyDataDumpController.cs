using Microsoft.AspNetCore.Authorization;
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

        [Route("CreateLeaderRemainingHours")]
        [HttpPost]
        public async Task<ActionResult> CreateLeaderRemaininghHours()
        {
            var resp = await _service.CreateLeaderRemainingHours();
            return (resp.Completed) ? Ok(resp) : StatusCode(resp.StatusCode, resp);
        }

        [Route("CalculateMonthlyIncurredHours")]
        [HttpPost]
        public async Task<ActionResult> CalculateMonthlyIncurredHours()
        {
            var calcResp = await _service.CalculateMonthlyHours();
            return (calcResp.Completed) ? Ok(calcResp) : StatusCode(calcResp.StatusCode, calcResp);
        }

        [Route("DataDump")]
        [HttpPost]
        public async Task<ActionResult> DataDump()
        {
            var finalResp = new NumConsolidationResponse();

            // LEER DATOS DE LOS EXCELS
            var excelLoadResponse = await _service.LoadDataFromExcels();
            if (!excelLoadResponse.Completed) return StatusCode(excelLoadResponse.StatusCode, excelLoadResponse);

            finalResp.Log.Append(excelLoadResponse.Log);

            // CREAR TABLA DE CONSOLIDACIÓN
            var consolidationResp = await _service.CreateConsolidation();

            finalResp.Completed = consolidationResp.Completed;
            finalResp.StatusCode = consolidationResp.StatusCode;
            finalResp.Log.Append(consolidationResp.Log);
            finalResp.NumConsolidate = consolidationResp.NumConsolidate;

            return (finalResp.Completed) ? Ok(finalResp) : StatusCode(finalResp.StatusCode, finalResp);
        }

        [Route("MonthlyDataDump")]
        [HttpPost]
        public async Task<ActionResult> MonthlyDataDump()
        {
            var finalResp = new NumConsolidationResponse();

            // LEER DATOS DE LOS EXCELS
            var excelLoadResponse = await _service.LoadDataFromExcels();
            if (!excelLoadResponse.Completed) return StatusCode(excelLoadResponse.StatusCode, excelLoadResponse);
            
            finalResp.Log.Append(excelLoadResponse.Log);


            // CREAR TABLA DE CONSOLIDACION
            var consolidationResp = await _service.CreateConsolidation();
            if (!consolidationResp.Completed) return StatusCode(consolidationResp.StatusCode, consolidationResp);

            finalResp.Log.Append(consolidationResp.Log);


            // CALCULAR HORAS MENSUALES
            var calcResp = await _service.CalculateMonthlyHours();
            if (!calcResp.Completed) return StatusCode(calcResp.StatusCode, calcResp);

            finalResp.Log.Append(calcResp.Log);
            

            // CREAR TABLA DE HORAS INCURRIDAS POR LIDERES
            var leaderRemainingHoursResp = await _service.CreateLeaderRemainingHours();

            finalResp.Completed = leaderRemainingHoursResp.Completed;
            finalResp.StatusCode = leaderRemainingHoursResp.StatusCode;
            finalResp.Log.Append(leaderRemainingHoursResp.Log);
            finalResp.NumConsolidate = consolidationResp.NumConsolidate;

            return (finalResp.Completed) ? Ok(finalResp) : StatusCode(finalResp.StatusCode, finalResp);
        }

        [Route("DumpEmployeesIntoUsers")]
        [HttpPost]
        public async Task<ActionResult> DumpEmployeesIntoUsers()
        {
            var resp = await _service.DumpEmployeesIntoUsers();
            return StatusCode(resp.StatusCode, resp);
        }

        [Route("GetLeaderRemainingHours")]
        [HttpGet]
        public async Task<ActionResult> GetLeaderRemainingHours(string? leaderId, string? employeeId, string? service)
        {
            var resp = await _service.GetLeaderRemainingHours(leaderId, employeeId, service);
            return StatusCode(resp.StatusCode, resp);
        }

        [Route("GetLeaderIncurredHours")]
        [HttpGet]
        public async Task<ActionResult> GetLeaderIncurredHours(string? leaderId, string? employeeId, string? service)
        {
            var resp = await _service.GetLeaderIncurredHours(leaderId, employeeId, service);
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
            var resp = await _service.GetConsolidatedEmployees();
            return resp.Completed ? Ok(resp) : StatusCode(resp.StatusCode, resp);
        }

    }
}
