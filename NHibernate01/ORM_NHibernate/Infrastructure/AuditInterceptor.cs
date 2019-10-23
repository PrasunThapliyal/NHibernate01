
namespace ORM_NHibernate.Infrastructure
{
    using NHibernate;
    using NHibernate.Type;
    using System;

    public class AuditInterceptor : EmptyInterceptor, IInterceptor
    {

        private int updates;
        private int creates;
        private int loads;

        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            System.Diagnostics.Debug.WriteLine("NHibernate Interceptor: OnPrepareStatement: " + sql);

            return base.OnPrepareStatement(sql);
        }

        // Note: In the following code, IAuditable is an EF Core interface. We aren't doing EF Core here
        // But something we can explore at some point

        public override void OnDelete(object entity,
                                      object id,
                                      object[] state,
                                      string[] propertyNames,
                                      IType[] types)
        {
            // do nothing
        }

        //public override bool OnFlushDirty(object entity,
        //                                  object id,
        //                                  object[] currentState,
        //                                  object[] previousState,
        //                                  string[] propertyNames,
        //                                  IType[] types)
        //{
        //    if (entity is IAuditable)
        //    {
        //        updates++;
        //        for (int i = 0; i < propertyNames.Length; i++)
        //        {
        //            if ("lastUpdateTimestamp".Equals(propertyNames[i]))
        //            {
        //                currentState[i] = new DateTime();
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //public override bool OnLoad(object entity,
        //                            object id,
        //                            object[] state,
        //                            string[] propertyNames,
        //                            IType[] types)
        //{
        //    if (entity is IAuditable)
        //    {
        //        loads++;
        //    }
        //    return false;
        //}

        //public override bool OnSave(object entity,
        //                            object id,
        //                            object[] state,
        //                            string[] propertyNames,
        //                            IType[] types)
        //{
        //    if (entity is IAuditable)
        //    {
        //        creates++;
        //        for (int i = 0; i < propertyNames.Length; i++)
        //        {
        //            if ("createTimestamp".Equals(propertyNames[i]))
        //            {
        //                state[i] = new DateTime();
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}

        //public override void AfterTransactionCompletion(ITransaction tx)
        //{
        //    if (tx.WasCommitted)
        //    {
        //        System.Diagnostics.Debug.WriteLine(
        //            "NHibernate Interceptor: AfterTransactionCompletion: " +
        //            "Creations: " + creates +
        //            ", Updates: " + updates +
        //            ", Loads: " + loads);
        //    }
        //    updates = 0;
        //    creates = 0;
        //    loads = 0;
        //}

    }
}
