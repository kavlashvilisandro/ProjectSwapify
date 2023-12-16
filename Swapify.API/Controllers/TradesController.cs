using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swapify.API.Requests;

namespace Swapify.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TradesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TradesController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task AddNewPost([FromBody] NewTradeRequest newTradeRequest)
        {

        }

    }
}
