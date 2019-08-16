
using GraphQL;
using GraphQL.Types;
using ProjectApi.Mutations.AdminMutation;
using ProjectApi.Queries.AdminQuery;

namespace ProjectApi.Schemes
{
    public class GQLSchema:Schema
    {
        public GQLSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<GraphQLAdminQuery>();
            Mutation = resolver.Resolve<GraphQLAdminMutation>();
            //Subscription = resolver.Resolve<GraphQLSubscription>();

        }
    }
}
