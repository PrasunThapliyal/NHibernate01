
namespace ORM_NHibernate.BusinessObjects
{
    public class Product : EntityBase
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual decimal UnitPrice { get; set; }
    }
}
