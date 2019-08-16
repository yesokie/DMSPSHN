using APModel.Models.Log;
using GraphQL.Client;
using Microsoft.AspNetCore.Http;
using MongoLog;
using Newtonsoft.Json;
using ProjectApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ProjectApi.MiddleWares
{
    /// <summary>
    /// Middleware này dùng để xử lý file upload và đưa dữ liệu về dạng application/json để GraphQL xử lý tiếp
    /// </summary>
    public class MultipartMiddleware
    {
        private readonly RequestDelegate next;
        private readonly string jsonMediaType = "application/json";

        public MultipartMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                string contentType = context.Request.ContentType;
                if (contentType == null)
                {
                    await HandleShemaConf(context);                    
                }else
                if (!contentType.StartsWith(jsonMediaType))
                {
                    try
                    {
                        HandleMultipart(context);
                    }catch(Exception exx)
                    {
                        //error handle multypart
                        ErrorLog log = new ErrorLog()
                        {
                            time = DateTime.Now,
                            action = context.Request.Path + context.Request.QueryString,
                            log_level = "alert",
                            api_message = new ErrorLog.ApiMessage(),
                            project_name = "log",
                            ip = context.Connection.RemoteIpAddress.ToString()
                        };
                        await LogHelper.AddLogAsync(log);
                    }
                }
                await next(context);
            }
            catch (Exception ex)
            {
               await HandleExceptionAsync(context, ex);
            }
            
        }

        private void HandleMultipart(HttpContext context)
        {
            var encoder = JavaScriptEncoder.Create();
            var body = context.Request.Form["operations"];
            byte[] requestData = Encoding.UTF8.GetBytes(body);
            context.Request.Body = new MemoryStream(requestData);
            context.Request.ContentType = jsonMediaType;
        }
        private async Task HandleShemaConf(HttpContext context)
        {
            string path = context.Request.Path.Value;
            if (path.EndsWith("schema.json"))
            {
                //generate admin schema.conf for client
                var gqlRequest = new GraphQL.Common.Request.GraphQLRequest
                {
                    Query = @"{
                          __schema {
                            queryType { name }
                            types {
                                inputFields {
                                  name
                                  description
                                  type{
                                    name
                                    ofType{
                                      kind
                                      name
                                      ofType{
                                        kind
                                        name
                                        ofType {
                                          name
                                          description
                                        }
                                      }
                                    }
                                  }
                                  defaultValue
                                }
                                
                                name
                                description
                                interfaces {
                                  name
                                  description
                                }
                                enumValues {
                                  description
                                  deprecationReason
                                }
                                fields {
                                    name
                                    description
                                    type {
                                        name
                                        kind
                                    }
                                    isDeprecated
                                    deprecationReason
                                }
                                kind
                                possibleTypes {
                                  name
                                  description
                                }
                            }
                            mutationType { name }
                            directives {
                              name
                              description
                              locations
                              args {
                                name
                                description
                                type{
                                  kind
                                  name
                                  ofType{
                                    kind
                                    name
                                    ofType {
                                      name
                                      description
                                    }
                                  }
                                }
                                defaultValue
                              }
                            }
                          }
                        }",
                    OperationName = ""
                };
                string endpoint = context.Request.Scheme + "://" + context.Request.Host.ToString() + context.Request.Path.Value.Substring(0, path.Length - 11) + "graphql";
                GraphQLClient graphQLClient = new GraphQLClient(endpoint);
                var graphQLResponse = await graphQLClient.PostAsync(gqlRequest);
                string dataContent = JsonConvert.SerializeObject(new { data = graphQLResponse.Data });
                context.Response.ContentType = "application/octet-stream";
                await context.Response.WriteAsync(dataContent);

                return;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.OK; // 500 if unexpected
            ErrorLog log = new ErrorLog()
            {
                time = DateTime.Now,
                action = context.Request.Path + context.Request.QueryString,
                log_level = "alert",
                api_message = new ErrorLog.ApiMessage(),
                project_name = "log",
                ip = context.Connection.RemoteIpAddress.ToString()
            };
            //if (exception is MyNotFoundException) code = HttpStatusCode.NotFound;
            //else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (exception is MyException) code = HttpStatusCode.BadRequest;
            var token = LoginSession.Current(context);

            log.api_message = new ErrorLog.ApiMessage();
            if (token != null)
            {
                log.authorize_name = token.FullName + ":" + token.UserName;
            }
            var stream = context.Request.Body;
            log.error_message = exception.ToString();
            //Lay lai cai goc;
            context.Response.Body = stream;
            await LogHelper.AddLogAsync(log);
            var result = JsonConvert.SerializeObject(new { status = 1, msg = exception.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            await context.Response.WriteAsync(result);

        }
    }
}
