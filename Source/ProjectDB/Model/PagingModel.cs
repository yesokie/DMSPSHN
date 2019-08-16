
namespace ProjectDb.Models
{
    public class PagingModel:Response
    {
        //public object Data { get; set; }
        public PagingModel() : base() { }
        public PagingModel(int code, string message) : base(code, message) { }
        public int Pages { get; set; }
        public int Page { get; set; }//currentpage
        public int Records { get; set; }

    }
}
