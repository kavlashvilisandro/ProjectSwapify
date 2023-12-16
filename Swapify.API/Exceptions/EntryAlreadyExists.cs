using Swapify.API.Exceptions.Base;

namespace Swapify.API.Exceptions
{
    public class EntryAlreadyExists : CustomException
    {
        public EntryAlreadyExists() : base("This entry already exists", 400) { }
    }
}
