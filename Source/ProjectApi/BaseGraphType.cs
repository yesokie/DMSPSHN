using CRMGraphQL.Models;
using GraphQL.Types;
using ProjectApi.Models;
using ProjectDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi
{
    public class BaseGraphType:ObjectGraphType
    {
        public Response Success(object data,string message="Thành công")
        {
            if(data==null)
                return new Response()
                {
                    Data = data,
                    Message = message
                };
            string name = data.GetType().Name;
            if (name.StartsWith("List"))
            {
                return new PagingModel() { Data = data,Message=message };
            }
            return new Response()
            {
                Data = data,Message=message
            };
        }
        public Response Fail(string message, int code = Response.FAILURE,object data=null)
        {
            return new Response(code, message,data);
        }
        public PagingModel FailPaging(string message,int code = Response.FAILURE,object data=null)
        {
            return new PagingModel(code, message)
            {
                Data=data
            };
        }
    }
}
