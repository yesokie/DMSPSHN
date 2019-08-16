using System.ComponentModel;
using ProjectDB.Model;

namespace ProjectDB.Entity
{
    [Description("node cuối cùng của cây tài liệu")]
    public class DocFile:Document
    {
        [Description("Đuôi mở rộng của file")]
        public string Extension { get; set; }
        
        public override string Type
        {
            get
            {
                return TYPE_FILE;
            }
        }
    }
}
