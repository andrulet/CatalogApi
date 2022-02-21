namespace CatalogApi.Models.Collections;

public class FilmAddIntoCollectionRequest
{
    public int CollectionId { get; set; }

    public int FilmId { get; set; }
}