using MongoDB.Driver;
using MongoUtil;
using MongoUtil.Procedures;
using ProjectDB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDB
{
    public class DatabaseInit
    {
        public static void Init()
        {
            BaseProcs.RegisterOption(BaseProcs.UNIQUE_OPTION, BaseProcs.IndexBuilder<AccountGroup>().Ascending(a => a.AccountId).Ascending(a => a.Group)).Wait();
        }
    }
}
