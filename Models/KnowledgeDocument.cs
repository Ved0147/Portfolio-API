using Google.Cloud.Firestore;

namespace PortfolioAPI.Models
{
    namespace PortfolioAPI.Models
    {
        [FirestoreData]
        public class KnowledgeDocument
        {
            [FirestoreProperty]
            public string Id { get; set; } = "";

            [FirestoreProperty]
            public string Category { get; set; } = "";

            [FirestoreProperty]
            public string Title { get; set; } = "";

            [FirestoreProperty]
            public string Content { get; set; } = "";

            [FirestoreProperty]
            public string Company { get; set; } = "";
        }
    }
}
