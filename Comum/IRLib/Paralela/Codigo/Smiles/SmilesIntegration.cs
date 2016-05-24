using System;
using System.Collections.Generic;
using System.Linq;

namespace IRLib.Paralela
{
    public class PartnerRedemptionGateway : IRLib.PartnerGateway.PartnerRedemptionClient
    {
        public PartnerRedemptionGateway() { }

        public string Cancel_Item_Payment(string transactionId)
        {
            try
            {
                return this.CancelItemPayment(transactionId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.Close();
            }
        }

        public string Cancel_Item_Payment_(string transactionId)
        {
            try
            {
                return this.CancelItemPayment(transactionId);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                this.Close();
            }
        }

    }

    public static class SmilesPropertys
    {
        public static readonly string BirthDate = "Birthdate";
        public static readonly string BusinessEmail = "BusinessEmail";
        public static readonly string BusinessPhone = "BusinessPhone";
        public static readonly string FinancialID = "FinancialId";
        public static readonly string Gender = "Gender";
        public static readonly string HomePhone = "HomePhone";
        public static readonly string MaritalStatus = "MaritalStatus";
        public static readonly string MobileNumber = "MobileNumber";
        public static readonly string Name = "Name";
        public static readonly string PersonalEmail = "PersonalEmail";
        public static readonly string ShippingAddressAddress = "ShippingAddress_Address";
        public static readonly string ShippingAddress_City = "ShippingAddress_City";
        public static readonly string ShippingAddress_Complement = "ShippingAddress_Complement";
        public static readonly string ShippingAddress_Country = "ShippingAddress_Country";
        public static readonly string ShippingAddress_County = "ShippingAddress_County";
        public static readonly string ShippingAddress_Number = "ShippingAddress_Number";
        public static readonly string ShippingAddress_State = "ShippingAddress_State";
        public static readonly string ShippingAddress_Type = "ShippingAddress_Type";
        public static readonly string ShippingAddress_ZipCode = "ShippingAddress_ZipCode";
        public static readonly string UserId = "UserId";
        public static readonly string SmilesNumber = "SmilesNumber";
        public static readonly string OrderID = "OrderId";
        public static readonly string Return_URL = "RETURN_URL";
        public static readonly string Login = "Login";
        public static readonly string Source = "Source";
        public static readonly string ItemCount = "ItemCount";
        public static readonly string ItemCategory_ = "ItemCategory_";
        public static readonly string ItemDeliveryTime_ = "ItemDeliveryTime_";
        public static readonly string ItemID_ = "ItemId_";
        public static readonly string ItemName_ = "ItemName_";
        public static readonly string MilesAmount_ = "MilesAmount_";
        public static readonly string PartnerAlias_ = "PartnerAlias_";
        public static readonly string PartnerProductCode_ = "PartnerProductCode_";
        public static readonly string Smiles_ProductName_ = "Smiles_ProductName_";
        public static readonly string Alias_spcPartner = "Alias_spcPartner";
        public static readonly string Owner_spcLogin = "Owner_spcLogin";
    }
}