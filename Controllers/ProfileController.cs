using Microsoft.AspNetCore.Mvc;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProfile()
        {
            var profile = new Models.Profile
            {
                Name = "Ved Vaiwala",
                Role = "Backend Developer",
                Location = "Surat, India"
            };
            return Ok(profile);
        }
    }
}
