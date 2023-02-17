namespace ISD.API.ViewModels.Responses
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

    }
}
