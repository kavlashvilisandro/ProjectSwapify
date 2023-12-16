using Dapper;
using MediatR;
using Npgsql;
using Swapify.API.Requests;
using Swapify.API.Responses;

namespace Swapify.API.Handlers
{
    public class MatchLabelHandler : IRequestHandler<MatchLabelRequest, MatchLabelResponse>
    {
        private readonly IConfiguration _config;
        public MatchLabelHandler(IConfiguration config)
        {
            _config = config;
        }
        public Task<MatchLabelResponse> Handle(MatchLabelRequest request, CancellationToken cancellationToken)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("DB")))
            {
                connection.Open();
                var res = await connection.QueryFirstAsync<long>("");
            }
        }
    }
}
