namespace CatalogApi.Models.Collections;

public class CollectionCreateRequest
{
    public string Title { get; set; }
        
    public bool IsPrivate { get; set; }
        
    public int UserId { get; set; }
}