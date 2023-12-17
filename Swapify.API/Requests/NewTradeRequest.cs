using MediatR;
using Swapify.API.Responses;

namespace Swapify.API.Requests
{
    public class NewTradeRequest : IRequest<NewTradeResponse>
    {
        public string HaveItem { get; set; }
        public string WantItem { get; set; }
        public string Description { get; set; }
        //public List<IFormFile> Image { get; set; }
    }
}
