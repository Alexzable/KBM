namespace KBMHttpService.Clients.Grpc.Common
{
    public class GrpcSettings
    {
        public string Host { get; set; } = null!;        
        public TimeSpan DefaultTimeout { get; set; } 
        public int MaxRetryAttempts { get; set; }  
        public TimeSpan RetryBackoff { get; set; }
    }
}
