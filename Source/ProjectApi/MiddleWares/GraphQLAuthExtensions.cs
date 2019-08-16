using GraphQL.Validation;
using GraphQLAuthorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.MiddleWares
{
    public static class GraphQLAuthExtensions
    {
        public static void AddGraphQLAuth(this IServiceCollection services, Action<AuthorizationSettings> configure)
        {
            //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();

            services.TryAddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();
                configure(authSettings);
                return authSettings;
            });
        }
    }
}
