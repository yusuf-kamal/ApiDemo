namespace ApiDemo.Response_Module
{
    public class ApiValdiationErrorResponse : ApiException
    {
        public ApiValdiationErrorResponse()
            : base(400)
        {
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
