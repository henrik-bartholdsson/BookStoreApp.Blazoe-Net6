using AutoMapper;
using AutoMapper.QueryableExtensions;
using BookStoreApp.Api.Data;
using BookStoreApp.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace BookStoreApp.Api.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly BookStoreDbContext context;
        private readonly IMapper mapper;

        public GenericRepository(BookStoreDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<T> AddAsync(T entity)
        {
            await context.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int? id)
        {
            if( id == null)
            {
                return null;
            }
            return await context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            context.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }

        public async Task<VirtualizeResponse<TResult>> GetAllAsync<TResult>(QueryParameters queryParam)
            where TResult : class
        {
            var totalSize = await context.Set<T>().CountAsync();
            var items = await context.Set<T>()
                .Skip(queryParam.StartIndex)
                .Take(queryParam.PageSize)
                .ProjectTo<TResult>(mapper.ConfigurationProvider)
                .ToListAsync();

            return new VirtualizeResponse<TResult> { Items = items, TotalSize = totalSize };
        }
    }
}
