namespace WebApi.Common.Exceptions
{
    public class AppBadRequestException : Exception
    {
        public AppBadRequestException(string message) : base(message) { }
    }
}
