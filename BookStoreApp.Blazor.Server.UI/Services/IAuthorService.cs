﻿using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services
{
    public interface IAuthorService
    {
        Task<Response<List<AuthorReadOnlyDto>>> GetAuthors();
        Task<Response<int>> CreateAuthor(AuthorCreateDto author);
        Task<Response<int>> EditAuthor(int id, AuthorUpdateDto author);
        Task<Response<AuthorReadOnlyDto>> GetAuthor(int id);
        Task<Response<AuthorUpdateDto>> GetAuthorForUpdate(int id);

    }
}
