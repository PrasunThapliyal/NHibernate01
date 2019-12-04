
namespace ORM_NHibernate.BusinessObjects
{
    public class Student : EntityBase
    {
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
    }
}
