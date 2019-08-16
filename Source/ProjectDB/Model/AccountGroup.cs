using MongoDB.Bson.Serialization.Attributes;
using MongoUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProjectDB.Model
{
    [BsonIgnoreExtraElements]
    [Description("Thiết lập 1 account thuốc 1 nhóm")]
    public class AccountGroup:MongoIdentifiable
    {
        [Description("Id tài khoản")]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string AccountId { get; set; }
        [Description("nhóm")]
        public string Group { get; set; }
        [Description("Quyền admin của nhóm --> cái này có nhiều đặc quyền đặc lợi khác mà các quyền đọc ghi file không có")]
        public bool IsAdmin { get; set; }
    }
}
