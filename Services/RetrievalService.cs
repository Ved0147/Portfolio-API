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
        private static readonly Dictionary<string, string[]>
 CategoryKeywords = new()
 {
     ["experience"] =
     [
         "experience",
        "company",
        "companies",
        "worked",
        "job",
        "jobs",
        "career",
        "employment"
     ],

     ["skills"] =
     [
         "skill",
        "skills",
        "technology",
        "technologies",
        "stack"
     ],

     ["project"] =
     [
         "project",
        "projects",
        "application",
        "portfolio"
     ],

     ["education"] =
     [
         "education",
        "degree",
        "college",
        "university",
        "study",
        "studies",
        "bachelor",
        "master"
     ],

     ["career"] =
     [
         "career",
        "goal",
        "goals",
        "future",
        "aspiration"
     ],

     ["certification"] =
     [
         "certification",
        "certifications",
        "certificate",
        "certificates"
     ]
 };
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
                        if (item.Title.Contains(
                            word,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            score += 10;
                        }

                        if (item.Category.Contains(
                            word,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            score += 8;
                        }

                        if (item.Content.Contains(
                            word,
                            StringComparison.OrdinalIgnoreCase))
                        {
                            score += 2;
                        }

                        if (CategoryKeywords.TryGetValue(
                            item.Category.ToLower(),
                            out var aliases))
                        {
                            if (aliases.Contains(word))
                            {
                                score += 20;
                            }
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
                .Take(8)
                .Select(x => x.item)
                .ToList();
        }
    }
}
