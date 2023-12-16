
using Dapper;
using MediatR;
using Npgsql;
using Swapify.API.Entities;

namespace Swapify.API.BackgroundWorkers
{
    public class LablerWorker : BackgroundService
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;
        public LablerWorker(IConfiguration config, IMediator mediator) 
        {
            _config = config;
            _mediator = mediator;
            
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested) 
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("DB")))
                {
                    connection.Open();
                    var toLabel = await connection.QueryAsync<LabelEntity>(@"select id, i_have, i_want from user_posts where status = false");
                    foreach(var label in toLabel) {
                        //var res = await _mediator.Send();

                    }
                }


                await Task.Delay(10000);
            }
        }
    }
}
