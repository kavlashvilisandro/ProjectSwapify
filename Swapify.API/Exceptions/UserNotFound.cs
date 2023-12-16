using Swapify.API.Exceptions.Base;

namespace Swapify.API.Exceptions
{
    public class UserNotFound : CustomException
    {
        public UserNotFound() : base("User not found", 404) { }
    }
}
