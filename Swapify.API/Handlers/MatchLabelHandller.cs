using Dapper;
using MediatR;
using Npgsql;
using OpenAI_API;
using Swapify.API.Entities;
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
        public async Task<MatchLabelResponse> Handle(MatchLabelRequest request, CancellationToken cancellationToken)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_config.GetConnectionString("DB")))
            {
                connection.Open();
                var qeuryOne = await connection.QueryFirstAsync<LabelEntity>(@"select * from labels where label like '%' || @label ||'%'", new {request.Name});
                if (qeuryOne !=  null)
                {
                    return new MatchLabelResponse(){MatchedLabelId = qeuryOne.Id};
                }
                var queryAll = await connection.QueryAsync<LabelEntity>(@"select * from labels");
                var match = await GetMatch(request.Name, queryAll);
                if(match.Id == 0) 
                {
                    match.Id = await connection.ExecuteAsync(@"insert into labels (label) values (@label) returning id", new {match.Label});
                }
                return new MatchLabelResponse() { MatchedLabelId = match.Id};
            }
            
        }

        private async Task<LabelEntity> GetMatch(string label, IEnumerable<LabelEntity> labels)
        {
            var openaiApiKey = _config.GetSection("openaiApiKey").Value;
            var openai = new OpenAIAPI(openaiApiKey);
            var dick = labels.ToDictionary(t => t.Id, t => t.Label);
            var stringDick = string.Join(", ", dick.Select(kv => $"{kv.Key}: {kv.Value}"));
            var prompt = $"Given the user's input: '{label}', and the list of existing labels: {stringDick}, " +
                $"provide a response in the format:\r\n- If a match is found, return idOfObj'_' followed by an " +
                $"empty string.\r\n- If no match is found, return '0_' followed by the label of the unmatched " +
                $"object (eg.for \"flufy red cat\" label would be cat).\r\n\r\nConsider various aspects of " +
                $"similarity such as object similarity that those words represent, partial matching, and common " +
                $"terms when determining a match.";

            var response = await openai.Completions.CreateCompletionAsync(
                model: "text-davinci-003",
                prompt: prompt,
                temperature: 0.7,
                max_tokens: 150
            );
            var resp = response.Completions[0].Text;
            var id = Int32.Parse(resp.Substring(0, resp.IndexOf("_")));
            var matchedLabel = resp.Substring(resp.IndexOf("_"));

            return new LabelEntity { Id = id, Label = matchedLabel };
        }
    }
}
