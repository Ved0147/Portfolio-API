using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorController : ControllerBase
    {
        private readonly FirestoreDb _db;
        public VisitorController()
        {
            _db = new FirestoreDbBuilder
            {
                ProjectId = "project-7933a77a-b6a8-43ad-819",
                DatabaseId = "ved-portfolio"
            }.Build();
        }
        [HttpGet]
        public async Task<IActionResult> IncrementandGet()
        {
            var docRef = _db.Collection("counters").Document("visitors");
            var snapshot = await docRef.GetSnapshotAsync();
            int count = 0;
            if(snapshot.Exists)
            {
                count = snapshot.GetValue<int>("count");
            }
            count++;
            await docRef.SetAsync(new { count });

            return Ok(new
            {
                visitors = count
            });
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var docRef = _db.Collection("counters")
                            .Document("visitors");

            var snapshot = await docRef.GetSnapshotAsync();

            int count = snapshot.GetValue<int>("count");

            return Ok(new
            {
                visitors = count
            });
        }
    }
}
