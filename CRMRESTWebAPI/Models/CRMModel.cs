using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CRMRESTWebAPI.Models
{
    [DataContract]
    public class updateTicketStatus
    {
        [DataMember]
        public string ticketID { get; set; }
        [DataMember]
        public int ticketStatusValue { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createUpdateAccount
    {
        //[DataMember]
        //public string oracleCustomerID { get; set; }
        [DataMember]
        public string salesCustomerID { get; set; }
        [DataMember]
        public string customerNumber { get; set; }
        [DataMember]
        public string customerType { get; set; }
        [DataMember]
        public string customerStatus { get; set; }
        [DataMember]
        public string countryOfResidence { get; set; }
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string telephoneTypeNo { get; set; }
        [DataMember]
        public string phoneCountryCode { get; set; }
        [DataMember]
        public string phoneAreaCode { get; set; }
        [DataMember]
        public string phoneNumber { get; set; }
        [DataMember]
        public string secondaryPhoneTypeNo { get; set; }
        [DataMember]
        public string altCountryCode { get; set; }
        [DataMember]
        public string altphoneAreaCode { get; set; }
        [DataMember]
        public string altphoneNumber { get; set; }
        [DataMember]
        public string emailAddress { get; set; }
        [DataMember]
        public string emailAddress2 { get; set; }
        [DataMember]
        public string website { get; set; }
        [DataMember]
        public string trnNumber { get; set; }
        [DataMember]
        public string brokerNumber { get; set; }
        [DataMember]
        public string titleNo { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string middleName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string accountName { get; set; }
        [DataMember]
        public string arabicCompanyName { get; set; }
        [DataMember]
        public string nationality { get; set; }
        [DataMember]
        public string customerDateofBirthGreg { get; set; }
        [DataMember]
        public string customerDateofBirthHijri { get; set; }
        [DataMember]
        public string gender { get; set; }
        [DataMember]
        public string customerIDType { get; set; }
        [DataMember]
        public string customerIdNumber { get; set; }
        [DataMember]
        public string customerIDIssuedDate { get; set; }
        [DataMember]
        public string hijriIDIssueDate { get; set; }
        [DataMember]
        public string customerIDExpiryDate { get; set; }
        [DataMember]
        public string hijriIDExpiryDate { get; set; }
        [DataMember]
        public string placeOfIssue { get; set; }
        [DataMember]
        public string passportNumber { get; set; }
        [DataMember]
        public string passportIssueDate { get; set; }
        [DataMember]
        public string passportExpiryDate { get; set; }
        [DataMember]
        public string legallyAuthrorized { get; set; }
        [DataMember]
        public string legalAgentName { get; set; }
        [DataMember]
        public string agentFunctionality { get; set; }
        [DataMember]
        public string agencyType { get; set; }
        [DataMember]
        public string agencyRegistrationNo { get; set; }
        [DataMember]
        public string agencyExpiryDate { get; set; }
        [DataMember]
        public string hijriAgencyExpiryDate { get; set; }
        [DataMember]
        public string legalRemarks { get; set; }
        [DataMember]
        public string companyName { get; set; }
        [DataMember]
        public string natureOfBusinessNo { get; set; }
        [DataMember]
        public string licenseSourceNo { get; set; }
        [DataMember]
        public string licenseTypeNo { get; set; }
        [DataMember]
        public string tradeLicenseNo { get; set; }
        [DataMember]
        public string tradeLicenseCountryCode { get; set; }
        [DataMember]
        public string tradeLicenseIssueDate { get; set; }
        [DataMember]
        public string hijriTradeLicIssueDate { get; set; }
        [DataMember]
        public string tradeLicenseExpiryDate { get; set; }
        [DataMember]
        public string hijriTradeLicExpiryDate { get; set; }
        [DataMember]
        public string reraCertificateNo { get; set; }
        [DataMember]
        public string RERAStartDate { get; set; }
        [DataMember]
        public string RERAExpiryDate { get; set; }
        [DataMember]
        public string homeAddress1 { get; set; }
        [DataMember]
        public string homeAddress2 { get; set; }
        [DataMember]
        public string homeAddress3 { get; set; }
        [DataMember]
        public string homeCity { get; set; }
        [DataMember]
        public string homeState { get; set; }
        [DataMember]
        public string homeCountry { get; set; }
        [DataMember]
        public string homePOBox { get; set; }
        [DataMember]
        public string homePostalCode { get; set; }
        [DataMember]
        public string officeAddress1 { get; set; }
        [DataMember]
        public string officeAddress2 { get; set; }
        [DataMember]
        public string officeAddress3 { get; set; }
        [DataMember]
        public string officeCity { get; set; }
        [DataMember]
        public string officeState { get; set; }
        [DataMember]
        public string officeCountry { get; set; }
        [DataMember]
        public string officePOBox { get; set; }
        [DataMember]
        public string officePostalCode { get; set; }
        [DataMember]
        public string arabicAddress1 { get; set; }
        [DataMember]
        public string arabicAddress2 { get; set; }
        [DataMember]
        public string arabicAddress3 { get; set; }
        [DataMember]
        public string arabicCity { get; set; }
        [DataMember]
        public string arabicState { get; set; }
        [DataMember]
        public string arabicCountry { get; set; }
        [DataMember]
        public string arabicPOBox { get; set; }
        [DataMember]
        public string arabicPostalCode { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createUpdateProperty
    {
        [DataMember]
        public string propertyID { get; set; }
        [DataMember]
        public string unitID { get; set; }
        [DataMember]
        public string orgID { get; set; }
        [DataMember]
        public string buildingLandID { get; set; }
        [DataMember]
        public string buildingORLand { get; set; }
        [DataMember]
        public string buildingORLandCode { get; set; }
        [DataMember]
        public string buildingOrLandAlias { get; set; }
        [DataMember]
        public string buildingOrLandName { get; set; }
        [DataMember]
        public string buildingOrLandStartDate { get; set; }
        [DataMember]
        public string buildingOrLandEndDate { get; set; }

        [DataMember]
        public string floorParcelID { get; set; }
        [DataMember]
        public string floorParcel { get; set; }
        [DataMember]
        public string floorParcelCode { get; set; }
        [DataMember]
        public string floorParcelAlias { get; set; }
        [DataMember]
        public string floorParcelName { get; set; }
        [DataMember]
        public string floorParcelStartDate { get; set; }
        [DataMember]
        public string floorParcelEndDate { get; set; }

        [DataMember]
        public string unitOrSection { get; set; }
        [DataMember]
        public string unitOrSectionCode { get; set; }
        [DataMember]
        public string unitOrSectionAlias { get; set; }
        [DataMember]
        public string unitOrSectionName { get; set; }
        [DataMember]
        public string unitOrSectionStartDate { get; set; }
        [DataMember]
        public string unitOrSectionEndDate { get; set; }
        [DataMember]
        public decimal titleDeedAreaSQFT { get; set; }
        [DataMember]
        public decimal balconyAreaSQFT { get; set; }
        [DataMember]
        public decimal saleableAreaSQFT { get; set; }
        [DataMember]
        public decimal titleDeedAreaSQM { get; set; }
        [DataMember]
        public decimal balconyAreaSQM { get; set; }
        [DataMember]
        public decimal saleableAreaSQM { get; set; }
        [DataMember]
        public string unitType { get; set; }
        [DataMember]
        public string unitModel { get; set; }
        [DataMember]
        public string unitFacing { get; set; }
        [DataMember]
        public string unitPosition { get; set; }
        [DataMember]
        public string roadSize { get; set; }
        [DataMember]
        public string titleDeedNumber { get; set; }
        [DataMember]
        public string titleDeedDate { get; set; }
        [DataMember]
        public string bankName { get; set; }
        [DataMember]
        public string bankAccountType { get; set; }
        [DataMember]
        public string bankIBAN { get; set; }
        [DataMember]
        public string bankAccount { get; set; }
        [DataMember]
        public string apartmentView { get; set; }
        [DataMember]
        public string apartmentType { get; set; }
        [DataMember]
        public string numOfCarParking { get; set; }
        [DataMember]
        public string maidRoom { get; set; }
        [DataMember]
        public string studyRoom { get; set; }
        [DataMember]
        public string numOfRooms { get; set; }
        [DataMember]
        public string furnitured { get; set; }
        [DataMember]
        public string terraceArea { get; set; }
        [DataMember]
        public string MAXGFA { get; set; }
        [DataMember]
        public string shellCore { get; set; }
        [DataMember]
        public string unitPlan { get; set; }
        [DataMember]
        public string VATUsage { get; set; }
        [DataMember]
        public decimal VATPercentage { get; set; }
        [DataMember]
        public decimal basePrice { get; set; }
        [DataMember]
        public decimal sellingPrice { get; set; }
        [DataMember]
        public decimal sellingPriceSQFT { get; set; }
        [DataMember]
        public decimal sellingPriceSQM { get; set; }
        [DataMember]
        public decimal basePriceSQFT { get; set; }
        [DataMember]
        public decimal basePriceSQM { get; set; }
        [DataMember]
        public decimal gracePeriod { get; set; }
        [DataMember]
        public decimal tokenDepositAmount { get; set; }
        [DataMember]
        public decimal downPaymentPercentage { get; set; }
        //[DataMember]
        //public string paymentPlanID { get; set; }
        [DataMember]
        public string unitStatus { get; set; }
        [DataMember]
        public string releaseID { get; set; }
        [DataMember]
        public decimal applicableDiscount { get; set; }

        //[DataMember]
        //public string siteNumber { get; set; }
        //[DataMember]
        //public int businessType { get; set; }
        //[DataMember]
        //public int propertyStatus { get; set; }


        //[DataMember]
        //public string unitNumber { get; set; }
        //[DataMember]
        //public int type { get; set; }
        //[DataMember]
        //public decimal netAreaSQM { get; set; }
        //[DataMember]
        //public decimal totalAreaSQM { get; set; }
        //[DataMember]
        //public string daar_country { get; set; }

        //[DataMember]
        //public string daar_floor_parcel_id { get; set; }


        //[DataMember]
        //public string daar_title_deed_transfer_date { get; set; }
        //[DataMember]
        //public string daar_source_of_area { get; set; }
        //[DataMember]
        //public decimal daar_applicable_discount { get; set; }
        //[DataMember]
        //public string daar_projectName { get; set; }
        //[DataMember]
        //public string daar_floor { get; set; }



        //[DataMember]
        //public decimal daar_net_area_suare_feet { get; set; }

        //[DataMember]
        //public decimal daar_terrace_suare_feet { get; set; }
        //[DataMember]
        //public decimal daar_total_area_suare_feet { get; set; }


        //[DataMember]
        //public string daar_bedroom { get; set; }









        //[DataMember]
        //public string daar_plot_number { get; set; }

        //[DataMember]
        //public string daar_plot_area_deed { get; set; }
        //[DataMember]
        //public int daar_propertyactive { get; set; }
        //[DataMember]
        //public int daar_unitactive { get; set; }
        //[DataMember]
        //public int daar_buildingactive { get; set; }
        //[DataMember]
        //public int daar_flooractive { get; set; }
        //[DataMember]
        //public string daar_escrowibanaccountnumber { get; set; }
        //[DataMember]
        //public string daar_escrowaccountnumber { get; set; }
        //[DataMember]
        //public decimal daar_reservationperiodindays { get; set; }
        //[DataMember]
        //public decimal daar_bookingadminfeepercentage { get; set; }


        //[DataMember]
        //public string daar_paymentterm { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createUpdateBroker
    {
        [DataMember]
        public string brokerID { get; set; }
        [DataMember]
        public string brokerType { get; set; }
        [DataMember]
        public string brokerNumber { get; set; }
        [DataMember]
        public string brokerName { get; set; }
        [DataMember]
        public string brokerNameArabic { get; set; }
        [DataMember]
        public string contractDate { get; set; }
        [DataMember]
        public string hijriContractDate { get; set; }
        [DataMember]
        public string contractExpiryDate { get; set; }
        [DataMember]
        public string countryCode { get; set; }
        [DataMember]
        public string activeFlag { get; set; }
        [DataMember]
        public string contractStatus { get; set; }





        //[DataMember]
        //public int IDType { get; set; }
        //[DataMember]
        //public string IDNumber { get; set; }
        //[DataMember]
        //public string IDExpiryDate { get; set; }
        //[DataMember]
        //public string Nationality { get; set; }
        //[DataMember]
        //public string addressLine1 { get; set; }
        //[DataMember]
        //public string addressLine2 { get; set; }
        //[DataMember]
        //public string addressLine3 { get; set; }
        //[DataMember]
        //public string addressLine4 { get; set; }
        //[DataMember]
        //public string emailAddress { get; set; }

        //[DataMember]
        //public string areaCode { get; set; }
        //[DataMember]
        //public string telephoneNumber { get; set; }
        //[DataMember]
        //public string city { get; set; }
        //[DataMember]
        //public string state { get; set; }
        //[DataMember]
        //public string postalCode { get; set; }
        //[DataMember]
        //public string Country { get; set; }
        //[DataMember]
        //public string tradeLicenseNumber { get; set; }
        //[DataMember]
        //public string tradeLicenseExpiryDate { get; set; }
        //[DataMember]
        //public string ndaSignedDate { get; set; }
        //[DataMember]
        //public string ndaExpiryDate { get; set; }
        //[DataMember]
        //public string reraNumber { get; set; }
        //[DataMember]
        //public string reraStartDate { get; set; }

        //[DataMember]
        //public string reraExpiryDate { get; set; }
        //[DataMember]
        //public string agencyAgreementSignedDate { get; set; }
        //[DataMember]
        //public string agencyAgreementExpiryDate { get; set; }
        //[DataMember]
        //public string cityArabic { get; set; }
        //[DataMember]
        //public string countryArabic { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createUpdateUser
    {
        [DataMember]
        public string domainName { get; set; }
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string employeeID { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string businessUnitGUID { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string emailID { get; set; }
        [DataMember]
        public string mobilePhone { get; set; }
        [DataMember]
        public string managerEmployeeID { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createUpdateProposalStatus
    {
        [DataMember]
        public string proposalID { get; set; }
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createLead
    {
        [DataMember]
        public string leadID { get; set; }
        [DataMember]
        public string requirement { get; set; }
        [DataMember]
        public string reqDescription { get; set; }
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public string jobTitle { get; set; }
        [DataMember]
        public int leadType { get; set; }
        [DataMember]
        public int leadSource { get; set; }
        [DataMember]
        public string businessPhone { get; set; }
        [DataMember]
        public string mobilePhone { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public int rating { get; set; }
    }

    [DataContract]
    public class createTemporaryReceipts
    {
        [DataMember]
        public string CRMProposalID { get; set; }
        [DataMember]
        public string accountNumber { get; set; }
        [DataMember]
        public string registrationID { get; set; }
        [DataMember]
        public string reservationNo { get; set; }
        [DataMember]
        public string salesCustomerID { get; set; }
        [DataMember]
        public string customerNo { get; set; }
        [DataMember]
        public string orgID { get; set; }
        [DataMember]
        public string receiptID { get; set; }
        [DataMember]
        public string receiptNumber { get; set; }
        [DataMember]
        public string receiptDate { get; set; }
        [DataMember]
        public string chargeType { get; set; }
        [DataMember]
        public decimal receiptAmount { get; set; }
        [DataMember]
        public string currencyCode { get; set; }
        [DataMember]
        public decimal functionalAmount { get; set; }
        [DataMember]
        public string paymentMethod { get; set; }
        [DataMember]
        public string receiptStatus { get; set; }
        [DataMember]
        public string arCashReceiptID { get; set; }
        [DataMember]
        public string arReceiptNumber { get; set; }
        [DataMember]
        public string arVoucherNumber { get; set; }
        [DataMember]
        public string arReceiptDate { get; set; }
        [DataMember]
        public string advanceInvTrxID { get; set; }
        [DataMember]
        public string advanceInvoiceNo { get; set; }
        [DataMember]
        public string advanceCMTRXID { get; set; }
        [DataMember]
        public string advanceCreditMemoNo { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createActualReceipts
    {
        [DataMember]
        public string CRMProposalID { get; set; }
        [DataMember]
        public string accountNumber { get; set; }
        [DataMember]
        public string registrationID { get; set; }
        [DataMember]
        public string reservationNo { get; set; }
        [DataMember]
        public string salesCustomerID { get; set; }
        [DataMember]
        public string customerNumber { get; set; }
        [DataMember]
        public string cashReceiptID { get; set; }
        [DataMember]
        public string receiptNumber { get; set; }
        [DataMember]
        public string receiptDate { get; set; }
        [DataMember]
        public string receiptCreationDate { get; set; }
        [DataMember]
        public string receiptStatus { get; set; }
        [DataMember]
        public decimal amount { get; set; }
        [DataMember]
        public string currencyCode { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createSalesContracts
    {
        [DataMember]
        public string customerID { get; set; }
        [DataMember]
        public string daar_contractno { get; set; }
        [DataMember]
        public string daar_contracttype { get; set; }
        [DataMember]
        public string daar_propertycode { get; set; }
        [DataMember]
        public decimal daar_contractvalue { get; set; }
        [DataMember]
        public string daar_bookeddate { get; set; }
        [DataMember]
        public string daar_solddate { get; set; }
        [DataMember]
        public string daar_saexecuteddate { get; set; }
        [DataMember]
        public string daar_maintananceeligibility { get; set; }
        [DataMember]
        public string daar_contractstatus { get; set; }

    }

    [DataContract]
    public class createLeasingContracts
    {
        [DataMember]
        public string customerID { get; set; }
        [DataMember]
        public string daar_contractno { get; set; }
        [DataMember]
        public string daar_contracttype { get; set; }
        [DataMember]
        public string daar_propertycode { get; set; }
        [DataMember]
        public decimal daar_contractvalue { get; set; }
        [DataMember]
        public string daar_leaseddate { get; set; }
        [DataMember]
        public string daar_fitoutperiod { get; set; }
        [DataMember]
        public string daar_leasestartdate { get; set; }
        [DataMember]
        public string daar_leaseenddate { get; set; }
        [DataMember]
        public string daar_maintananceeligibility { get; set; }

    }

    [DataContract]
    public class createCustomerInvoice
    {
        [DataMember]
        public string CRMProposalID { get; set; }
        [DataMember]
        public string accountNumber { get; set; }
        [DataMember]
        public string registrationID { get; set; }
        [DataMember]
        public string registrationNo { get; set; }
        [DataMember]
        public string orgID { get; set; }
        [DataMember]
        public string customertrxID { get; set; }
        [DataMember]
        public string salesCustomerID { get; set; }
        [DataMember]
        public string customerNumber { get; set; }
        [DataMember]
        public string invoiceNumber { get; set; }
        [DataMember]
        public string invoiceDate { get; set; }
        [DataMember]
        public string CustTrxTypeID { get; set; }
        [DataMember]
        public string transactionType { get; set; }
        [DataMember]
        public string currencyCode { get; set; }
        [DataMember]
        public decimal amount { get; set; }
        [DataMember]
        public decimal appliedAmount { get; set; }
        [DataMember]
        public decimal creditedAmount { get; set; }
        [DataMember]
        public decimal remainingAmount { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }

    [DataContract]
    public class createProject
    {
        [DataMember]
        public string propertyID { get; set; }
        [DataMember]
        public string orgID { get; set; }
        [DataMember]
        public string propertyName { get; set; }
        [DataMember]
        public string propertyCode { get; set; }
        [DataMember]
        public string propertyStatus { get; set; }
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string activeProperty { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }
    [DataContract]
    public class createPaymentPlans
    {
        [DataMember]
        public string propertyID { get; set; }
        [DataMember]
        public string paymentPlanID { get; set; }
        [DataMember]
        public string paymentPlanNo { get; set; }
        [DataMember]
        public string paymentPlanName { get; set; }
        [DataMember]
        public string orgID { get; set; }
        [DataMember]
        public string paymentPlanStatus { get; set; }
        [DataMember]
        public string outcome { get; set; }
    }
}