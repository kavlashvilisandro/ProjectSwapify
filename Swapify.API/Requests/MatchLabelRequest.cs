using MediatR;
using Swapify.API.Responses;

namespace Swapify.API.Requests
{
    public class MatchLabelRequest : IRequest<MatchLabelResponse>
    {
        public long PostId { get; set; }
        public string Name { get; set;}
    }
}
