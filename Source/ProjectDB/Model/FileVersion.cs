using MongoDB.Bson.Serialization.Attributes;
using MongoUtil.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProjectDB.Model
{
    [BsonIgnoreExtraElements]
    [Description("Đại diện cho phiên bản của file với Name=tên version")]
    public class FileVersion:BaseMongoModelWithTime
    {
        [Description("Mô tả những thay đổi")]
        public string Note { get; set; }

    }
}
