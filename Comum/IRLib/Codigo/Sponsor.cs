using CTLib;
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;


namespace IRLib
{
    [ObjectType(ObjectType.RemotingType.CAO)]
    public class Sponsor : MarshalByRefObject, ISponsor, ISponsoredObject
    {
        public static bool AcessoLocal;
        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            lease.InitialLeaseTime = DefaultLease.InitialLeaseTime;
            lease.RenewOnCallTime = DefaultLease.RenewOnCallTime;
            lease.SponsorshipTimeout = DefaultLease.SponsorshipTimeout;
            return lease;
            //return null;
        }

        public void DesregistrarSponsor(MarshalByRefObject obj)
        {
            if (AcessoLocal)
                return;
            if (obj == null)
                return;
            try
            {
                ILease lease = (ILease)RemotingServices.GetLifetimeService(obj);
                lease.Unregister(this);
            }
            catch (NullReferenceException)
            { }
            catch (Exception ex)
            {
               // throw ex;
            }
        }

        //public void RegistrarSponsor(MarshalByRefObject obj)
        //{

        //    if (AcessoLocal)
        //        return;
        //    try
        //    {
        //        ObjectType oType;
        //        oType = obj.GetType().GetCustomAttributes(typeof(ObjectType), false)[0] as ObjectType;

        //        if (oType.Type != ObjectType.RemotingType.SingleCall)
        //        {
        //            ILease lease = (ILease)RemotingServices.GetLifetimeService(obj);
        //            lease.Register(this);
        //        }
        //    }
        //    catch (NullReferenceException)
        //    { }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void RegistrarSponsor(ISponsoredObject obj)
        {

            if (AcessoLocal)
                return;
            try
            {
                if (obj != null)
                {
                    ILease lease = (ILease)RemotingServices.GetLifetimeService((MarshalByRefObject)obj);
                    lease.Register(this);
                }
            }
            catch (NullReferenceException)
            { }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Renova o lease
        public TimeSpan Renewal(ILease lease)
        {
            return TimeSpan.FromMinutes(5);
        }

    }
}
