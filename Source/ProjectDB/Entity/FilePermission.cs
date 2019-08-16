using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ProjectDB.Entity
{
    [Description("Định nghĩa quyền được thao tác với tài liệu")]
    public class FilePermission
    {
        [Description("quyền được tạo document")]
        public bool Write { get; set; }
        [Description("Quyền được chỉnh sửa tài liệu vd: đổi tên,...")]
        public bool Edit { get; set; }
        [Description("Quyền được xóa document")]
        public bool Delete { get; set; }
    }
}
