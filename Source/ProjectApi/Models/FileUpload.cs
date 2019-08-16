using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMGraphQL.Models
{
    public class FileUpload
    {
        public string id { get; set; }
        public string filename { get; set; }
        public string minetype { get; set; }
        public string path { get; set; }
    }
}
