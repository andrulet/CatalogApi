using CatalogApi.Models.Comments;

namespace CatalogApi.Services.IServices;

public interface ICommentService
{
    CommentResponse Create(CreateCommentRequest request);
    void Delete(int id);
    void Edit(int id, EditCommentRequest request);
}