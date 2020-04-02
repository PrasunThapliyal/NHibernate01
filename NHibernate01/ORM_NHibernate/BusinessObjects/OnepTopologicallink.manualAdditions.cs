
namespace ORM_NHibernate.BusinessObjects
{
    using System;
    using System.Collections.Generic;

	public partial class OnepTopologicallink : BusinessBase<uint>
	{
		static OnepTopologicallink() { }
		public virtual sbyte Discriminator { get; set; } = 1;

		public override string ToString()
		{
			return String.Format("TL {0}", this.Name);
		}

	}
}