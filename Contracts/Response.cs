namespace Catalog.Contracts
{
    public class Response<T>
    {
        public int statusCode { get; set; }
        public T data { get; set; }

        public string message { get; set; } = string.Empty;
    }
}
