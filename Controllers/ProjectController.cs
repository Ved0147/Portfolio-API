using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models;
using PortfolioAPI.Services;

namespace PortfolioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly FirestoreDb _db;

    public ProjectsController(
        FirestoreService firestoreService)
    {
        _db = firestoreService.Db;
    }

    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var snapshot =
            await _db.Collection("projects")
                     .GetSnapshotAsync();

        var projects =
            snapshot.Documents
            .Select(doc => new Projects
            {
                Id = doc.Id,

                Title = doc.GetValue<string>("title"),

                Description = doc.GetValue<string>("description"),

                GithubUrl =doc.GetValue<string>("github"),

                //Featured =doc.GetValue<bool>("featured"),

                //Order =doc.GetValue<int>("order"),

                //Type =doc.GetValue<string>("type"),

                Technologies = doc.GetValue<List<string>>("techStack"),

                Featured =doc.GetValue<List<string>>("featured")
            })
            .OrderBy(x => x.Order)
            .ToList();

        return Ok(projects);
    }
}