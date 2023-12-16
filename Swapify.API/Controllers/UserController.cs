using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swapify.API.Requests;
using Swapify.API.Responses;

namespace Swapify.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            await _mediator.Send(registerUserRequest);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest loginUserRequest)
        {
            return Ok(await _mediator.Send(loginUserRequest));
        }
    }
}
