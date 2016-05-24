using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class TSData_WhitelistRecord
    {
        public readonly string tSData = "TSData";

        public readonly string header = "Header";
        public Header_WhitelistRecord Header = new Header_WhitelistRecord();

        public readonly string whitelistRecord = "WhitelistRecord";
        public List<Whitelist_WhitelistRecord> WhitelistRecord = new List<Whitelist_WhitelistRecord>();
    }

    [Serializable]
    public class Header_WhitelistRecord
    {
        public readonly string version = "Version";
        public string Version { get; set; }

        public readonly string issuer = "Issuer";
        public int Issuer { get; set; }

        public readonly string receiver = "Receiver";
        public int Receiver { get; set; }

        public readonly string iD = "ID";
        public int ID { get; set; }

        public readonly string expire = "Expire";
    }

    [Serializable]
    public class Whitelist_WhitelistRecord
    {
        public readonly string action = "Action";
        public string Action { get; set; }

        public readonly string utid = "UTID";
        public string UTID { get; set; }

        public readonly string coding = "Coding";
        public string Coding { get; set; }

        public readonly string permission = "Permission";
        public Permission_WhitelistRecord Permission { get; set; }
    }

    [Serializable]
    public class Permission_WhitelistRecord
    {
        public readonly string upid = "UPID";
        public int UPID { get; set; }

        public readonly string type = "Type";
        public readonly string Type_Event = "EVENT";
        public readonly string Type_Area = "AREA";
        public readonly string Type_TicketType = "TICKETTYPE";
        public readonly string Type_PersonCategory = "PERSONCATEGORY";
        public readonly string Type_SeasonTicket = "SEASONTICKET";
        public readonly string Type_Validity = "VALIDITY";
        public readonly string Type_Sector = "SECTOR";

        public readonly string from = "From";
        public DateTime From { get; set; }

        public readonly string to = "To";
        public DateTime To { get; set; }

        public readonly string tsproperty = "TSProperty";
        public TSProperty_WhitelistRecord TSProperty { get; set; }
    }

    [Serializable]
    public class TSProperty_WhitelistRecord
    {
        public readonly string id = "ID";
        public string EventoID { get; set; }
        public string AreaID { get; set; }
        public string TicketTypeID { get; set; }
        public string PersonCategoryID { get; set; }
        public string SeassonPassID { get; set; }

        public string Sector { get; set; }
    }
}
