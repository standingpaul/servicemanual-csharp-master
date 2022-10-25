using Microsoft.AspNetCore.Mvc;

namespace ServiceManual.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceManualController
    {
        [HttpGet]
        public async Task<string> Get()
        {
            Console.WriteLine($"Get");

            return "Worked";
        }
    }
}
