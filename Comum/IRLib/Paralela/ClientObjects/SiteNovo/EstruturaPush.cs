using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IRLib.Paralela.ClientObjects
{

    [Serializable]
    [DataContract]
    public class EstruturaPush
    {
        [DataMember]
        public int User_ID { get; set; }

        [DataMember]
        public string Latitude { get; set; }

        [DataMember]
        public string Longitude { get; set; }

        [DataMember]
        public string Plataforma { get; set; }

        [DataMember]
        public string Token { get; set; }
    }
}
