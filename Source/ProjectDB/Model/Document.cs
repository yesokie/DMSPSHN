using MongoDB.Bson.Serialization.Attributes;
using MongoUtil.Models;
using ProjectDB.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProjectDB.Model
{
    [BsonIgnoreExtraElements]
    [Description("Đối tượng tài liệu của hệ thống")]
    public class Document:BaseMongoModelWithTime
    {
        public const string TYPE_FILE = "file";//default value
        public const string TYPE_DIRECTORY = "directory";

        protected string _ParentPath;
        [Description("Đường dẫn thư mục cha")]
        public string ParentPath {
            get
            {
                return _ParentPath.IsNullOrEmpty() ? "" : _ParentPath;
            }
            set
            {
                _ParentPath = value;
            }
        }
        
        protected string _unsignName;
        [Description("Tăng cường bộ lọc tìm kiếm với trường này nhưng không cần hiển thị")]
        [DesignOnly(true)]
        public string UnsignName
        {
            get
            {
                return _unsignName.IsNullOrEmpty() ? Name.ToUnsign() : _unsignName;
            }
            set
            {
                _unsignName = value;
            }
        }
        [Description("Đường dẫn tương đối của tài liệu, tùy thuộc vào tài khoản sở hữu --> chuỗi này có thể dùng để tra cứu")]
        public string Path
        {
            get
            {
                return ParentPath + "/" + _id;
            }
        }   
        
        [Description("Tạo bởi ai")]
        [DesignOnly(false)]
        public BaseMongoModel Creator { get; set; }
        [Description("Chỉnh sửa lần cuối bởi")]
        [DesignOnly(false)]
        public BaseMongoModel Updator { get; set; }
        public virtual string Type { get; set; }
        protected int _Priority;
        [Description("Mức độ ưu tiên của tài liệu, càng nhỏ càng lên đầu, giá trị nhỏ nhất bắt đầu từ 1")]
        public int Priority {
            get
            {
                return _Priority < 1 ? 1 : _Priority;
            }
            set { _Priority = value; }
        }
        [Description("Ẩn tài liệu này")]
        public bool Hidden { get; set; }
        protected List<DocShare> _Shared;
        [Description("Danh sách các đối tượng được chia sẻ")]
        public List<DocShare> Shared
        {
            get
            {
                return _Shared == null ? new List<DocShare>() : _Shared;
            }
            set
            {
                _Shared = value;
            }
        }
    }
}
