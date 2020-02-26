namespace ORM_NHibernate.BusinessObjects
{
    public partial class OnepFibertl : OnepTopologicallink
    {
        #region Declarations

        private double? _loss = null;

        #endregion

        #region Constructors

        public OnepFibertl() { }

        public OnepFibertl(long defaultID) : base(defaultID) { }

        public OnepFibertl(OnepFibertl rhs)
            : base(rhs)
        {
            this._loss = rhs._loss;
            CopyOnepFibertlVolatileProperties(rhs);
        }

        partial void CopyOnepFibertlVolatileProperties(OnepFibertl rhs);

        #endregion

        #region Methods

        public override string BusinessSignature()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_loss);

            return sb.ToString();
        }
        #endregion

        #region Properties

        #region primitives
        public virtual double? Loss { get => _loss; set => _loss = value; }
        #endregion primitives

        #endregion
    }
}