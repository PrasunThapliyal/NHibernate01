
namespace ORM_NHibernate.CommonCS
{
    using System;
    using System.ComponentModel;

    public interface IOnePlannerBase : INotifyPropertyChanged
    {
        // Properties
        Guid UniqueId { get; }
        int? NaturalId { get; }
        String NaturalName { get; }
        long DefaultId { get; }

        void SetDefaultId(long defaultID);
        void ResetDefaultId();
    }
}
