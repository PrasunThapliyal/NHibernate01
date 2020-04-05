
namespace ORM_NHibernate.BusinessObjects
{
	using System;
	using System.Collections.Generic;
	using System.Text;


	public partial class OnepNetwork: BusinessBase<long>
    {
		#region Declarations

		private string _name = null;
		private string _mcpProjectId = null;
		private IList<OnepAmptp> _onepAmptps = new List<OnepAmptp>();
		private IList<OnepFibertl> _onepFibertls = new List<OnepFibertl>();
		private IList<OnepTerminationpoint> _onepTerminationpoints = new List<OnepTerminationpoint>();
		private IList<OnepTopologicallink> _onepTopologicallinks = new List<OnepTopologicallink>();

        #endregion

        #region Constructors

        public OnepNetwork() { }

		public OnepNetwork(long defaultID) : base(defaultID) { }

		public OnepNetwork(OnepNetwork rhs)
		{
			this._name = rhs._name;
			this._mcpProjectId = rhs._mcpProjectId;

			CopyOnepNetworkVolatileProperties(rhs);
		}

		partial void CopyOnepNetworkVolatileProperties(OnepNetwork rhs);

		#endregion

		#region Manual Additions

		static OnepNetwork() { }

		public override string ToString()
		{
			return String.Format("OnepNetwork {0}", this.Name);
		}

		#endregion

		#region Methods

		public override string BusinessSignature()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			sb.Append(this.GetType().FullName);
			sb.Append(_name);
			sb.Append(_mcpProjectId);

			return sb.ToString();
		}
		#endregion

		#region Properties

		public virtual string Name { get => _name; set => _name = value; }
		public virtual string McpProjectId { get => _mcpProjectId; set => _mcpProjectId = value; }
		public virtual IList<OnepAmptp> OnepAmptps { get => _onepAmptps; set => _onepAmptps = value; }
		public virtual IList<OnepFibertl> OnepFibertls { get => _onepFibertls; set => _onepFibertls = value; }
		public virtual IList<OnepTerminationpoint> OnepTerminationpoints { get => _onepTerminationpoints; set => _onepTerminationpoints = value; }
		public virtual IList<OnepTopologicallink> OnepTopologicallinks { get => _onepTopologicallinks; set => _onepTopologicallinks = value; }

		#endregion

	}
}
