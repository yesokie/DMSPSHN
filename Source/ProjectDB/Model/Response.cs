
namespace ProjectDb.Models
{
    public class Response
    {
        public const int SUCCESS = 0;
        public const int FAILURE = 1;
        public const int NOT_AUTHORIZE = 2;
        public const int EXISTED = 3;
        public const int NOT_EXISTED = 4;
        public Response()
        {
            Code = SUCCESS;//success
        }
        public Response(int code, string message)
        {
            Code = code;
            Message = message;
        }
        public Response(int code,string message,object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }
        public string Message { get; set; }
        public int Code { get; set; }//more detail error
        public object Data { get; set; }//any type
    }
}
