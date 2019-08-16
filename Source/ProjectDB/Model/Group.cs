using MongoDB.Bson.Serialization.Attributes;
using MongoUtil;
using MongoUtil.Models;
using System;
using System.ComponentModel;

namespace ProjectDB.Model
{
    [BsonIgnoreExtraElements]
    [Description("Nhóm được hiểu như phòng/ban")]
    public class Group
    {
        [Description("Tên nhóm nhưng đồng thời là Id luôn")]
        [BsonId]
        public string Name { get; set; }
        [Description("Tạo bởi ai")]
        [DesignOnly(false)]
        public BaseMongoModel Creator { get; set; }
        [Description("Chỉnh sửa lần cuối bởi")]
        [DesignOnly(false)]
        public BaseMongoModel Updator { get; set; }
        protected DateTime _CreatedTime;
        protected DateTime _UpdatedTime;
        [DesignOnly(false)]
        public DateTime CreatedTime
        {
            get
            {
                if (_CreatedTime < new DateTime(2018, 1, 1))
                    _CreatedTime = DateTime.Now;
                return _CreatedTime;
            }
            set
            {
                _CreatedTime = value;
            }
        }
        [DesignOnly(false)]
        public DateTime UpdatedTime
        {
            get
            {
                if (_UpdatedTime < new DateTime(2018, 1, 1))
                    _UpdatedTime = DateTime.Now;
                return _UpdatedTime;
            }
            set
            {
                _UpdatedTime = value;
            }
        }
    }
}
