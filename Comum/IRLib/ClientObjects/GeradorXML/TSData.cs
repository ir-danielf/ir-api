using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class TSData_tSProperty
    {
        public readonly string tSData = "TSData";

        public readonly string header = "Header";
        public Header_tSProperty Header = new Header_tSProperty();

        public readonly string tSProperty = "TSProperty";
        public List<tSProperty> TSProperty = new List<tSProperty>();
    }

    [Serializable]
    public class Header_tSProperty
    {
        public readonly string version = "Version";
        public string Version { get; set; }

        public readonly string issuer = "Issuer";
        public int Issuer { get; set; }

        public readonly string receiver = "Receiver";
        public int Receiver { get; set; }

        public readonly string iD = "ID";
        public int ID { get; set; }
    }

    [Serializable]
    public class tSProperty
    {
        public readonly string type = "Type";
        public string Type { get; set; }

        public readonly string action = "Action";
        public string Action { get; set; }

        public readonly string iD = "ID";
        public int ID { get; set; }

        public readonly string name = "Name";
        public string Name { get; set; }

        public readonly string arguments = "Arguments";
        public Arguments_tSProperty Arguments { get; set; }
    }

    [Serializable]
    public class Arguments_tSProperty
    {
        public readonly string place = "Place";
        public string Place { get; set; }

        public readonly string fromDate = "FromDate";
        public DateTime FromDate { get; set; }

        public readonly string fromTime = "FromTime";
        public DateTime FromTime
        {
            get
            {
                return FromDate;
            }
        }

        public readonly string toDate = "ToDate";
        public DateTime ToDate
        {
            get
            {
                return FromDate;
            }
        }

        public readonly string toTime = "ToTime";
        public DateTime ToTime
        {
            get
            {
                return FromDate;
            }
        }
    }
}
