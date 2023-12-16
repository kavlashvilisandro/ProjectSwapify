using MediatR;

namespace Swapify.API.Requests
{
    public class RegisterUserRequest : IRequest
    {
        public string UserName { get; set;}
        public string Password { get; set;}
    }
}
