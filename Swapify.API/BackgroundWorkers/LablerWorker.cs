
using Dapper;
using MediatR;
using Npgsql;
using Swapify.API.Entities;
using Swapify.API.Requests;

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
                    var toLabel = await connection.QueryAsync<PostEntity>(@"select id, i_have as HaveItem, i_want as WantItem from user_posts where status = false");
                    foreach (var post in toLabel)
                    {
                        var wantRes = await _mediator.Send(new MatchLabelRequest() { Name = post.WantItem, PostId = post.Id });
                        var haveRes = await _mediator.Send(new MatchLabelRequest() { Name = post.HaveItem, PostId = post.Id });
                        var dp = new DynamicParameters();
                        dp.Add("postId", post.Id);
                        dp.Add("haveLabelId", haveRes.MatchedLabelId);
                        dp.Add("wantLabelId", wantRes.MatchedLabelId);
                        var res = await connection.ExecuteAsync(@"insert into post_labels values (@postId, @haveLabelId, @wantLabelId)", dp);
                        //send data to ilia

                    }
                }


                await Task.Delay(1000);
            }
        }
    }
}
