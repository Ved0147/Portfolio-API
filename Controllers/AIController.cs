using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly FirestoreDb _db;

        public AIController()
        {
            _db = new FirestoreDbBuilder
            {
                //ProjectId = "project-7933a77a-b6a8-43ad-819",
                //DatabaseId = "ved-portfolio"
                ProjectId = "portfolio-prod-499308",
                DatabaseId = "portfolio-prod"
            }.Build();
        }

        [HttpPost]
        public async Task<IActionResult> Ask(ChatRequest request)
        {
            var chatDoc = _db.Collection("counters").Document("aiChats");

            var chatSnapshot =
                await chatDoc.GetSnapshotAsync();

            int chatCount = 0;

            if (chatSnapshot.Exists)
            {
                chatCount =
                    chatSnapshot.GetValue<int>("count");
            }

            await chatDoc.SetAsync(
                new
                {
                    count = chatCount + 1
                });
            var docRef = _db.Collection("portfolio")
                            .Document("info");

            var snapshot = await docRef.GetSnapshotAsync();

            if (!snapshot.Exists)
            {
                return NotFound("Portfolio data not found");
            }

            var data = snapshot.ToDictionary();

            string question =
                request.Question.ToLower();

            string answer =
                "I don't have information about that.";

            if (question.Contains("skill"))
            {
                answer = data["skills"].ToString();
            }
            else if (question.Contains("project"))
            {
                answer = data["projects"].ToString();
            }
            else if (question.Contains("goal"))
            {
                answer = data["goal"].ToString();
            }
            else if (question.Contains("role"))
            {
                answer = data["role"].ToString();
            }
            else if (question.Contains("name"))
            {
                answer = data["name"].ToString();
            }
            if (question == "hi" ||
                question == "hello" ||
                question == "hey")
            {
                answer =
                    "Hi! I'm Ved AI Assistant. How may I assist you?";
            }
            else if (question.Contains("how are you"))
            {
                answer =
                    "I'm doing great! I'm here to help you learn more about Ved's professional background.";
            }
            else if (question.Contains("thank"))
            {
                answer =
                    "Welcome";
                    //"You're welcome! Feel free to ask anything about Ved's experience and projects.";
            }
            await _db
                    .Collection("chatHistory")
                    .AddAsync(new
                    {
                        question = request.Question,
                        answer = answer,
                        timestamp = Timestamp.GetCurrentTimestamp()
                    });
            return Ok(new
            {
                answer
            });
        }
    }
}
#region AI Assistant with Gemini API
//namespace PortfolioAPI.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AIController : ControllerBase
//    {
//        private readonly IConfiguration _configuration;

//        public AIController(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }
//        [HttpPost]
//        public async Task<IActionResult> Ask(ChatRequest request)
//        {
//            string prompt = $"""
//                You are Ved Vaiwala's AI assistant.

//                Portfolio Information:

//                Name: Ved Vaiwala

//                Skills:
//                - ASP.NET Core
//                - React
//                - TypeScript
//                - SQL Server
//                - Google Cloud
//                - Firestore

//                Projects:
//                - Portfolio Website
//                - Visitor Counter API

//                Goal:
//                Become a Cloud and DevOps Engineer.

//                User Question:
//                {request.Question}

//                Answer professionally and only using the information above.
//                """;

//            var apiKey = _configuration["Gemini:ApiKey"];
//            GoogleAI googleAI = new(apiKey);

//            var model = googleAI.GenerativeModel("gemini-2.0-flash");

//            var response = await model.GenerateContent(prompt);
//            return Ok(new
//            {
//                answer = response.Text
//            });
//        }
//    }
//}
#endregion