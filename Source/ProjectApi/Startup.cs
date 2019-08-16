using System;
using GraphQL;
using GraphQL.Http;
using GraphQLAuthorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using CommonLib.Config;
using ProjectApi.Queries.AdminQuery;
using ProjectApi.Mutations.AdminMutation;
using ProjectApi.Schemes;
using GraphQL.Server;
using ProjectApi.Models;
using GraphQL.Server.Internal;
using ProjectApi.MiddleWares;
using GraphQL.Server.Ui.Playground;
using GraphiQl;
using ProjectDB.Types;
using MongoUtil.Types;
using ProjectApi.Types;

namespace ProjectApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;

        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            if (Environment.IsProduction())
            {
            }
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_2);
            // configure DI for application services
            //services.AddTransient<IAccountService, AccountService>();
            //services.AddTransient<INotifyService, NotifyService>();
            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromHours(2);
            });
            services.AddDistributedMemoryCache();

            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();
            services.AddSingleton<GraphQLAdminQuery>();
            services.AddSingleton<GraphQLAdminMutation>();

            //----------------
            services.AddSingleton<IAdminMutation, TestMutation>();
            services.AddSingleton<IAdminQuery, TestQuery>();
            //---------------------TYPE------------------------

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<FilteredInputType>();
            services.AddSingleton<SortedInputType>();
            services.AddGraphQLResponse<GroupType>();//những thằng nào là Type thì dùng hàm này
            services.AddSingleton<GroupInput>();
            services.AddGraphQLResponse<LoginSessionType>();
            services.AddGraphQLResponse<BaseMongoModelType>();
            services.AddSingleton<BaseMongoModelInput>();
            //services.AddSingleton<ISchema, GQLSchema>();
            services.AddSingleton<GQLSchema>();
            //services.AddSingleton<CustomerSchema>();
            //add subscription for asterisk
            //services.AddSingleton<INotifyHandler, NotifyHandler>();
            //test


            services.AddGraphQL(options =>
            {
                options.ComplexityConfiguration = services
                           .BuildServiceProvider()
                           .GetRequiredService<IOptions<GraphQLOptions>>()
                           .Value
                           .ComplexityConfiguration;
                // Show stack traces in exceptions. Don't turn this on in production.
                options.ExposeExceptions = Environment.IsDevelopment();
            }).AddWebSockets()
            //.AddUserContextBuilder(context => AdminLoginSession.Current(context))
            .AddUserContextBuilder(context =>
            {
                return (AuthorizationUser)LoginSession.Current(context);
            })
            .AddDataLoader()
            .Services.AddTransient(typeof(IGraphQLExecuter<>), typeof(InstrumentingGraphQLExecutor<>));
            services.AddGraphQLAuth(_ =>
            {

                _.AddPolicy(Policies.ADMIN_POLICY, (AuthorizationPolicyBuilder p) =>
                {
                    p.RequireClaim("permission", Policies.PERMISSION_ADMIN);
                });
                _.AddPolicy(Policies.BASE_ACCOUNT_POLICY, (AuthorizationPolicyBuilder p) =>
                {
                });
                _.AddPolicy(Policies.ROOT_POLICY, (AuthorizationPolicyBuilder p) =>
                {
                    p.RequireClaim("permission", Policies.PERMISSION_ROOT);
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            //loggerFactory.AddConsole();
            //app.UseDeveloperExceptionPage();
            app.UseCors("CorsPolicy");
            app.UseDefaultFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                },
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });

            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            });
            app.UseMiddleware(typeof(MultipartMiddleware));
            //app.UseAuthentication();
            app.UseWebSockets();
            app.UseGraphQLWebSockets<GQLSchema>("/graphql");

            app.UseGraphQL<GQLSchema>("/graphql");
            //app.UseGraphQL<CustomerSchema>("/customer/graphql");
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions()
            {
                Path = "/ui/playground",
                GraphQLEndPoint = "/graphql"
            });
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions()
            {
                Path = "/user/ui/playground",
                GraphQLEndPoint = "/user/graphql"
            });
            app.UseGraphiQl("/user/ui/graphql", "/user/graphql");
            app.UseGraphiQl("/ui/graphql", "/graphql");

            app.UseStaticFiles(new StaticFileOptions()
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers.Add("Cache-Control", "no-cache, no-store");
                    context.Context.Response.Headers.Add("Expires", "-1");
                },

                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });


        }
    }
}
