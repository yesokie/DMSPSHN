using MongoUtil.Types;
using ProjectDB.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDB.Model
{
    public class FilePermissionType:BaseType<FilePermission>
    {
    }
    public class FilePermissionInput : BaseInputType<FilePermission> { }
}
