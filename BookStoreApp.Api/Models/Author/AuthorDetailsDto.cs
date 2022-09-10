using BookStoreApp.Api.Models.Book;

namespace BookStoreApp.Api.Models.Author
{
    public class AuthorDetailsDto : AuthorReadOnlyDto
    {
        public List<BookReadOnlyDto> Books { get; set; }
    }
}
