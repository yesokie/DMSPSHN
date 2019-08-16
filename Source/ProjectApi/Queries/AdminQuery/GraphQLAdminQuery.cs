using GraphQL.Types;
using System.Collections.Generic;

namespace ProjectApi.Queries.AdminQuery
{
    public class GraphQLAdminQuery : ObjectGraphType<object>
    {
        
        public GraphQLAdminQuery(IEnumerable<IAdminQuery> graphQueryMarkers)
        {
            Name = "AdminQuery";
            foreach (var marker in graphQueryMarkers)
            {
                var q = marker as ObjectGraphType<object>;
                foreach (var f in q.Fields)
                {
                    AddField(f);
                }
            }
        }
    }
}
