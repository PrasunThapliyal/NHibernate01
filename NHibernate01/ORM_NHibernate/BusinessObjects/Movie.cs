﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ORM_NHibernate.BusinessObjects
{
    public class Movie: Product
    {
        public virtual string Director { get; set; }
        public virtual IList<ActorRole> Actors { get; set; }
        public virtual string NewProp { get; set; }

        public override sbyte Discriminator { get; set; } = 1;
    }
}
