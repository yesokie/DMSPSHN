using MongoDB.Bson.Serialization.Attributes;
using MongoUtil;
using MongoUtil.Procedures;
using ProjectDB.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProjectDB.Model
{
    [BsonIgnoreExtraElements]
    public class EventLog:MongoIdentifiable
    {
        public static void CreateEvent(BaseEvent data)
        {
            EventLog evt = new EventLog() {
                Data = data
            };
            BaseProcs.Add(evt).Wait();
        }
        protected DateTime _CreatedTime;
        public DateTime CreatedTime
        {
            get
            {
                return _CreatedTime < new DateTime(2019, 1, 1) ? DateTime.Now : _CreatedTime;
            }
            set
            {
                _CreatedTime = value;
            }
        }
        protected string _Type;
        public string Type {
            get{
                return  _Type.IsNullOrEmpty()?Data.GetType().Name:_Type;
            }
            set
            {
                _Type = value;
            }
        }
        protected string _Message;
        [Description("Row text Message build by event")]
        public string Message {
            get
            {
                try
                {
                    return _Message.IsNullOrEmpty() ? ((BaseEvent)Data).GetMessage() : _Message;
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                _Message = value;
            }
        }
        public object Data { get; set; }
    }

    #region Định nghĩa cac loại eventData
    [Description("Đối tượng event cơ bản chứa 1 thành phần cơ bản nào đó")]
    abstract public class BaseEvent
    {
        public BaseEvent(string _activator,string _activatorId)
        {
            Activator = _activator;
            ActivatorId = _activatorId;
        }
        [Description("Tên của người thực hiện")]
        public string Activator { get; set; }
        [Description("Id account thực hiện")]
        public string ActivatorId { get; set; }
        public abstract string GetMessage();
    }
    
    [BsonIgnoreExtraElements]
    public class CreateDirectoryEvent : BaseEvent
    {
        public CreateDirectoryEvent(string _activator, string _activatorId,DocDirectory _dir):base(_activator,_activatorId)
        {
            Dir = _dir;
        }
        [Description("đối tượng thư mục")]
        public DocDirectory Dir { get; set; }
        
        public override string GetMessage()
        {
            return $"{Activator} đã tạo thư mục {Dir.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class DeleteDirectoryEvent : CreateDirectoryEvent
    {
        public DeleteDirectoryEvent(string _activator, string _activatorId, DocDirectory _dir) : base(_activator,_activatorId,_dir) { }
        public override string GetMessage()
        {
            return $"{Activator} đã xóa thư mục {Dir.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateDirectoryEvent : CreateDirectoryEvent
    {
        public UpdateDirectoryEvent(string _activator, string _activatorId, DocDirectory _dir,DocDirectory _origin) : base(_activator,_activatorId, _dir)
        {
            Origin = _origin;
        }
        [Description("đối tượng thư mục")]
        public DocDirectory Origin { get; set; }
        public override string GetMessage()
        {
            if(Origin==null)
                return $"{Activator} đã sửa thư mục {Dir.Name}";
            return "";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateDirectoryNameEvent : CreateDirectoryEvent
    {
        public UpdateDirectoryNameEvent(string _activator, string _activatorId, DocDirectory _dir, string _origin) : base(_activator,_activatorId, _dir)
        {
            Origin = _origin;
        }
        public string Origin { get; set; }
        public override string GetMessage()
        {
            if (Origin == null)
                return $"{Activator} đã đổi tên thư mục {Origin} thành {Dir.Name}";
            return "";
        }
    }
    [BsonIgnoreExtraElements]
    public class CreateFileEvent : BaseEvent
    {
        public CreateFileEvent(string _activator, string _activatorId, DocFile _file):base(_activator,_activatorId)
        {
            File = _file;
        }
        public DocFile File { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã tạo tài liệu {File.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateFileEvent : CreateFileEvent
    {
        public UpdateFileEvent(string _activator, string _activatorId, DocFile _file,DocFile _origin) : base(_activator,_activatorId, _file)
        {
            Origin = _origin;
        }
        public DocFile Origin { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã chỉnh sửa tài liệu {File.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateFileNameEvent : CreateFileEvent
    {
        public UpdateFileNameEvent(string _activator, string _activatorId, DocFile _file, string _origin) : base(_activator,_activatorId, _file)
        {
            Origin = _origin;
        }
        public string Origin { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã đổi tên tài liệu {Origin} thành {File.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class DeleteFileEvent : CreateFileEvent
    {
        public DeleteFileEvent(string _activator, string _activatorId, DocFile _file) : base(_activator,_activatorId, _file) { }
        public override string GetMessage()
        {
            return $"{Activator} đã xóa tài liệu {File.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class CreateFileVersionEvent : BaseEvent
    {
        public CreateFileVersionEvent(string _activator, string _activatorId, DocFile _file,FileVersion _version):base(_activator,_activatorId)
        {
            File = _file;
            FileVersion = _version;
        }
        public DocFile File { get; set; }
        public FileVersion FileVersion { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã tạo phiên bản {FileVersion.Name} cho tài liệu {File.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateFileVersionEvent : CreateFileVersionEvent
    {
        public UpdateFileVersionEvent(string _activator, string _activatorId, DocFile _file, FileVersion _version,FileVersion _origin) : base(_activator,_activatorId, _file, _version) {
            Origin = _origin;
        }
        public FileVersion Origin { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã chỉnh sửa phiên bản {FileVersion.Name} thuộc tài liệu {File.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateFileVersionNameEvent : CreateFileVersionEvent
    {
        public UpdateFileVersionNameEvent(string _activator, string _activatorId, DocFile _file, FileVersion _version, string _origin) : base(_activator,_activatorId, _file, _version)
        {
            Origin = _origin;
        }
        public string Origin { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã đổi tên phiên bản {Origin} thành {FileVersion.Name} thuộc tài liệu {File.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class DeleteFileVersionEvent : CreateFileVersionEvent
    {
        public DeleteFileVersionEvent(string _activator, string _activatorId, DocFile _file, FileVersion _version) : base(_activator,_activatorId, _file, _version)
        {
        }
        public override string GetMessage()
        {
            return $"{Activator} đã xóa phiên bản {FileVersion.Name} thuộc tài liệu {File.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class CreateShareDocEvent : BaseEvent
    {
        public CreateShareDocEvent(string _activator, string _activatorId, Document _document,DocShare _share):base(_activator,_activatorId)
        {
            Doc = _document;
            Share = _share;
        }
        public Document Doc { get; set; }
        public DocShare Share { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã chia sẻ {(Doc.Type==Document.TYPE_FILE?"Tài liệu":"Thư mục")} {Doc.Name} {(Share.Type==DocShare.TYPE_GROUP?"tới nhóm":Share.Type==DocShare.TYPE_MEMBER?"tới thành viên":"công khai link:")} {Share.To}";
        }
    }
    [BsonIgnoreExtraElements]
    public class DeleteShareDocEvent : CreateShareDocEvent
    {
        public DeleteShareDocEvent(string _activator, string _activatorId, Document _document, DocShare _share) : base(_activator,_activatorId, _document,_share) { }
        public override string GetMessage()
        {
            return $"{Activator} đã hủy chia sẻ {(Doc.Type == Document.TYPE_FILE ? "Tài liệu" : "Thư mục")} {Doc.Name} {(Share.Type == DocShare.TYPE_GROUP ? "tới nhóm" : Share.Type == DocShare.TYPE_MEMBER ? "tới thành viên" : "công khai link:")} {Share.To}";
        }
    }
    [BsonIgnoreExtraElements]
    public class CreateGroupEvent : BaseEvent
    {
        public CreateGroupEvent(string _activator, string _activatorId, Group _group):base(_activator,_activatorId)
        {
            Group = _group;
        }
        public Group Group { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã tạo nhóm {Group.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateGroupEvent : CreateGroupEvent
    {
        public UpdateGroupEvent(string _activator, string _activatorId, Group _group,Group _origin) : base(_activator,_activatorId, _group)
        {
            Origin = _origin;
        }
        public Group Origin { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã sửa nhóm {Group.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateGroupNameEvent : CreateGroupEvent
    {
        public UpdateGroupNameEvent(string _activator, string _activatorId, Group _group, string _origin) : base(_activator,_activatorId, _group)
        {
            Origin = _origin;
        }
        public string Origin { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã đổi tên nhóm {Origin} thành {Group.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class DeleteGroupEvent : CreateGroupEvent
    {
        public DeleteGroupEvent(string _activator, string _activatorId, Group _group) : base(_activator,_activatorId, _group) { }
        public override string GetMessage()
        {
            return $"{Activator} đã xóa nhóm {Group.Name}";
        }
    }
    [BsonIgnoreExtraElements]
    public class CreateAccountEvent : BaseEvent
    {
        public CreateAccountEvent(string _activator, string _activatorId, Account _accont):base(_activator,_activatorId)
        {
            Account = _accont;
        }
        public override string GetMessage()
        {
            return $"{Activator} đã tạo tài khoản {Account.UserName}";
        }
        public Account Account { get; set; }
    }
    [BsonIgnoreExtraElements]
    public class UpdateAccountEvent : CreateAccountEvent
    {
        public UpdateAccountEvent(string _activator, string _activatorId, Account _account,Account _origin) : base(_activator,_activatorId, _account) {
            Origin = _origin;
        }
        public Account Origin { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã cập nhật tài khoản {Account.UserName}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateAccountNameEvent : CreateAccountEvent
    {
        public UpdateAccountNameEvent(string _activator, string _activatorId, Account _account, string _origin) : base(_activator,_activatorId, _account)
        {
            Origin = _origin;
        }
        public string Origin { get; set; }
        public override string GetMessage()
        {
            return $"{Activator} đã đổi tên tài khoản {Origin} thành {Account.UserName}";
        }
    }
    [BsonIgnoreExtraElements]
    public class UpdateAccountPasswordEvent : CreateAccountEvent
    {
        public UpdateAccountPasswordEvent(string _activator, string _activatorId, Account _account) : base(_activator,_activatorId, _account)
        {
        }
        public override string GetMessage()
        {
            return $"{Activator} đã đổi mật khẩu tài khoản {Account.UserName}";
        }
    }
    [BsonIgnoreExtraElements]
    public class DeleteAccountEvent : CreateAccountEvent
    {
        public DeleteAccountEvent(string _activator, string _activatorId, Account _account) : base(_activator,_activatorId, _account)
        {
        }
        public override string GetMessage()
        {
            return $"{Activator} đã xóa tài khoản {Account.UserName}";
        }
    }
    #endregion
}
