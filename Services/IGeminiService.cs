using Mscc.GenerativeAI.Types;

namespace PortfolioAPI.Services
{
    public interface IGeminiService
    {
        Task<string> Ask(
     string question,
     string context,
     List<PortfolioAPI.Models.ChatMessage> history);
    }
}
