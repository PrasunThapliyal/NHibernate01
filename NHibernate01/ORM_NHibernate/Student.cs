
namespace ORM_NHibernate
{
    using System;

    public class Student
    {
        public virtual int Id { get; set; }
        public virtual string Firstname { get; set; }
        public virtual string Lastname { get; set; }
    }
}
