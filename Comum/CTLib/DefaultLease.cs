using System;

namespace CTLib
{
    public static class DefaultLease
    {
        //LeaseTime Inicial
        public static readonly TimeSpan InitialLeaseTime = TimeSpan.FromMinutes(5);

        //Tempo que o Lease vai se renovar quando for chamado
        public static readonly TimeSpan RenewOnCallTime = TimeSpan.FromMinutes(5);

        //Tempo que o Lease vai aguardar pela renovação do Sponsor
        public static readonly TimeSpan SponsorshipTimeout = TimeSpan.FromSeconds(10);
    }
}
