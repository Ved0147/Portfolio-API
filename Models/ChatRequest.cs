namespace PortfolioAPI.Models
{
    public class ChatRequest
    {
        public string Question { get; set; } = "";

        public List<ChatMessage> History
        {
            get;
            set;
        } = [];
    }   
}
