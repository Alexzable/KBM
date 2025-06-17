namespace KBMHttpService.Common.Exceptions
{
    public class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message, Exception? inner = null) : base(message, inner) { }
    }
}
