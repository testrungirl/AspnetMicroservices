
namespace Rebate.GRPC.Data
{
    public class GenericResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

}
