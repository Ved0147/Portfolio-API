namespace PortfolioAPI.Models
{
    public class Projects
    {
        public string Id { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string GithubUrl { get; set; } = string.Empty;

        //public bool Featured { get; set; }

        public int Order { get; set; }

        public string Type { get; set; } = string.Empty;

        public List<string> Technologies { get; set; }
            = new();

        public List<string> Featured { get; set; }
            = new();
    }
}