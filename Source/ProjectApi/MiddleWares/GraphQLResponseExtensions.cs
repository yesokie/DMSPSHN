using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using ProjectDb.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.MiddleWares
{
    public static class GraphQLResponseExtensions
    {
        // <summary>
        /// hàm này sẽ dùng làm hàm chuẩn để đỡ phải khai báo nhiều lần
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        public static void AddGraphQLResponse<T>(this IServiceCollection services) where T : GraphType
        {
            if (services.Where(s => s.ImplementationType == typeof(T)).Count() == 0)
            {
                services.AddSingleton<T>();
            }
            services.AddSingleton<ResponseType<T>>();
            services.AddSingleton<PagingModelType<T>>();
        }
        
        public static void AddGraphQLResponseOnly<T>(this IServiceCollection services) where T : GraphType
        {
            //var check = services.Where(s => s.ImplementationType == typeof(T)).Count();
            if (services.Where(s => s.ImplementationType == typeof(T)).Count() == 0)
            {
                services.AddSingleton<T>();
            }
            services.AddSingleton<ResponseType<T>>();
            //var last = services[services.Count()-1];
        }
        public static void AddGraphQLPagingOnly<T>(this IServiceCollection services) where T : GraphType
        {
            if (services.Where(s => s.ImplementationType == typeof(T)).Count() == 0)
            {
                services.AddSingleton<T>();
            }
            services.AddSingleton<PagingModelType<T>>();
        }
    }
}
