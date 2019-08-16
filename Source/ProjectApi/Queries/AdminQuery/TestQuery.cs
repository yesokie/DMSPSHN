using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using MongoUtil.Models;
using MongoUtil.Procedures;
using MongoUtil.Types;
using ProjectDb.Models;
using ProjectDb.Types;
using ProjectDB.Model;
using ProjectDB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Queries.AdminQuery
{
    public class TestQuery:BaseGraphType,IAdminQuery
    {
        public TestQuery(IHttpContextAccessor httpContext)
        {
            FieldAsync<ResponseType<GroupType>>("getAnimal",
                description:"lấy nội dung animal theo _id truyền vào",
                arguments:new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "_id" }),
                resolve:async contex =>
                {
                    string _id = contex.GetArgument<string>("_id");
                    var animal = await BaseProcs.Get<Group>(_id);
                    return Success(animal);
                }
                );
            Field<PagingModelType<GroupType>>("listAnimal",
                description:"Danh sách animal có phân trang",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "page" }, new QueryArgument<IntGraphType> { Name = "pageSize" }, new QueryArgument<ListGraphType<SortedInputType>> { Name = "sorted" }, new QueryArgument<ListGraphType<FilteredInputType>> { Name = "filtered" }),
                resolve: context =>
                {
                    List<QueryFilter> filters = context.GetArgument("filtered", new List<QueryFilter>());
                    int page = context.GetArgument("page", 0);//page nay bat dau tu 0
                    int pageSize = context.GetArgument("pageSize", 10);
                    int skip = (page * pageSize);
                    List<QueryOrder> order = context.GetArgument("sorted", new List<QueryOrder>());
                    int pages, records;
                    var data = BaseProcs.Search<Group>(filters, skip, pageSize, order, out pages, out records);
                    return new PagingModel() { Data = data, Pages = pages, Page = page, Records = records };
                }
                );
        }
    }
}
