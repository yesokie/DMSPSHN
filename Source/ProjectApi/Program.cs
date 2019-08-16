using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommonLib.Cache;
using CommonLib.Config;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization;
using MongoLog;
using MongoUtil;
using MongoUtil.Extentions;
using MongoUtil.Procedures;
using ProjectApi;
using ProjectApi.Models;
using ProjectDB;

namespace ProjectApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LoadConfig();

            CreateWebHostBuilder(args).Build().Run();
        }

        private static void LoadConfig()
        {
            ConfigurationHelper.Init();
            RedisConnection.Init(ConfigurationHelper.GetInteger("Redis:Db"), ConfigurationHelper.GetString("Redis:Host"), ConfigurationHelper.GetInteger("Redis:Port"), ConfigurationHelper.GetString("Redis:PassWd"));
            BsonSerializer.RegisterSerializer(typeof(DateTime), new MyMongoDBDateTimeSerializer());
            LogHelper.Init(ConfigurationHelper.GetString("Mongo:ConnectionString"), ConfigurationHelper.GetString("Mongo:DatabaseName"), ConfigurationHelper.GetBoolean("Mongo:IsReplicaSet"), ConfigurationHelper.GetString("Mongo:ReplicaSetName"));
            BaseProcs.Init(MongoConnect.GetHelper(ConfigurationHelper.GetString("Mongo:ConnectionString"), ConfigurationHelper.GetString("Mongo:DatabaseName"), 10, ConfigurationHelper.GetBoolean("Mongo:IsReplicaSet"), ConfigurationHelper.GetString("Mongo:ReplicaSetName"))).Wait();
            DatabaseInit.Init();
            //AccountProcs.Init(MongoConnect.GetHelper(ConfigurationHelper.GetString("AccountDb:ConnectionString"), ConfigurationHelper.GetString("AccountDb:DatabaseName"), 10, ConfigurationHelper.GetBoolean("AccountDb:IsReplicaSet"), ConfigurationHelper.GetString("AccountDb:ReplicaSetName"))).Wait();
            RedisConnection.SetModelConfig<LoginSession>(a => a.AccessToken);
            CultureInfo.CurrentCulture = new CultureInfo("vi-VN");
        }
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = 52428800; //50MB
                })
                .UseIISIntegration()

                .UseUrls("http://0.0.0.0:" + ConfigurationHelper.GetString("Port", "5000"));
    }
}
