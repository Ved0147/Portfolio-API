using Google.Cloud.Firestore;
using PortfolioAPI.Models.PortfolioAPI.Models;

namespace PortfolioAPI.Services
{
    public class RetrievalService
    {
        private readonly FirestoreDb _db;

        public RetrievalService(
            FirestoreService firestoreService)
        {
            _db = firestoreService.Db;
        }
        private static readonly HashSet<string> StopWords =
                                                            [
                                                                "what",
                                                                "tell",
                                                                "about",
                                                                "does",
                                                                "the",
                                                                "is",
                                                                "a",
                                                                "an",
                                                                "of",
                                                                "and",
                                                                "to"
                                                            ];

        public async Task<List<KnowledgeDocument>>
     Search(string question)
        {
            var snapshot =
                await _db
                    .Collection("knowledge")
                    .GetSnapshotAsync();

            var words =
                     question
                         .ToLower()
                         .Split(' ',
                             StringSplitOptions.RemoveEmptyEntries)
                         .Where(x => !StopWords.Contains(x))
                         .ToList();

            return snapshot.Documents
                .Select(doc =>
                {
                    var item =
                        doc.ConvertTo<KnowledgeDocument>();

                    var searchableText =
                        $"{item.Title} {item.Category} {item.Content}"
                        .ToLower();

                    var score = 0;

                    foreach (var word in words)
                    {
                        if (item.Title
                            .Contains(word,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            score += 5;
                        }

                        if (item.Category
                            .Contains(word,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            score += 3;
                        }

                        if (item.Content
                            .Contains(word,
                                StringComparison.OrdinalIgnoreCase))
                        {
                            score += 1;
                        }
                    }

                    return new
                    {
                        item,
                        score
                    };
                })
                .Where(x => x.score > 0)
                .OrderByDescending(x => x.score)
                .Take(5)
                .Select(x => x.item)
                .ToList();
        }
    }
}
