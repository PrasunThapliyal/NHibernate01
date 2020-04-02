
namespace ORM_NHibernate.BusinessObjects
{
    using System;


    /// <summary>
    /// Every Business object must have a Id.
    /// </summary>
    /// <typeparam name="T">Literal for type.</typeparam>
    public interface IBusinessBase<T>
    {
        /// <summary>
        /// Gets Id which uniquely identifies a persistent object.
        /// </summary>
        T Id { get; }
    }


    /// <summary>
    /// Base for all business objects.
    ///
    /// For an explanation of why Equals and GetHashCode are overriden, read the following...
    /// http://devlicio.us/blogs/billy_mccafferty/archive/2007/04/25/using-equals-gethashcode-effectively.aspx
    /// </summary>
    /// <typeparam name="T">DataType of the primary key.</typeparam>
    public abstract partial class BusinessBase<T> : IBusinessBase<T> //, IOnePlannerBase
    {
        #region Declarations

        /// <summary>
        /// Uniquely identifies a persistent object. For transient objects has a default value.  
        /// </summary>
        private T _id = default(T);

        #endregion Declarations

        #region Properties

        /// <summary>
        /// Gets or sets id value.
        /// Exclude Id from serialization so that it is not persisted to file and is reset to 0 when deserialized.
        /// This avoids confusion for NHibernate which uses Id=0 to determine that object is not associated with a session.
        /// </summary>
        [System.Runtime.Serialization.IgnoreDataMember]
        public virtual T Id
        {
            get { return _id; }
            set
            {
                T oldValue = _id;
                _id = value;
                //NotifyPropertyChanged("Id", oldValue, value);
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Transient objects are not associated with an
        /// item already in storage.  For instance, a
        /// Customer is transient if its ID is 0.
        /// </summary>
        public virtual bool IsTransient()
        {
            return BusinessBase<T>.IsTransient(this);
        }

        /// <summary>
        /// Checks if an object is transient i.e an object yet to be associated with nHibernate proxy.
        /// An object is transient if, Id has default value. i.e. 0.
        /// </summary>
        /// <param name="obj">object for which the check is made.</param>
        /// <returns>true if transient object, else false.</returns>
        private static bool IsTransient(BusinessBase<T> obj)
        {
            return obj != null && Equals(obj.Id, default(T));
        }

        /// <summary>
        /// Returns the underlying type of an proxy object created by nHibernate.
        /// Applicable for persistent object.
        /// </summary>
        /// <returns>System.Type of unproxied object.</returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        #endregion Methods
    }
}
