using System.Net;

namespace BlogWebsite.WebApi.Utilities
{
    public class CustomException : Exception
    {
        public CustomException(string message) : base(message) { }
    }
}
