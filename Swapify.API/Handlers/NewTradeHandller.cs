using Dapper;
using MediatR;
using Npgsql;
using Swapify.API.Exceptions;
using Swapify.API.Requests;
using Swapify.API.Responses;
using System.Data;

namespace Swapify.API.Handlers
{
    public class NewTradeHandller : IRequestHandler<NewTradeRequest, NewTradeResponse>
    {
        private readonly IConfiguration _config;

        public NewTradeHandller(IConfiguration config) 
        {
            _config = config;
        }
        public async Task<NewTradeResponse> Handle(NewTradeRequest request, CancellationToken cancellationToken)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("DB")))
            {
                connection.Open();
                var dp = new DynamicParameters();
                dp.Add("haveItem", request.HaveItem, DbType.String);
                dp.Add("wantItem", request.WantItem, DbType.String);
                dp.Add("description", request.Description, DbType.String);

                var id = await connection.ExecuteAsync("insert into user_posts(i_have, i_want, status, description) values (@haveItem, @wantItem, false, @description) returning id", dp);
                return new NewTradeResponse() { NewTradeId = id };
            }
        }
    }
}
