using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Models
{
    public class GraphQLRequest
    {
        public string OperationName { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}
