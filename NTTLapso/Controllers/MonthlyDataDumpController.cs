using Microsoft.AspNetCore.Mvc;
using NTTLapso.Models.DataDump;
using NTTLapso.Service;

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

        [Route("NTTLapso/MonthlyDump")]
        [HttpPost]
        public async Task<ActionResult> DumpData(bool isFirstCharge = false, string ? userId = null)
        {
            if (isFirstCharge)
            {
                DataDumpResponse resp = await _service.DumpData(userId);

                // Si no ha habido problemas cargando los usuarios en la tabla monthly_incurred_hours => cargamos los excels para volcarlos datos a la base de datos.
                
                if(resp.Completed == true)
                {
                    DataDumpResponse excelLoadResponse = await _service.LoadDataFromExcels();
                    resp.Completed = excelLoadResponse.Completed;
                    resp.Message += "\n"+excelLoadResponse.Message;
                }
                
                return Ok(resp);
            }
            else
                return Ok(await _service.LoadDataFromExcels());
        }
    }
}
