namespace P7CreateRestApi.DTO.CommonDTO
{
    public class ApiResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public static ApiResponseDTO<T> SuccessResult(T data, string message = "Operation successful")
        {
            return new ApiResponseDTO<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponseDTO<T> ErrorResult(string message, IList<string>? errors = null)
        {
            return new ApiResponseDTO<T>
            {
                Success = false,
                Message = message,
                Errors = errors ?? new List<string>()
            };
        }

        public static ApiResponseDTO<T> ErrorResult(IList<string> errors)
        {
            return new ApiResponseDTO<T>
            {
                Success = false,
                Message = "Operation failed",
                Errors = errors
            };
        }
    }
}
