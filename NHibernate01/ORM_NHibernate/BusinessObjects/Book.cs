
namespace ORM_NHibernate.BusinessObjects
{
    public class Book : Product 
    { 
        public virtual string ISBN { get; set; } 
        public virtual string Author { get; set; }

        public override sbyte Discriminator { get; set; } = 2;
    }
}
