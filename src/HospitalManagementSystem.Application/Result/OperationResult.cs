namespace HospitalManagementSystem.Application.Results
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public OperationResult()
        {
            Errors = new List<string>();
        }

        public static OperationResult<T> Success(T data, string message = "Operation completed successfully")
        {
            return new OperationResult<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        public static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T>
            {
                IsSuccess = false,
                Message = message
            };
        }

        public static OperationResult<T> Failure(List<string> errors)
        {
            return new OperationResult<T>
            {
                IsSuccess = false,
                Message = "Operation failed",
                Errors = errors
            };
        }
    }
}
