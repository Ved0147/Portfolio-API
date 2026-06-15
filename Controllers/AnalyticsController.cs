using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public AnalyticsController()
        {
            _db = new FirestoreDbBuilder
            {
                //ProjectId = "project-7933a77a-b6a8-43ad-819",
                //DatabaseId = "ved-portfolio"
                ProjectId = "portfolio-prod-499308",
                DatabaseId = "portfolio-prod"

            }.Build();
        }

        [HttpGet]
        public async Task<IActionResult> GetAnalytics()
        {
            var visitorDoc = await _db
                .Collection("counters")
                .Document("visitors")
                .GetSnapshotAsync();

            int visitors = visitorDoc.Exists
                ? visitorDoc.GetValue<int>("count")
                : 0;
            var aiChatDoc = await _db
                                .Collection("counters")
                                .Document("aiChats")
                                .GetSnapshotAsync();

            var resumeDoc = await _db
                .Collection("counters")
                .Document("resumeDownloads")
                .GetSnapshotAsync();

            int aiChats =
                aiChatDoc.Exists
                ? aiChatDoc.GetValue<int>("count")
                : 0;

            int resumeDownloads =
                resumeDoc.Exists
                ? resumeDoc.GetValue<int>("count")
                : 0;

            return Ok(new
            {
                visitors,
                aiChats,
                resumeDownloads
            });
        }
    }
}