﻿using AutoMapper;
using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Services.Base;

namespace BookStoreApp.Blazor.Server.UI.Services
{
    public class AuthorService : BaseHttpService, IAuthorService
    {
        private readonly IClient _client;
        private readonly IMapper mapper;

        public AuthorService(IClient client, ILocalStorageService localStorage, IMapper mapper)
            : base(client, localStorage)
        {
            _client = client;
            this.mapper = mapper;
        }

        public async Task<Response<int>> Create(AuthorCreateDto author)
        {
            Response<int> response = new Response<int> {  };
            try
            {
                await GetBearerToken();
                await _client.AuthorsPOSTAsync(author);
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<int>(exception);
            }

            return response;
        }

        public async Task<Response<int>> Delete(int id)
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await _client.AuthorsDELETEAsync(id);
            }
            catch (ApiException ex)
            {

                response = ConvertApiExceptions<int>(ex);
            }

            return response;
        }

        public async Task<Response<int>> Edit(int id, AuthorUpdateDto author)
        {
            Response<int> response = new Response<int> { };
            try
            {
                await GetBearerToken();
                await _client.AuthorsPUTAsync(id, author);
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<int>(exception);
            }

            return response;
        }

        public async Task<Response<AuthorDetailsDto>> Get(int id)
        {
            Response<AuthorDetailsDto> response;

            try
            {
                await GetBearerToken();

                var data = await _client.AuthorsGETAsync(id);
                response = new Response<AuthorDetailsDto>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<AuthorDetailsDto>(exception);
            }


            return response;
        }

        public async Task<Response<List<AuthorReadOnlyDto>>> Get()
        {
            Response<List<AuthorReadOnlyDto>> response;

            try
            {
                await GetBearerToken();
                var data = await _client.AuthorsAllAsync();
                response = new Response<List<AuthorReadOnlyDto>>
                {
                    Data = data.ToList(),
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<List<AuthorReadOnlyDto>>(exception);
            }


            return response;
        }

        public async Task<Response<AuthorUpdateDto>> GetForUpdate(int id)
        {
            Response<AuthorUpdateDto> response;

            try
            {
                await GetBearerToken();

                var readOnlyDto = await _client.AuthorsGETAsync(id);

                var data = mapper.Map<AuthorUpdateDto>(readOnlyDto);

                data.Id = id;

                response = new Response<AuthorUpdateDto>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<AuthorUpdateDto>(exception);
            }


            return response;
        }
    }
}
