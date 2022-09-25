using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models.Author;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Api.Repositories
{
    public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
    {
        private readonly BookStoreDbContext context;
        private readonly IMapper mapper;
        public AuthorRepository(BookStoreDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<AuthorDetailsDto> GetAuthorDetailsAsync(int id)
        {
            var author = await context.Authors
                    .Include(q => q.Books)
                    .ProjectTo<AuthorDetailsDto>(mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(q => q.Id == id);

            return author;
        }
    }
}
