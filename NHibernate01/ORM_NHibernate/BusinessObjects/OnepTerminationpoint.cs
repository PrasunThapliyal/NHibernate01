
namespace ORM_NHibernate.BusinessObjects
{
	using System.Collections.Generic;

	public partial class OnepTerminationpoint : BusinessBase<long>
	{
		#region Declarations

		private byte? _ptp = null;
		private string _name = null;
		private string _notes = null;
		private byte? _role = null;

		private OnepNetwork _onepNetwork = null;
		private IList<OnepTopologicallink> _onepTopologicallinks1 = new List<OnepTopologicallink>();
		private IList<OnepTopologicallink> _onepTopologicallinks2 = new List<OnepTopologicallink>();

		#endregion

        #region Constructors

        public OnepTerminationpoint() { }

		public OnepTerminationpoint(long defaultID) : base(defaultID) { }

		public OnepTerminationpoint(OnepTerminationpoint rhs)
		{
			this._ptp = rhs._ptp;
			this._role = rhs._role;
			this._name = rhs._name;
			this._notes = rhs._notes;

			CopyOnepTerminationpointVolatileProperties(rhs);
		}

		partial void CopyOnepTerminationpointVolatileProperties(OnepTerminationpoint rhs);

		#endregion

		#region Methods

		public override string BusinessSignature()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append(this.GetType().FullName);
			sb.Append(_ptp);
			sb.Append(_role);
			sb.Append(_name);
			sb.Append(_notes);

			return sb.ToString();
		}
		#endregion

		#region Properties

		public virtual byte? Ptp { get => _ptp; set => _ptp = value; }
		public virtual string Name { get => _name; set => _name = value; }
		public virtual string Notes { get => _notes; set => _notes = value; }
		public virtual OnepNetwork OnepNetwork { get => _onepNetwork; set => _onepNetwork = value; }
		public virtual byte? Role { get => _role; set => _role = value; }
		public virtual IList<OnepTopologicallink> OnepTopologicallinksForAEndTP { get => _onepTopologicallinks1; set => _onepTopologicallinks1 = value; }
		public virtual IList<OnepTopologicallink> OnepTopologicallinksForZEndTP { get => _onepTopologicallinks2; set => _onepTopologicallinks2 = value; }

		#endregion

	}
}