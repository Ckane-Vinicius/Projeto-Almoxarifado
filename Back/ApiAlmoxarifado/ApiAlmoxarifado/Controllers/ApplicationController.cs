using ApiAlmoxarifado.Application.Application_Interfaces;
using ApiAlmoxarifado.DTO;
using Microsoft.AspNetCore.Mvc;

namespace ApiAlmoxarifado.Controllers
{
    [Produces("application/json")]
    [Route("api/v1")]
    public class Request01Controller : Controller
    {
        private readonly IApplication _applicationApplication;
        private readonly ILogger<Request01Controller> _logger;

        public Request01Controller(IApplication applicationApplication, ILogger<Request01Controller> logger)
        {
            _applicationApplication = applicationApplication;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Request request)
        {
            try
            {
                var result = await _applicationApplication.ProcessRequestAsync(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _applicationApplication.GetRegistrosAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
