using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;

namespace PortfolioAPI.Services
{
   

    public class GeminiService : IGeminiService
    {
        private readonly string _apiKey;

        public GeminiService(IConfiguration configuration)
        {
            _apiKey = configuration["Gemini:ApiKey"]!;
        }

        public async Task<string> Ask(
    string question,
    string context,
    List<PortfolioAPI.Models.ChatMessage> history)
        {
            var conversation =
                history?.Any() == true
                    ? string.Join(
                        "\n",
                        history.Select(x =>
                            $"{x.Sender}: {x.Text}"))
                    : "No previous conversation.";

            var prompt =
                $"""
        You are Ved Vaiwala's personal portfolio assistant.

        Your purpose is to answer questions about:
        - Skills
        - Work experience
        - Projects
        - Education
        - Certifications
        - Career goals

        Rules:
        1. Use ONLY the provided context.
        2. Be concise and professional.
        3. Summarize instead of copying context.
        4. If information is unavailable, say:
           "I don't have information about that."
        5. Answer in second-person when describing Ved.
        6. Use conversation history to understand follow-up questions.
        7. Answer naturally.
        8. Summarize information.
        9. Do NOT dump raw context.
        10. Use bullet points when listing skills.
        11. For company questions, list all companies found.
        12. Keep responses under 100 words.
        13. Avoid repeating information.
        14. If the question is unrelated, politely decline to answer.
        15. If the question is about Ved's personal life, politely decline to answer.
        16. For skill questions, list all skills found

        Conversation History:
        {conversation}

        Context:
        {context}

        Current Question:
        {question}
        """;

            var googleAI = new GoogleAI(apiKey: _apiKey);

            var model =
                googleAI.GenerativeModel(
                    model: Model.Gemini25Flash);

            var response =
                await model.GenerateContent(prompt);

            return response.Text;
        }
    }
}
