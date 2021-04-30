namespace TMF.Identity.API.ErrorHandling
{
    public class ApiError
    {
        public string Id { get; set; }
        public short Code { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
