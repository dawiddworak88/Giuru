namespace Foundation.ApiExtensions.Communications
{
    public class ApiRequest<T>
    {
        public T Data { get; set; }
        public string Language { get; set; }
        public string AccessToken { get; set; }
        public string EndpointAddress { get; set; }
    }
}
