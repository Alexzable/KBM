namespace KBMHttpService.Common.Exceptions
{
    public class ClientMappingException : Exception
    {
        public ClientMappingException(string message, Exception? inner = null) : base(message, inner) { }
    }
}
