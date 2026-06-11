namespace PortfolioAPI.Models
{
    public class Experience
    {
        public string Id { get; set; } = string.Empty;

        public string Company { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int Order { get; set; }
    }
}