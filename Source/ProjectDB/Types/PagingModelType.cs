using GraphQL.Types;
using ProjectDb.Models;

namespace ProjectDb.Types
{
    public class PagingModelType<TGraphType>:ObjectGraphType<PagingModel> where TGraphType : GraphType
    {
        public PagingModelType()
        {
            Name = $"Paging{typeof(TGraphType).Name}";
            Field(x => x.Page);
            Field(x => x.Pages);
            Field<ListGraphType<TGraphType>>("Data");
            Field(x => x.Code);
            Field(x => x.Message,true);
            Field(x => x.Records);
        }
    }
}
