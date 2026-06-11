using Google.Cloud.Firestore;

namespace PortfolioAPI.Services;

public class FirestoreService
{
    public FirestoreDb Db { get; }

    public FirestoreService()
    {
        Db = new FirestoreDbBuilder
        {
            ProjectId = "project-7933a77a-b6a8-43ad-819",
            DatabaseId = "ved-portfolio"
        }.Build();
    }
}