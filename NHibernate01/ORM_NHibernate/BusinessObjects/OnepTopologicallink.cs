
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
		private OnepTopologicallink _onepTopologicallinkMember2 = null;
		private IList<OnepTopologicallink> _onepTopologicallinks1 = new List<OnepTopologicallink>();

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
		public virtual string Name
		{
			get { return _name; }
			set
			{
				if (value != _name) // Unlike VB, (string.Empty == null) does not return true, so no need to replace it with string.Equals()
				{
					string oldValue = _name;
					OnNameChanging(_name, value);
					_name = value;
					OnNameChanged(oldValue, value);
					NotifyPropertyChanged("Name", oldValue, value);
				}
			}
		}

		public virtual double? Length
		{
			get { return _length; }
			set
			{
				if (value != _length) // Unlike VB, (string.Empty == null) does not return true, so no need to replace it with string.Equals()
				{
					double? oldValue = _length;
					OnLengthChanging(_length, value);
					_length = value;
					OnLengthChanged(oldValue, value);
					NotifyPropertyChanged("Length", oldValue, value);
				}
			}
		}

		#endregion primitives

		public virtual OnepTopologicallink OnepTopologicallinkMemberByParentTL
		{
			get { return _onepTopologicallinkMember1; }
			set
			{
				OnepTopologicallink oldValue = _onepTopologicallinkMember1;
				OnOnepTopologicallinkMemberByParentTLChanging(_onepTopologicallinkMember1, value);
				_onepTopologicallinkMember1 = value;
				OnOnepTopologicallinkMemberByParentTLChanged(oldValue, value);
				NotifyPropertyChanged("OnepTopologicallinkMemberByParentTL", oldValue, value);
			}
		}

		public virtual OnepNetwork OnepNetwork
		{
			get { return _onepNetwork; }
			set
			{
				OnepNetwork oldValue = _onepNetwork;
				OnOnepNetworkChanging(_onepNetwork, value);
				_onepNetwork = value;
				OnOnepNetworkChanged(oldValue, value);
				NotifyPropertyChanged("OnepNetwork", oldValue, value);
			}
		}

		public virtual OnepTerminationpoint OnepTerminationpointByAEndTP
		{
			get { return _onepTerminationpoint1; }
			set
			{
				OnepTerminationpoint oldValue = _onepTerminationpoint1;
				OnOnepTerminationpointByAEndTPChanging(_onepTerminationpoint1, value);
				_onepTerminationpoint1 = value;
				OnOnepTerminationpointByAEndTPChanged(oldValue, value);
				NotifyPropertyChanged("OnepTerminationpointByAEndTP", oldValue, value);
			}
		}

		public virtual OnepTerminationpoint OnepTerminationpointByZEndTP
		{
			get { return _onepTerminationpoint2; }
			set
			{
				OnepTerminationpoint oldValue = _onepTerminationpoint2;
				OnOnepTerminationpointByZEndTPChanging(_onepTerminationpoint2, value);
				_onepTerminationpoint2 = value;
				OnOnepTerminationpointByZEndTPChanged(oldValue, value);
				NotifyPropertyChanged("OnepTerminationpointByZEndTP", oldValue, value);
			}
		}

		public virtual OnepTopologicallink OnepTopologicallinkMemberByUniMate
		{
			get { return _onepTopologicallinkMember2; }
			set
			{
				OnepTopologicallink oldValue = _onepTopologicallinkMember2;
				OnOnepTopologicallinkMemberByUniMateChanging(_onepTopologicallinkMember2, value);
				_onepTopologicallinkMember2 = value;
				OnOnepTopologicallinkMemberByUniMateChanged(oldValue, value);
				NotifyPropertyChanged("OnepTopologicallinkMemberByUniMate", oldValue, value);
			}
		}

		public virtual IList<OnepTopologicallink> OnepTopologicallinksForParentTL
		{
			get { return _onepTopologicallinks1; }
			set
			{
				IList<OnepTopologicallink> oldValue = _onepTopologicallinks1;
				OnOnepTopologicallinksForParentTLChanging(_onepTopologicallinks1, value);
				_onepTopologicallinks1 = value;
				OnOnepTopologicallinksForParentTLChanged();
				NotifyPropertyChanged("OnepTopologicallinksForParentTL", oldValue, value);
			}
		}

		#endregion
	}
}