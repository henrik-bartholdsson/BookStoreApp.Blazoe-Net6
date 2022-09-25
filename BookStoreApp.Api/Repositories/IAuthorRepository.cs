using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Author;

namespace BookStoreApp.Api.Repositories
{
    public interface IAuthorRepository : IGenericRepository<Author>
    {
        Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id);
    }
}
