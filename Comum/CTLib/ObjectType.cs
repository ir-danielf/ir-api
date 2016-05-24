using System;
using System.Configuration;

namespace CTLib
{
    public interface ISponsoredObject { }

    public class ObjectType : Attribute
    {
        public enum RemotingType
        {
            CAO,
            SingleTon,
            SingleCall,
            SingleTonSpecialEvent
        }
        private RemotingType r;

        public RemotingType Type
        {
            get { return r; }
            set { r = value; }
        }
        public ObjectType(RemotingType remoting)
        {
            r = remoting;
        }
    }

    public class JirayaObject : Attribute { }


    public class Remoting1
    {
        private static string ip = ConfigurationManager.AppSettings["IP"];


        private static int GetRandomicPort()
        {
            int portaInicial = Convert.ToInt32(ConfigurationManager.AppSettings["PortaInicial"]);
            int portaFinal = Convert.ToInt32(ConfigurationManager.AppSettings["PortaFinal"]);
            if (portaFinal - portaInicial < 0)
                throw new Exception("O range de portas definido no config é inválido!");
            Random rnd = new Random((int)DateTime.Now.Ticks);

            return rnd.Next(portaInicial, portaFinal + 1);
        }

        private static ObjectType.RemotingType GetObjectType<T>()
        {
            var type = typeof(T);


            object[] o = type.GetCustomAttributes(typeof(ObjectType), false);
            if (o.Length > 0)
            {
                var ot = type.GetCustomAttributes(typeof(ObjectType), false)[0] as ObjectType;
                return ot.Type;
            }
            else
                return ObjectType.RemotingType.CAO;
        }

        private static int GetPort<T>()
        {
            if (GetObjectType<T>() == ObjectType.RemotingType.SingleTonSpecialEvent)
                return Convert.ToInt32(ConfigurationManager.AppSettings["PortaSpecialEvent"]);
            else
                return GetRandomicPort();
        }
        
        public static T GetObject<T>() where T : new()
        {
            var type = typeof(T);


            var objectName = string.Format("tcp://{0}:{1}/{2}", ip, GetPort<T>(), type.Name);

            switch (GetObjectType<T>())
            {
                case ObjectType.RemotingType.CAO:
                case ObjectType.RemotingType.SingleTon:
                case ObjectType.RemotingType.SingleCall:

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["AcessoLocal"]))
                        return new T();
                    else
                        return (T)Activator.GetObject(type, objectName);

                case ObjectType.RemotingType.SingleTonSpecialEvent:

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["SingleTonObjectsAtivo"]))
                        return (T)Activator.GetObject(type, objectName);
                    else
                        return new T();

                default:
                    throw new ArgumentException("Como vc fez isso?");

            }
        }
    }
}
