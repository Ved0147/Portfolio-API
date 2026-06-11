namespace PortfolioAPI.Services
{
    public class GeminiService : IGeminiService
    {
        public async Task<string> AskAsync(string question)
        {
            return "Gemini response here";
        }
    }
}
