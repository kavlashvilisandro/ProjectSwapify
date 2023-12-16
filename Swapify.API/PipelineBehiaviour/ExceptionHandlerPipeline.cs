using MediatR;

namespace Swapify.API.PipelineBehiaviour
{
    public class ExceptionHandlerPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }catch(Exception ex)
            {
                throw;
            }
        }
    }
}
