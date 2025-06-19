namespace KBMHttpService.Common
{
    public class ResultId<T>
    {
        public required T Id { get; set; }

        public ResultId() { }

        public ResultId(T id)
        {
            Id = id;
        }
    }
}
