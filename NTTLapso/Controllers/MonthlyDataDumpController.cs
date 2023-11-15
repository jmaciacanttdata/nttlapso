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
        public async Task<CustomResponseCharge> DumpData(bool isFirstCharge = false, string ? userId = null)
        {
            if (isFirstCharge)
                return await _service.DumpData(userId);
            else
                return null;
        }
    }
}
