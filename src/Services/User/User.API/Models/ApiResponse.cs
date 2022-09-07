using User.Application.Enums;

namespace User.API.Models
{
    public class ApiResponse<T> : ApiResponse
    {
        public ApiResponse(T data)
        {
            Data = data;
            IsSuccess = true;
        }

        public T Data { get; set; }
    }

    public class ApiResponse
    {
        public ApiResponse()
        {

        }

        public ApiResponse(bool isSuccess)
        {
            if (!isSuccess)
                Code = (int) BaseError.UnknownError;

            IsSuccess = isSuccess;
        }

        public ApiResponse(string message)
        {
            IsSuccess = false;
            Message = message;
            Code = (int)BaseError.UnknownError;
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int Code { get; set; } = (int)BaseError.NoError;
    }
}
