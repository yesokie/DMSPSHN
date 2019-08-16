using CommonLib.Cache;
using GraphQL.Types;
using GraphQLAuthorization;
using Microsoft.AspNetCore.Http;
using MongoUtil.Procedures;
using Newtonsoft.Json;
using ProjectApi.Models;
using ProjectApi.Types;
using ProjectDb.Types;
using ProjectDB.Model;
using ProjectDB.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Mutations.AdminMutation
{
    public class TestMutation:BaseGraphType,IAdminMutation
    {
        public TestMutation(IHttpContextAccessor httpContext)
        {
            FieldAsync<ResponseType<GroupType>>("addAnimal",
                arguments:new QueryArguments(new QueryArgument<GroupInput> { Name = "animal" }),
                resolve:async context =>
                {
                    Group animal = context.GetArgument<Group>("animal");
                    await BaseProcs.Add(animal);
                    return Success(animal);
                }
            );

            FieldAsync<ResponseType<GroupType>>("updateAnimal",
                description:"Cập nhật animal yêu cầu quyền của người đã login",
                arguments: new QueryArguments(new QueryArgument<GroupInput> { Name = "animal" }),
                resolve: async context =>
                {
                    var user = LoginSession.Current(httpContext.HttpContext);
                    Group animal = context.GetArgument<Group>("animal");
                    await BaseProcs.Update(animal);
                    return Success(animal,$"bạn đã update thành công bằng tài khoản {user.UserName}");
                }
            ).FieldAuthorizeWithName(Policies.BASE_ACCOUNT_POLICY);

            FieldAsync<ResponseType<LoginSessionType>>("login",
                arguments:new QueryArguments(new QueryArgument<NonNullGraphType<StringGraphType>> { Name="username"},
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name="password"}),
                resolve:async context =>
                {
                    string username = context.GetArgument<string>("username");
                    string password = context.GetArgument<string>("password");
                    ///giả thiết check database thành công thì lưu vào session để các query sau hiểu là thằng này đã đăng nhập
                    ///module Authentication sẽ tự động check header Authorization của client gửi lên và truy vấn các cache này để quyết định 
                    ///1 user có đang login hay không 
                    ///
                    RedisConnection cache = new RedisConnection();
                    LoginSession session = new LoginSession()
                    {
                        FullName = "Register to input this field",
                        AccessToken = Guid.NewGuid().ToString("N"),
                        CreateOn = DateTime.Now,
                        ExpireOn = DateTime.Now.AddHours(2),
                        UserName=username,

                    };
                    //lưu session=redis (cache = database)
                    await cache.StoreAsync(session, TimeSpan.FromDays(2));//for test i will let the time long
                    var HttpContext = httpContext.HttpContext;
                    //cache bằng session mặc định của dotnet
                    HttpContext.Session.SetString("token", JsonConvert.SerializeObject(session));
                    return Success(session);
                }
            );
        }
    }
}
