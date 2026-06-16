using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;
using PortfolioAPI.Models;
using PortfolioAPI.Services;

namespace PortfolioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AIController : ControllerBase
    {
        private readonly FirestoreService _db;
        private readonly RetrievalService _retrievalService;
        private readonly IGeminiService _geminiService;

        public AIController(RetrievalService retrievalService,
            FirestoreService db,
            IGeminiService geminiService)
        {
            _db = db;
            _retrievalService = retrievalService;
            _geminiService = geminiService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask(ChatRequest request)
        {
            var docs =
                await _retrievalService
                    .Search(request.Question);

            var context =
                string.Join(
                    "\n\n",
                    docs.Select(x => x.Content));

            var answer =
                await _geminiService
                    .Ask(
                        request.Question,
                        context,
                        request.History);

            await _db.Db
                .Collection("chatHistory")
                .AddAsync(new
                {
                    question =
                        request.Question,
                    answer,
                    timestamp =
                        Timestamp.GetCurrentTimestamp()
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