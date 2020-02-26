
namespace ORM_NHibernate.BusinessObjects
{
	using System.Collections.Generic;

	public partial class OnepTopologicallink : BusinessBase<uint>
	{
		#region Declarations

		private string _name = null;
		private double? _length = null;
		private OnepTopologicallink _onepTopologicallinkMember1 = null;
		private OnepNetwork _onepNetwork = null;
		private OnepTerminationpoint _onepTerminationpoint1 = null;
		private OnepTerminationpoint _onepTerminationpoint2 = null;

		#endregion

		#region Constructors

		public OnepTopologicallink() { }

		public OnepTopologicallink(long defaultID) : base(defaultID) { }

		public OnepTopologicallink(OnepTopologicallink rhs)
		{
			this._name = rhs._name;
			this._length = rhs._length;

			CopyOnepTopologicallinkVolatileProperties(rhs);
		}

		partial void CopyOnepTopologicallinkVolatileProperties(OnepTopologicallink rhs);

		#endregion

		#region Methods

		public override string BusinessSignature()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append(this.GetType().FullName);
			sb.Append(_name);
			sb.Append(_length);

			return sb.ToString();
		}
		#endregion

		#region Properties

		#region primitives
		public virtual string Name { get => _name; set => _name = value; }
		public virtual double? Length { get => _length; set => _length = value; }
		#endregion primitives

		public virtual OnepNetwork OnepNetwork { get => _onepNetwork; set => _onepNetwork = value; }

		public virtual OnepTerminationpoint OnepTerminationpointByAEndTP { get => _onepTerminationpoint1; set => _onepTerminationpoint1 = value; }

		public virtual OnepTerminationpoint OnepTerminationpointByZEndTP { get => _onepTerminationpoint2; set => _onepTerminationpoint2 = value; }

		public virtual OnepTopologicallink OnepTopologicallinkMemberByUniMate { get => _onepTopologicallinkMember1; set => _onepTopologicallinkMember1 = value; }

		#endregion
	}
}