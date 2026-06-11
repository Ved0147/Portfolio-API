namespace PortfolioAPI.Services
{
    public interface IGeminiService
    {
        Task<string> AskAsync(string question);
    }
}
