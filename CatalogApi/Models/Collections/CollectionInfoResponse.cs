using CatalogApi.Entities;

namespace CatalogApi.Models.Collections;

public class CollectionInfoResponse
{
    public int Id { get; set; }
        
    public string Title { get; set; }
    
    public string FirstName { get; set; }

    public CollectionInfoResponse(Collection collection)
    {
        Id = collection.Id;
        Title = collection.Title;
        FirstName = collection.User.FirstName;
    }
}