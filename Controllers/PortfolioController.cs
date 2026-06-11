using Microsoft.AspNetCore.Mvc;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var Portfolio = new
            {
                Name = "Ved Vaiwala",

                Skills = new[]
                {
                    "C#",
                    "ASP.NET Core",
                    "Entity Framework Core",
                    "SQL Server",
                    "Google Cloud"
                },
                Projects = new[]
                 {
                "Portfolio Website",
                "Visitor Counter API",
                "EBook Store",
                "Personal Resume AI Assistant",
            },
                Goal = "Become a Cloud, DevOps and AI Engineer"
            };
            return Ok(Portfolio);
        }
    }
}
