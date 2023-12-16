using MediatR;
using Swapify.API.Requests;
using Swapify.API.Responses;

namespace Swapify.API.Handlers
{
    public class LoginUserRequestHandler : IRequestHandler<LoginUserRequest, LoginUserResponse>
    {
        public Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
