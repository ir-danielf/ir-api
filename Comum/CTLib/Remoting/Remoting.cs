using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;

namespace CTLib
{
    public class Remoting
    {
        public enum Projeto
        { 
            IRBilheteria, 
            Jiraya
        }
        public enum TipoConexao
        {
            Local = 0,
            RemotoPortaRandomica = 2
        }

        public TipoConexao Conexao { get; set; }
        private string ConexaoString { get; set; }
        public string Porta { get; set; }
        public string PortaSpecialEvent { get; set; }
        
        public bool Conectar(Projeto projeto)
        {
            string ip;
            string url;
            string urlSpecialEvent;
            bool AcessoLocal = Convert.ToBoolean(ConfigurationManager.AppSettings["AcessoLocal"]);
            if (AcessoLocal)
            {
                Conexao = TipoConexao.Local;
                ConexaoString = ConfigurationManager.AppSettings["Conexao"];
                return true;
            }
            else
            {
                Conexao = TipoConexao.RemotoPortaRandomica;
                int portaInicial = Convert.ToInt32(ConfigurationManager.AppSettings["PortaInicial"]);
                int portaFinal = Convert.ToInt32(ConfigurationManager.AppSettings["PortaFinal"]);
                int portaSPE = Convert.ToInt32(ConfigurationManager.AppSettings["PortaSpecialEvent"]);
                if (portaFinal - portaInicial < 0)
                    throw new Exception("O range de portas definido no config é inválido!");
                Random rnd = new Random((int)DateTime.Now.Ticks);
                ip = ConfigurationManager.AppSettings["IP"];

                if (string.IsNullOrEmpty(ip))
                    throw new Exception(("IP Não definido."));

                Porta = rnd.Next(portaInicial, portaFinal + 1).ToString();
                url = "tcp://" + ip + ":" + Porta + "/";
                urlSpecialEvent = string.Format("tcp://{0}:{1}/", ip, portaSPE);
                RegistrarDLLs(url, urlSpecialEvent, projeto);
                return true;
            }
        }

        private void RegistrarObjeto(Type type, string url, string urlSpecialEvent, string assemblyName)
        {
            ObjectType ot;

            if (type.IsSubclassOf(typeof(MarshalByRefObject)))
            {
                object[] o = type.GetCustomAttributes(typeof(ObjectType), false);
                if (o.Length > 0)
                {
                    ot = type.GetCustomAttributes(typeof(ObjectType), false)[0] as ObjectType;

                    switch (ot.Type)
                    {
                        //registra o objeto como um Client Activated Object (CAO)
                        case ObjectType.RemotingType.CAO:
                            RemotingConfiguration.RegisterActivatedClientType(type, url + assemblyName);//CAO
                            break;

                        //registra o objeto como SingleCall ou Singleton
                        case ObjectType.RemotingType.SingleCall:
                        case ObjectType.RemotingType.SingleTon:
                            RemotingConfiguration.RegisterWellKnownClientType(type, url + type.Name.ToString());
                            break;
                        case ObjectType.RemotingType.SingleTonSpecialEvent:
                            RemotingConfiguration.RegisterWellKnownClientType(type, urlSpecialEvent + type.Name);
                            break;
                    }
                }
                else
                {
                    RemotingConfiguration.RegisterActivatedClientType(type, url + assemblyName);//CAO
                }
            }
        }

        private void RegistrarAssembly(Assembly assembly, string url, string urlSpecialEvent, string assemblyName, Projeto projeto)
        {
            var types = assembly.GetTypes();
            
            if (projeto == Projeto.Jiraya)
                types = types.Where(c => c.GetCustomAttributes(typeof(CTLib.JirayaObject), false).Length > 0).ToArray();

            foreach (var type in types)
                RegistrarObjeto(type, url, urlSpecialEvent, assemblyName);
        }



        private void RegistrarDLLs(string url, string urlSpecialEvent, Projeto projeto)
        {

            RegistrarAssembly(Assembly.LoadFrom("IRLib.dll"), url, urlSpecialEvent, "IRLib", projeto);
            RegistrarAssembly(Assembly.LoadFrom("CTLib.dll"), url, urlSpecialEvent, "CTLib", projeto);
        }
    }
}
