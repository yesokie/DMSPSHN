using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMGraphQL.Models
{
    public class CRMError: GraphQL.ExecutionError
    {
        public CRMError(string message) : base(message) { }
    }
}
