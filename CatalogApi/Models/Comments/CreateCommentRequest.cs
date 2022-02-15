namespace CatalogApi.Models.Comments
{
    public class CreateCommentRequest
    {
        public int FilmId { get; set; }
        
        public int UserId { get; set; }
        
        public string Content { get; set; }
    }
}