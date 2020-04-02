using System;

namespace ORM_NHibernate.BusinessObjects
{
    public partial class OnepTerminationpoint : BusinessBase<uint>
    {
        private OnepAmptp _onepAmp = null;

        static OnepTerminationpoint()
        {
        }

        public override string ToString()
        {
            return String.Format("TP {0}", this.Name);
        }

        public override string NaturalName
        {
            get
            {
                return this.Name;
            }
        }

        public virtual OnepAmptp OnepAmpRole { get => _onepAmp; set => _onepAmp = value; }
    }
}