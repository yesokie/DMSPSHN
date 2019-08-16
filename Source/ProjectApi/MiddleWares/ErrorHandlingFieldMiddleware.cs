using APModel.Models.Log;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.AspNetCore.Http;
using MongoLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.MiddleWares
{
    public class ErrorHandlingFieldMiddleware
    {
        public async Task<object> Resolve(
           ResolveFieldContext context,
           FieldMiddlewareDelegate next)
        {
            var metadata = new Dictionary<string, object>
                {
                    { "typeName", context.ParentType.Name },
                    { "fieldName", context.FieldName },
                    { "path", context.Path },
                    { "arguments", context.Arguments },
                };
            var path = $"{context.ParentType.Name}.{context.FieldName}";
            try
            {
                

                using (context.Metrics.Subject("field", path, metadata))
                {
                    return await next(context).ConfigureAwait(false);
                }
                //return await next(context);
            }
            catch (Exception e)
            {
                ErrorLog log = new ErrorLog()
                {
                    time = DateTime.Now,
                    action = path,
                    log_level = "alert",
                    api_message = new ErrorLog.ApiMessage() { request=JsonConvert.SerializeObject(context.Arguments)},
                    project_name = "log"
                };

                log.error_message = e.ToString();
                //Lay lai cai goc;
                try
                {
                    await LogHelper.AddLogAsync(log);
                }catch(Exception ex)
                {

                }
                throw;
            }
        }
    }
}
