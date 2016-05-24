using System;
using System.Collections.Generic;

namespace IRLib.ClientObjects
{
    [Serializable]
    public class BlacklistRecord
    {
        public readonly string tSData = "TSData";

        public readonly string blacklistRecord = "BlacklistRecord";

        public readonly string expire = "Expire";
        public DateTime Expire { get; set; }

        public readonly string action = "Action";
        public string Action { get; set; }

        public readonly string blockingtype = "BlockingType";
        public int BlockingType { get; set; }

        public readonly string blockingreason = "BlockingReason";
        public int BlockingReason { get; set; }

        public readonly string from = "From";
        public DateTime From { get; set; }

        public readonly string to = "To";
        public DateTime To { get; set; }

        public readonly string displaymessage = "DisplayMessage";
        public string DisplayMessage { get; set; }

        public readonly string comment = "Comment";
        public string Comment { get; set; }

        public readonly string header = "Header";
        public Header_BlackListRecord Header = new Header_BlackListRecord();

        public readonly string whitelistRecord = "WhitelistRecord";
        public List<WhitelistRecord_BlackListRecord> Black_WhitelistRecord = new List<WhitelistRecord_BlackListRecord>();
    }

    [Serializable]
    public class Header_BlackListRecord
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
    public class WhitelistRecord_BlackListRecord
    {
        public readonly string utid = "UTID";
        public string UTID { get; set; }

        public readonly string coding = "Coding";
        public string Coding { get; set; }

        public readonly string permission = "Permission";
        public Permission_BlackListRecord Permission = new Permission_BlackListRecord();

        public readonly string tsproperty = "TSProperty";
        public TSProperty_BlackListRecord TSProperty { get; set; }
    }

    [Serializable]
    public class TSProperty_BlackListRecord
    {
        public readonly string type = "Type";

        public readonly string id = "ID";

        public readonly string Type_Event = "EVENT";
        public string EventoID { get; set; }

        public readonly string Type_Area = "AREA";
        public string AreaID { get; set; }

        public readonly string Type_TicketType = "TICKETTYPE";
        public string TicketTypeID { get; set; }

        public readonly string Type_SeasonTicket = "SEASONTICKET";
        public string SeasonTicketID { get; set; }

        public readonly string Type_Sector = "SECTOR";
        public string Sector { get; set; }

        public readonly string Type_Validity = "VALIDITY";

        public readonly string from = "From";
        public DateTime From { get; set; }

        public readonly string to = "To";
        public DateTime To { get; set; }
    }

    [Serializable]
    public class Permission_BlackListRecord
    {
        public readonly string upid = "UPID";
        public int UPID { get; set; }
    }
}
