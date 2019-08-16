using CommonLib.Cache;
using CommonLib.Config;
using MongoDB.Bson.Serialization;
using MongoLog;
using MongoUtil;
using MongoUtil.Extentions;
using MongoUtil.Procedures;
using ProjectDB;
using ProjectDB.Model;
using System;
using System.Globalization;

namespace ThanhThuyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadConfig();
            CreateDirEvent();
            var data = BaseProcs.mongo.GetAll<EventLog>();
           
        }
        private static void CreateDirEvent()
        {
            
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
            CultureInfo.CurrentCulture = new CultureInfo("vi-VN");
        }
    }
}
