using GraphQL.Types;
using System.Collections.Generic;
using CRMGraphQL;


namespace ProjectApi.Mutations.AdminMutation
{
    public class GraphQLAdminMutation:BaseGraphType
    {
        public GraphQLAdminMutation(IEnumerable<IAdminMutation> graphMutationMarkers)
        {
            Name = "AdminMutation";
            foreach (var marker in graphMutationMarkers)
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
