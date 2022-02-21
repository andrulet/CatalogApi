namespace CatalogApi.Models.Comments
{
    public class EditCommentRequest
    {
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}