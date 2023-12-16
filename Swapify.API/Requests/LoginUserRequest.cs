using MediatR;
using Swapify.API.Responses;

namespace Swapify.API.Requests
{
    public class LoginUserRequest : IRequest<LoginUserResponse>
    {
        public string UserName { get; set;}
        public string Password { get; set;}
    }
}
