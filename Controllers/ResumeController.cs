using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public ResumeController()
        {
            _db = new FirestoreDbBuilder
            {
                ProjectId = "project-7933a77a-b6a8-43ad-819",
                DatabaseId = "ved-portfolio"
            }.Build();
        }

        [HttpGet]
        public IActionResult DownloadResumeFromBucket()
        {
            return Ok(new
            {
                url = "https://storage.googleapis.com/ved-portfolio-website/Ved_Vaiwala_Resume.pdf"
            });
        }

        [HttpPost("download")]
        public async Task<IActionResult> Download()
        {
            var docRef = _db
                .Collection("counters")
                .Document("resumeDownloads");

            var snapshot =
                await docRef.GetSnapshotAsync();

            int count = 0;

            if (snapshot.Exists)
            {
                count =
                    snapshot.GetValue<int>("count");
            }

            await docRef.SetAsync(
                new
                {
                    count = count + 1
                });

            return Ok();
        }
    }
}