namespace KBMHttpService.Shared.Exceptions
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string? Code { get; set; }

        public ErrorResponse(string message, string? code = null)
        {
            Message = message;
            Code = code;
        }
    }
}
