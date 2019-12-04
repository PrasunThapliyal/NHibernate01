
namespace ORM_NHibernate.BusinessObjects
{
    using System;

    public abstract class EntityBase
    {
        public virtual int Id { get; protected set; }
    }
}
