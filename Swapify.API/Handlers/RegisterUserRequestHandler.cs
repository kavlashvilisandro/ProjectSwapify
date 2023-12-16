using MediatR;
using Swapify.API.Requests;
using Npgsql;
using Dapper;
using Swapify.API.Exceptions;

namespace Swapify.API.Handlers
{
    public class RegisterUserRequestHandler : IRequestHandler<RegisterUserRequest>
    {
        private readonly IConfiguration _config;
        public RegisterUserRequestHandler(IConfiguration config)
        {
            _config = config;
        }
        public async Task Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("DB")))
            {
                connection.Open();
                using(NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    bool entryExists = await connection.QuerySingleOrDefaultAsync<bool>("select true from users where user_name = @UserName", request,transaction);
                    if (entryExists)
                    {
                        throw new EntryAlreadyExists();
                    }
                    await connection.ExecuteAsync("insert into users(user_name,password) values(@UserName,@Password)", request, transaction);
                }
            }
        }
    }
}
