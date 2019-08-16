using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel;

namespace ProjectDB.Entity
{
    [Description("Quy định chia sẻ 1 tài liệu đến 1 đối tượng")]
    public class DocShare
    {
        public const string TYPE_PUBLIC = "public";
        public const string TYPE_MEMBER = "member";
        public const string TYPE_GROUP = "group";

        [Description("Loại chia sẻ là cho cộng đồng,1 member hay 1 group")]
        public string Type { get; set; }
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [Description("đối tượng được chia sẻ vd: với nhóm thì dùng trường Name, còn với Account thì dùng UserName")]
        public string To { get; set; }
        [Description("Đối tượng chia sẻ được quyền gì")]
        public FilePermission Permission { get; set; }
    }
}
