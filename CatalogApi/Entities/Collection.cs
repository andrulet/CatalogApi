namespace CatalogApi.Entities
{
    public class Collection
    {
        public int Id { get; set; }
        
        public int Userid { get; set; }
        
        public string Title { get; set; }
        
        public bool IsPrivate { get; set; }
    }
}