using ProjectDB.Model;

namespace ProjectDB.Entity
{
    public class DocDirectory:Document
    {
        public string Type
        {
            get
            {
                return TYPE_DIRECTORY;
            }
        }
    }
}
