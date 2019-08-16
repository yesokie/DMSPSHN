using GraphQL.Types;
using ProjectDb.Models;

namespace ProjectDb.Types
{
    public class ResponseType<TGraphType> :ObjectGraphType<Response> where TGraphType:GraphType
    {
        public ResponseType()
        {
            Name = $"Response{typeof(TGraphType).Name}";
            Field(x => x.Code);
            Field(x => x.Message, true);
            Field<TGraphType>("Data");
        }
    }
}
