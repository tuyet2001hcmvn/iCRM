namespace ISD.ViewModels.API
{
    public class ApiResponse
    {
        public int Code { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

    }
}
