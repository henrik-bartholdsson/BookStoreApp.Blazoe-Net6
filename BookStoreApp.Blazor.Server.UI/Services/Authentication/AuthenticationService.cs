using Blazored.LocalStorage;
using BookStoreApp.Blazor.Server.UI.Providers;
using BookStoreApp.Blazor.Server.UI.Services.Base;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreApp.Blazor.Server.UI.Services.Authentication
{
    public class AuthenticationService : BaseHttpService, IAuthenticationService
    {
        private readonly IClient _httpClient;
        private readonly ILocalStorageService localStorage;
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public AuthenticationService(IClient httpClient, ILocalStorageService localStorage, AuthenticationStateProvider authenticationStateProvider)
            : base(httpClient, localStorage)
        {
            _httpClient = httpClient;
            this.localStorage = localStorage;
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<Response<AuthResponse>> AuthenticateAsync(LoginUserDto loginModel)
        {
            Response<AuthResponse> response;
            try
            {
                var result = await _httpClient.LoginAsync(loginModel);
                response = new Response<AuthResponse>
                {
                    Data = result,
                    Success = true
                };
                await localStorage.SetItemAsync("accessToken", result.Token);

                await ((ApiAuthenticationStateProvider)authenticationStateProvider).LogedIn();
            }
            catch (ApiException exception)
            {
                response = ConvertApiExceptions<AuthResponse>(exception);
            }

            return response;
        }

        public async Task Logout()
        {
            await ((ApiAuthenticationStateProvider)authenticationStateProvider).LoggedOut();
        }
    }
}
