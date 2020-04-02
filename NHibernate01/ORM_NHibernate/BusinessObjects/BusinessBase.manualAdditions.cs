
namespace ORM_NHibernate.BusinessObjects
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public abstract partial class BusinessBase<T> : IBusinessBase<T> //, IOnePlannerDeletable
    {
        static int _count = 0;
        private Guid _uniqueId;
        private int _internalCreationId;
        private long _defaultID;

        #region Constructors

        /// <summary>
        /// Creates an instance of BusinessBase/>
        /// </summary>
        protected BusinessBase()
        {
            _uniqueId = Guid.NewGuid();
            ResetDefaultId();
        }

        protected BusinessBase(long defaultID)
        {
            _uniqueId = Guid.NewGuid();
            SetDefaultId(defaultID);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Default ID  uses hi/lo combination to guarantee a unique default id in time and space
        /// although it's not persited itself, its copy is used in OnepObjectID, which in turn may be persited
        /// and/or passed across java boundry
        /// The hi is stored to/taken from the highest int in the Guid Unique Id, as it's essencially a timestamp tick http://www.ietf.org/rfc/rfc4122.txt
        /// The loId is the internal creation id, which is incremental per application instance per type
        /// the uniqueness is based on the combination of time and creation variant, same idea as in http://blogs.msdn.com/b/oldnewthing/archive/2008/06/27/8659071.aspx
        /// Default Id setter is the only place that can change uniqueId and internal Creation Id
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public virtual long DefaultId
        {
            get
            {
                if (_defaultID != 0)
                    return _defaultID;
                else
                {
                    Debug.Assert(false);
                    return _defaultID;
                }
            }
            private set
            {
                StoreDefaultID(value);
                // Store permanet defaultID
                _defaultID = CalcDefaultID();
            }
        }



        private void StoreDefaultID(long value)
        {
            var longBytes = BitConverter.GetBytes(value);
            // store the higher 32 bit in the Unique Id
            var guidBytes = _uniqueId.ToByteArray();
            Array.Copy(longBytes, guidBytes, 4);
            _uniqueId = new Guid(guidBytes);
            // store the lower 32 bit in the internalCreation id
            _internalCreationId = BitConverter.ToInt32(longBytes, 4);
        }

        private long CalcDefaultID()
        {
            // get the higher 32 bit from the Unique Id
            var guidBytes = _uniqueId.ToByteArray();
            // get the lower 32 bit from the internalCreation id
            var intBytes = BitConverter.GetBytes(_internalCreationId);
            Array.Copy(intBytes, 0, guidBytes, 4, 4);
            return BitConverter.ToInt64(guidBytes, 0);
        }

        /// <summary>
        /// Get the Globally Unique ID associated with the instance. 
        /// </summary>
        /// <remarks>UniqueId is NOT persisted. It is set in the constructor and is immutable for the life of the object.
        /// UniqueId MUST NOT be used in Equals and GetHashCode for 1P objects. 
        /// NHibernate relies on business equality for its proxies to work; two objects for the same database row must be considered equal, 
        /// and including UniqueId in Equals and GetHashCode would break this.
        /// </remarks>
        [System.Runtime.Serialization.IgnoreDataMember]
        public virtual Guid UniqueId
        {
            get { return _uniqueId; }
            set { _uniqueId = value; }
        }

        /// <summary>
        /// default natural Id is obtained from the first 4 bytes of GUID
        /// </summary>
        public virtual int? NaturalId
        {
            get
            {
                return _internalCreationId;
            }
        }

        /// <summary>
        /// default natural name is obtained from natural id
        /// </summary>
        public virtual String NaturalName
        {
            get
            {
                return _internalCreationId.ToString();
            }
        }

        public override string ToString()
        {
            return this.GetType().Name + NaturalName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Reset id in order to allow entity to be saved as a new database entry
        /// </summary>
        public virtual void ResetId()
        {
            Id = default(T);
        }

        /// <summary>
        /// Sets the default id.
        /// </summary>
        /// <param name="defaultID">The default ID.</param>
        public virtual void SetDefaultId(long defaultID)
        {
            DefaultId = defaultID;
        }

        /// <summary>
        /// Resets the default id.
        /// </summary>
        public virtual void ResetDefaultId()
        {
            ResetDefaultId(16);
        }

        /// <summary>
        /// Resets the default id with timestamp in (32 - maskbits) bits  and type signature in maskbits bits
        /// maskbits in [0,32), recommended 16
        /// </summary>
        protected virtual void ResetDefaultId(int maskbits)
        {
            _count++;
            if (_internalCreationId == _count)
                _count++;
            _internalCreationId = _count;
            uint timestamp = (uint)(CalcDefaultID() & 0xFFFFFFFF);
            timestamp &= (0xFFFFFFFF >> maskbits);
            uint signature = (uint)this.GetType().GetHashCode() & (0xFFFFFFFF << (32 - maskbits));
            DefaultId = (long)_internalCreationId << 32 | timestamp | signature;
        }

        /// <summary>
        /// default business signature
        /// </summary>
        /// <returns></returns>
        public virtual string BusinessSignature()
        {
            return GetType().Name + Id.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual U As<U>() where U : BusinessBase<T>
        {
            return this as U;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual bool Is<U>() where U : BusinessBase<T>
        {
            return this is U;
        }

        public override bool Equals(object rhs)
        {
            BusinessBase<T> other = rhs as BusinessBase<T>;

            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) && !IsTransient(other) && Id.Equals(other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                otherType.IsAssignableFrom(thisType);
            }
            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(T)))
                return base.GetHashCode();

            return Id.GetHashCode();
        }
        #endregion
    }

}
