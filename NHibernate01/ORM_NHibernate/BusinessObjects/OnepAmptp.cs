
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

		#region Methods

		public override string BusinessSignature()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append(this.GetType().FullName);
			sb.Append(_targetGain);
			return sb.ToString();
		}

		//public override string BusinessSignature()
		//{
		//	System.Text.StringBuilder sb = new System.Text.StringBuilder();

		//	sb.Append(this.GetType().FullName);
		//	sb.Append(_targetGain);
		//	return sb.ToString();
		//}
		#endregion

		#region Properties

		public virtual double? TargetGain { get => _targetGain; set => _targetGain = value; }
		public virtual OnepNetwork OnepNetwork { get => _onepNetwork; set => _onepNetwork = value; }
		public virtual OnepTerminationpoint OnepTerminationpoint { get => _onepTerminationpoint; set => _onepTerminationpoint = value; }

		#endregion

		#region Manual Additions

		// TODO : Why is OnepNetwork in the manual additions in 1P
		// Likewise, onepTerminationpoint
		private OnepNetwork _onepNetwork = null;
		private OnepTerminationpoint _onepTerminationpoint = null;

		#endregion

	}
}
