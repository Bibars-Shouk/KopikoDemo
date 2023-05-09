using Blazored.LocalStorage;
using BlazorKopikoDemoClient.Client.DataModels;
using BlazorKopikoDemoClient.Protos;
using Grpc.Net.Client;

namespace BlazorKopikoDemoClient.Client.State
{
    public class AuthState
    {
        private readonly ILocalStorageService _localstorage;
        private readonly GrpcChannel _channel;

        public event Action OnChange;

        private AuthData _authData = new AuthData
        {
            IsAuthenticated = false,
            AccessToken = String.Empty,
            AccessTokenExpiryDate = DateTime.MinValue,
            RefreshToken = String.Empty,
            RefreshTokenExpiryDate = DateTime.MinValue,
        };

        public AuthData AuthData { get { return _authData; } set { _authData = value; OnChange?.Invoke(); } }

        public AuthState(ILocalStorageService localstorage, GrpcChannel channel)
        {
            _localstorage = localstorage;
            _channel = channel;
        }

        public bool CheckRefreshTokenExpiryDate(DateTime refreshTokenExpiryDate)
        {
            // for a quick test, replace the if statement with this one:
            //if (refreshTokenExpiryDate.AddDays(-10) < DateTime.UtcNow) return false;

            if (refreshTokenExpiryDate.AddMinutes(-5) < DateTime.UtcNow) return false;
            else return true;
        }

        public bool CheckAccessTokenExpiryDate(DateTime accessTokenExpiryDate)
        {
            // for a quick test, replace the if statement with this one:
            //if (accessTokenExpiryDate.AddMinutes(-100) < DateTime.UtcNow) return false;

            if (accessTokenExpiryDate < DateTime.UtcNow) return false;
            else return true;
        }

        public async Task LogoutUser()
        {
            AuthData = new AuthData
            {
                IsAuthenticated = false,
                AccessToken = String.Empty,
                AccessTokenExpiryDate = DateTime.MinValue,
                RefreshToken = String.Empty,
                RefreshTokenExpiryDate = DateTime.MinValue,
            };
            OnChange?.Invoke();

            await _localstorage.RemoveItemAsync("auth");
        }

        public async Task RefreshAccessToken()
        {
            var client = new Auth.AuthClient(_channel);

            var refreshTokenRequest = new RefreshTokenRequest
            {
                AccessToken = AuthData.AccessToken,
                RefreshToken = AuthData.RefreshToken
            };

            var _reply = await client.RefreshTokenAsync(refreshTokenRequest);

            await HandleAuthReply(_reply);
        }

        public async Task InitUserAuth()
        {
            var isAuthExist = await _localstorage.ContainKeyAsync("auth");
            if (!isAuthExist)
            {
                await _localstorage.SetItemAsync<AuthData>("auth", AuthData);
            }
            else
            {
                var AuthDateTmp = await _localstorage.GetItemAsync<AuthData>("auth");

                if (AuthDateTmp.IsAuthenticated)
                {
                    if (!CheckRefreshTokenExpiryDate(AuthDateTmp.RefreshTokenExpiryDate))
                    {
                        await LogoutUser();
                    }
                    else if (!CheckAccessTokenExpiryDate(AuthDateTmp.AccessTokenExpiryDate))
                    {
                        AuthData = AuthDateTmp;
                        await RefreshAccessToken();
                    }
                    else
                    {
                        AuthData = AuthDateTmp;
                        OnChange?.Invoke();
                    }
                }
            }
        }

        // HandleAuthReply only handles changing the auth state and saving it to the localstorage
        public async Task HandleAuthReply(AuthReply reply)
        {
            AuthData = new AuthData
            {
                IsAuthenticated = true,
                AccessToken = reply.AccessToken,
                AccessTokenExpiryDate = reply.AccessTokenExpiryDate.ToDateTime(),
                RefreshToken = reply.RefreshToken,
                RefreshTokenExpiryDate = reply.RefreshTokenExpiryDate.ToDateTime(),
            };

            OnChange?.Invoke();

            await _localstorage.SetItemAsync<AuthData>("auth", AuthData);
        }
    }
}
