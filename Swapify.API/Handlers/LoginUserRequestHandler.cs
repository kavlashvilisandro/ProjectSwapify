using Dapper;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using Swapify.API.Exceptions;
using Swapify.API.Requests;
using Swapify.API.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;

namespace Swapify.API.Handlers
{
    public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        private readonly IConfiguration _config;
        public LoginUserRequestHandler(IConfiguration config)
        {
            _config = config;
        }
        public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("DB")))
            {
                connection.Open();
                bool entryExists = await connection.QuerySingleOrDefaultAsync<bool>("select true from users where user_name = @UserName", request);
                if (entryExists)
                {
                    return new LoginUserResponse()
                    {
                        Token = GenerateJwtToken(new Claim("UserName", request.UserName))
                    };
                };
                throw new UserNotFound();
            }
        }
        private string GenerateJwtToken(params Claim[] _claims)
        {
            SymmetricSecurityKey symmetricKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("JWT:SecretKey")));

            SigningCredentials signingCredentials =
                new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken token = new JwtSecurityToken
                (
                    issuer: _config.GetValue<string>("JWT:Issuer"),
                    audience: _config.GetValue<string>("JWT:Audience"),
                    claims: _claims,
                    expires: DateTime.Now.AddMinutes(_config.GetValue<int>("JWT:ExpiresMinutes")),
                    signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
