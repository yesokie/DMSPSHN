using MongoUtil.Types;
using ProjectDB.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectDB.Types
{
    public class FileVersionType:BaseType<FileVersion>
    {
    }
    public class FileVersionInput : BaseInputType<FileVersion> { }
}
