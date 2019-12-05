using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_NHibernate.BusinessObjects
{
    public class ActorRole: EntityBase
    {
        public virtual string Actor { get; set; }
        public virtual string Role { get; set; }
    }
}
