using GraphQL.Types;
using MongoUtil.Types;
using ProjectDB.Model;

namespace ProjectDB.Types
{
    public class DocumentType:BaseType<Document>
    {
        public DocumentType()
        {
            Field(x => x.Shared, true, typeof(ListGraphType<DocShareType>));
        }
    }

    public class DocumentInput : BaseInputType<Document> { }
}
