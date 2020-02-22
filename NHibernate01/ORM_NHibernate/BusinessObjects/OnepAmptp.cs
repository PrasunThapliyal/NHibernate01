
namespace ORM_NHibernate.BusinessObjects
{
    public partial class OnepAmptp : BusinessBase<uint>
    {

		#region Declarations

		private double? _targetGain = null;

		#endregion



		#region Constructors

		public OnepAmptp() { }

		public OnepAmptp(long defaultID) : base(defaultID) { }

		public OnepAmptp(OnepAmptp rhs)
		{
			this._targetGain = rhs._targetGain;

			CopyOnepAmptpVolatileProperties(rhs);
		}

		partial void CopyOnepAmptpVolatileProperties(OnepAmptp rhs);

		#endregion

		public virtual double? TargetGain { get => _targetGain; set => _targetGain = value; }

	}
}
