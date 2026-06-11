namespace PortfolioAPI.Models
{
    public class Skill
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int Level { get; set; }

        public int Order { get; set; }
    }
}