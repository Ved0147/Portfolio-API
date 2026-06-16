using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models;
using PortfolioAPI.Models.PortfolioAPI.Models;
using PortfolioAPI.Services;
using System.Text.Json;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : Controller
    {
        private readonly FirestoreDb _db;

        public SeedController(
            FirestoreService firestoreService) 
        {
            _db = firestoreService.Db;
        }
        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            var filePath =
                Path.Combine(
                    AppContext.BaseDirectory,
                    "Data",
                    "knowledge.json");

            Console.WriteLine(filePath);

            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest(
                    $"File not found: {filePath}");
            }

            var json =
                await System.IO.File.ReadAllTextAsync(filePath);

            var docs = JsonSerializer.Deserialize<List<KnowledgeDocument>>(
         json,
         new JsonSerializerOptions
         {
             PropertyNameCaseInsensitive = true
         });

            Console.WriteLine($"Count: {docs?.Count}");

            foreach (var doc in docs!)
            {
                if (string.IsNullOrWhiteSpace(doc.Id))
                {
                    Console.WriteLine(
                        $"Skipping invalid document: {doc.Title}");
                    continue;
                }

                await _db
                    .Collection("knowledge")
                    .Document(doc.Id.Trim())
                    .SetAsync(doc);
            }

            return Ok("Knowledge Base Seeded");
        }
    }
}
