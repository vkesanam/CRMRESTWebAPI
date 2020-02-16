
using CRMRESTWebAPI.Log;
using CRMRESTWebAPI.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Web.Http;

namespace CRMRESTWebAPI.Controllers
{
    [CRMRESTWebAPI.Models.AuthorizeAttribute]
    [RoutePrefix("api/Registration")]
    public class CRMController : ApiController
    {
        #region IFD Connectiong String Details
        //static string Username = ConfigurationSettings.AppSettings["Username"];
        //static string Password = ConfigurationSettings.AppSettings["Password"];
        ////string Domain = ConfigurationSettings.AppSettings["Domain"];
        //static string Org = ConfigurationSettings.AppSettings["Org"];
        #endregion
        CRMErrorLogFile oErrorLog = new CRMErrorLogFile();

        #region OnPremise Connection String Details
        //static string Username = "Administrator";
        //static string Password = "Welcome@123";
        //static string Domain = "contoso";
        //static string Org = "http://192.168.1.8:5555/DAARUAT/XRMServices/2011/Organization.svc";
        static string Username = ConfigurationSettings.AppSettings["Username"];
        static string Password = ConfigurationSettings.AppSettings["Password"];
        //static string Domain = ConfigurationSettings.AppSettings["Domain"];
        static string Org = ConfigurationSettings.AppSettings["Org"];
        //static string SystemUnitGroup = ConfigurationSettings.AppSettings["SystemUnitGroup"];
        //static string SystemDefaultUnit = ConfigurationSettings.AppSettings["SystemDefaultUnit"];
        #endregion

        #region IFD
        IOrganizationService service = CRMRESTWebAPI.CRMConnection.CRMConnect.GetProcessIFD(Username, Password, Org);
        #endregion

        #region OnPremise
        //IOrganizationService service = CRMRESTWebAPI.CRMConnection.CRMConnect.GetProcessOnPremise(Username, Password, Domain, Org);
        #endregion



        [HttpPost]
        [Route("TicketStatus")]
        public updateTicketStatus updateTicketStatus(updateTicketStatus regTicStatus)
        {
            try
            {
                if(regTicStatus.ticketID!=null)
                {
                    QueryExpression q1 = new QueryExpression();
                    q1.ColumnSet = new ColumnSet("ticketnumber");
                    FilterExpression fe = new FilterExpression(LogicalOperator.And);
                    fe.AddCondition(new ConditionExpression("ticketnumber", ConditionOperator.Equal, regTicStatus.ticketID));
                    q1.Criteria = fe;
                    q1.EntityName = "incident";
                    EntityCollection ec = service.RetrieveMultiple(q1);
                    if(ec.Entities.Count>0)
                    {
                        foreach(Entity c in ec.Entities)
                        {
                            Guid CaseGuid = new Guid(c.Attributes["incidentid"].ToString());
                            Entity Case = new Entity("incident");
                            Case["new_maximostatus"] = new OptionSetValue(regTicStatus.ticketStatusValue);
                            Case.Id = CaseGuid;
                            service.Update(Case);
                            regTicStatus.outcome= CRMRESTWebAPI.Models.Outcomes.Updated.ToString();
                            return regTicStatus;
                        }
                    }
                }
            }
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            return regTicStatus;
        }
        [HttpPost]
        [Route("Units")]
        public createUpdateProperty createProperty(createUpdateProperty regProperty)
        {
            try
            {
                Entity Property = new Entity();
                if (regProperty != null)
                {
                    if (!string.IsNullOrEmpty(regProperty.unitID))
                    {
                        Property = new Entity("product", "productnumber", regProperty.unitID);
                    }
                    if (regProperty.propertyID != null && regProperty.propertyID != string.Empty)
                    {
                        Guid projectGUID = getProjectGUID(regProperty.propertyID);
                        if (projectGUID != Guid.Empty)
                        {
                            Property["daar_projectid"] = new EntityReference("daar_project", projectGUID);
                        }
                    }
                    Property["daar_projectnumber"] = regProperty.propertyID;
                    if (regProperty.orgID != null && regProperty.orgID != string.Empty)
                    {
                        Guid organizationGUID = getOrganizationGUID(regProperty.orgID);
                        if (organizationGUID != Guid.Empty)
                        {
                            Property["daar_operatingunit"] = new EntityReference("daar_organization", organizationGUID);
                        }
                    }
                    Property["daar_orgid"] = regProperty.orgID;
                    Property["daar_buildinglandid"] = regProperty.buildingLandID!=string.Empty?regProperty.buildingLandID:string.Empty;
                    Property["daar_buildingno"] = regProperty.buildingORLand != string.Empty ? regProperty.buildingORLand : string.Empty;
                    Property["daar_buildinglandcode"] = regProperty.buildingORLandCode != string.Empty ? regProperty.buildingORLandCode : string.Empty;
                    Property["daar_buildinglandalias"] = regProperty.buildingOrLandAlias != string.Empty ? regProperty.buildingOrLandAlias : string.Empty;
                    Property["daar_buildinglandname"] = regProperty.buildingOrLandName != string.Empty ? regProperty.buildingOrLandName : string.Empty;
                    if (regProperty.buildingOrLandStartDate != default(DateTime).ToString() && regProperty.buildingOrLandStartDate != null && regProperty.buildingOrLandStartDate != string.Empty)
                    {
                        Property["daar_buildinglandcodestartdate"] = Convert.ToDateTime(regProperty.buildingOrLandStartDate);
                    }
                    if (regProperty.buildingOrLandEndDate != default(DateTime).ToString() && regProperty.buildingOrLandEndDate != null && regProperty.buildingOrLandEndDate != string.Empty)
                    {
                        Property["daar_buildinglandcodeenddate"] = Convert.ToDateTime(regProperty.buildingOrLandEndDate);
                    }

                    Property["daar_floorparcelid"] = regProperty.floorParcelID != string.Empty ? regProperty.floorParcelID : string.Empty;
                    Property["daar_floor"] = regProperty.floorParcel != string.Empty ? regProperty.floorParcel : string.Empty;
                    Property["daar_floorparcelcode"] = regProperty.floorParcelCode != string.Empty ? regProperty.floorParcelCode : string.Empty;
                    Property["daar_floorparcelalias"] = regProperty.floorParcelAlias != string.Empty ? regProperty.floorParcelAlias : string.Empty;
                    Property["daar_floorparcelname"] = regProperty.floorParcelName != string.Empty ? regProperty.floorParcelName : string.Empty;
                    if (regProperty.floorParcelStartDate != default(DateTime).ToString() && regProperty.floorParcelStartDate != null && regProperty.floorParcelStartDate != string.Empty)
                    {
                        Property["daar_floorparcelstartdate"] = Convert.ToDateTime(regProperty.floorParcelStartDate);
                    }
                    if (regProperty.floorParcelEndDate != default(DateTime).ToString() && regProperty.floorParcelEndDate != null && regProperty.floorParcelEndDate != string.Empty)
                    {
                        Property["daar_floorparcelenddate"] = Convert.ToDateTime(regProperty.floorParcelEndDate);
                    }
                    Property["name"]= regProperty.unitOrSection != string.Empty ? regProperty.unitOrSection : string.Empty;
                    Property["daar_unitno"] = regProperty.unitOrSection != string.Empty ? regProperty.unitOrSection : string.Empty;
                    Property["daar_unitsectioncode"] = regProperty.unitOrSectionCode != string.Empty ? regProperty.unitOrSectionCode : string.Empty;
                    Property["daar_unitsectionalias"] = regProperty.unitOrSectionAlias != string.Empty ? regProperty.unitOrSectionAlias : string.Empty;
                    Property["daar_unitsectionname"] = regProperty.unitOrSectionName != string.Empty ? regProperty.unitOrSectionName : string.Empty;
                    if (regProperty.unitOrSectionStartDate != default(DateTime).ToString() && regProperty.unitOrSectionStartDate != null && regProperty.unitOrSectionStartDate != string.Empty)
                    {
                        Property["daar_unitsectionstartdate"] = Convert.ToDateTime(regProperty.unitOrSectionStartDate);
                    }
                    if (regProperty.unitOrSectionEndDate != default(DateTime).ToString() && regProperty.unitOrSectionEndDate != null && regProperty.unitOrSectionEndDate != string.Empty)
                    {
                        Property["daar_unitsectionenddate"] = Convert.ToDateTime(regProperty.unitOrSectionEndDate);
                    }
                    Property["daar_titledeedareasqft"] = regProperty.titleDeedAreaSQFT;
                    Property["daar_balcony_suare_feet"] = regProperty.balconyAreaSQFT;
                    Property["daar_saleable_area_suare_feet"] = regProperty.saleableAreaSQFT;
                    Property["daar_titledeedareasqm"] = regProperty.titleDeedAreaSQM;
                    Property["daar_balcony_suare_mtr"] = regProperty.balconyAreaSQM;
                    Property["daar_saleableareasqm"] = regProperty.saleableAreaSQM;
                    Property["daar_unittype"] = regProperty.unitType != string.Empty ? regProperty.unitType : string.Empty;
                    Property["daar_model"] = regProperty.unitModel!=string.Empty? regProperty.unitModel:string.Empty;
                    Property["daar_unit_facing"] = regProperty.unitFacing!=string.Empty? regProperty.unitFacing:string.Empty;
                    Property["daar_unit_position"] = regProperty.unitPosition!=string.Empty? regProperty.unitPosition:string.Empty;
                    Property["daar_road_size"] = regProperty.roadSize!=string.Empty? regProperty.roadSize:string.Empty;
                    Property["daar_title_deed_number"] = regProperty.titleDeedNumber!=string.Empty? regProperty.titleDeedNumber:string.Empty;
                    Property["daar_titledeeddate"] = regProperty.titleDeedDate!=string.Empty? regProperty.titleDeedDate:string.Empty;
                    Property["daar_bankname"] = regProperty.bankName != string.Empty ? regProperty.bankName : string.Empty;
                    Property["daar_bankaccounttype"] = regProperty.bankAccountType != string.Empty ? regProperty.bankAccountType : string.Empty;
                    Property["daar_bankiban"] = regProperty.bankIBAN != string.Empty ? regProperty.bankIBAN : string.Empty;
                    Property["daar_bankaccount"] = regProperty.bankAccount != string.Empty ? regProperty.bankAccount : string.Empty;
                    Property["daar_apartment_view"] = regProperty.apartmentView!=string.Empty? regProperty.apartmentView:string.Empty;
                    Property["daar_apartment_type"] = regProperty.apartmentType!=string.Empty? regProperty.apartmentType:string.Empty;
                    Property["daar_num_of_car_parking"] = regProperty.numOfCarParking!=string.Empty? regProperty.numOfCarParking:string.Empty;
                    Property["daar_maid_room"] = regProperty.maidRoom!=string.Empty? regProperty.maidRoom:string.Empty;
                    Property["daar_study_room"] = regProperty.studyRoom != string.Empty ? regProperty.studyRoom : string.Empty;
                    Property["daar_numofrooms"] = regProperty.numOfRooms!=string.Empty? regProperty.numOfRooms:string.Empty;
                    Property["daar_furnitured"] = regProperty.furnitured!=string.Empty? regProperty.furnitured:string.Empty;
                    Property["daar_terracearea"] = regProperty.terraceArea != string.Empty ? regProperty.terraceArea : string.Empty;
                    Property["daar_maxgfa"] = regProperty.MAXGFA != string.Empty ? regProperty.MAXGFA : string.Empty;
                    Property["daar_shellcore"] = regProperty.shellCore != string.Empty ? regProperty.shellCore : string.Empty;
                    Property["daar_unit_or_section"] = regProperty.unitPlan!=string.Empty? regProperty.unitPlan:string.Empty;
                    Property["daar_vatusage"] = regProperty.VATUsage != string.Empty ? regProperty.VATUsage : string.Empty;
                    Property["daar_vatpercentage"] = regProperty.VATPercentage;
                    Property["price"] = new Money(regProperty.sellingPrice);
                    Property["daar_selling_price_ft"] = regProperty.sellingPriceSQFT;
                    Property["daar_selling_price_smt"] = regProperty.sellingPriceSQM;
                    Property["currentcost"] = new Money(regProperty.basePrice);
                    Property["daar_base_price_ft"] = regProperty.basePriceSQFT;
                    Property["daar_base_price_smt"] = regProperty.basePriceSQM;
                    Property["daar_graceperiod"] = regProperty.gracePeriod;
                    Property["daar_tokendepositamount"] = new Money(regProperty.tokenDepositAmount);
                    Property["daar_downpaymentpercentage"] = regProperty.downPaymentPercentage;
                    //Property["daar_paymentplanid"] = regProperty.paymentPlanID!=string.Empty? regProperty.paymentPlanID:string.Empty;
                    Property["daar_unitstatus"] = regProperty.unitStatus != string.Empty ? regProperty.unitStatus : string.Empty;
                    Property["daar_releaseid"] = regProperty.releaseID != string.Empty ? regProperty.releaseID : string.Empty;
                    //Property["defaultuomscheduleid"] = new EntityReference("uomschedule", new Guid(SystemUnitGroup));
                    //Property["defaultuomid"] = new EntityReference("uom", new Guid(SystemDefaultUnit));
                    Property["daar_applicable_discount"] = regProperty.applicableDiscount;

                    //Property["daar_project"] = regProperty.daar_projectName!=string.Empty?regProperty.daar_projectName:string.Empty;
                    //Property["name"] = regProperty.daar_projectName!=string.Empty?regProperty.daar_projectName:string.Empty;
                    //Property["daar_businesstype"] = new OptionSetValue(regProperty.businessType);
                    //Property["daar_propertystatus"] = new OptionSetValue(regProperty.propertyStatus);

                    //Property["daar_unitno"] = regProperty.unitNumber!=string.Empty? regProperty.unitNumber:string.Empty;
                    //Property["daar_type"] = new OptionSetValue(regProperty.type);
                    //Property["daar_netareasqm"] = regProperty.netAreaSQM;
                    //Property["daar_totalareasqm"] = regProperty.totalAreaSQM;


                    //Property["daar_country"] = regProperty.daar_country;


                    //Property["daar_floor_parcel_id"] = regProperty.daar_floor_parcel_id;

                    //Property["daar_org_id"] = regProperty.daar_org_id;
                    //if (regProperty.daar_title_deed_transfer_date != default(DateTime).ToString() && regProperty.daar_title_deed_transfer_date!=null && regProperty.daar_title_deed_transfer_date!=string.Empty)
                    //{
                    //    Property["daar_title_deed_transfer_date"] = Convert.ToDateTime(regProperty.daar_title_deed_transfer_date);
                    //}
                    //Property["daar_source_of_area"] = regProperty.daar_source_of_area;

                    //Property["daar_floor"] = regProperty.daar_floor;



                    //Property["daar_net_area_suare_feet"] = regProperty.daar_net_area_suare_feet;

                    //Property["daar_terrace_suare_feet"] = regProperty.daar_terrace_suare_feet;
                    //Property["daar_total_area_suare_feet"] = regProperty.daar_total_area_suare_feet;


                    //Property["daar_bedroom"] = regProperty.daar_bedroom;










                    //Property["daar_plot_number"] = regProperty.daar_plot_number;

                    //Property["daar_plot_area_deed"] = regProperty.daar_plot_area_deed;
                    //Property["daar_property_id"] = regProperty.propertyID;
                    //Property["daar_site_number"] = regProperty.siteNumber;
                    //Property["daar_propertyactive"] = new OptionSetValue(regProperty.daar_propertyactive);
                    //Property["daar_buildingactive"] = new OptionSetValue(regProperty.daar_buildingactive);
                    //Property["daar_flooractive"] = new OptionSetValue(regProperty.daar_flooractive);
                    //Property["daar_unitactive"] = new OptionSetValue(regProperty.daar_unitactive);
                    //Property["daar_escrowibanaccountnumber"] = regProperty.daar_escrowibanaccountnumber != string.Empty ? regProperty.daar_escrowibanaccountnumber : string.Empty;
                    //Property["daar_escrowaccountnumber"] = regProperty.daar_escrowaccountnumber != string.Empty ? regProperty.daar_escrowaccountnumber : string.Empty;
                    //Property["daar_paymentterm"] = regProperty.daar_paymentterm != string.Empty ? regProperty.daar_paymentterm : string.Empty;
                    //Property["daar_reservationperiodindays"] = regProperty.daar_reservationperiodindays;
                    //Property["daar_bookingadminfeepercentage"] = regProperty.daar_bookingadminfeepercentage;


                }

                UpsertRequest request = new UpsertRequest()
                {
                    Target = Property
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    regProperty.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    regProperty.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();
            }
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            return regProperty;
        }

        [HttpPost]
        [Route("Account")]
        public createUpdateAccount CreateOrg(createUpdateAccount orgAccount)
        {
            #region Try
            try
            {
                Entity Account = new Entity();
                if (orgAccount != null)
                {
                    if (!string.IsNullOrEmpty(orgAccount.customerNumber))
                    {
                        Account = new Entity("account", "accountnumber", orgAccount.customerNumber);
                    }

                    Account["daar_createdfromoracle"] = true; //
                    //Account["accountnumber"] = orgAccount.customerNumber != string.Empty ? orgAccount.customerNumber : string.Empty;
                    //Account["daar_oraclecustomerid"] = orgAccount.oracleCustomerID != string.Empty ? orgAccount.oracleCustomerID : string.Empty;
                    Account["daar_oraclecustomernumber"] = orgAccount.salesCustomerID != string.Empty ? orgAccount.salesCustomerID : string.Empty;
                    #region Customer Type
                    if (orgAccount.customerType == "I")
                    {
                        Account["daar_customertype"] = new OptionSetValue(1); //1-Individual
                    }
                    else if (orgAccount.customerType == "O")
                    {
                        Account["daar_customertype"] = new OptionSetValue(2); //1-Organization
                    }
                    #endregion
                    #region Customer Status
                    if (orgAccount.customerStatus == "A")
                    {
                        Account["statecode"] = new OptionSetValue(1); //1-Individual
                    }
                    else if (orgAccount.customerType == "I")
                    {
                        Account["statecode"] = new OptionSetValue(0); //1-Organization
                    }
                    #endregion
                    Account["daar_countryofresidence"] = orgAccount.countryOfResidence!=string.Empty?orgAccount.countryOfResidence:string.Empty;
                    Account["address1_name"] = orgAccount.city!=string.Empty?orgAccount.city:string.Empty;
                    #region Primary Phone
                    #region Telephone Type
                    if (orgAccount.telephoneTypeNo == "MOBILE")
                    {
                        Account["daar_telephonetype"] = new OptionSetValue(1); //2-Mobile
                    }
                    else if (orgAccount.telephoneTypeNo == "GEN")
                    {
                        Account["daar_telephonetype"] = new OptionSetValue(2); //1-Telephone
                    }
                    else if (orgAccount.telephoneTypeNo == "FAX")
                    {
                        Account["daar_telephonetype"] = new OptionSetValue(3); //1-Fax
                    }
                    #endregion

                    #region Phone Country Code
                    if(orgAccount.phoneCountryCode!=null && orgAccount.phoneCountryCode!=string.Empty)
                    {
                        Guid phoneCountryCodeGUID = getPhoneCountryCodeGUID(orgAccount.phoneCountryCode);
                        if (phoneCountryCodeGUID != Guid.Empty)
                        {
                            Account["daar_phonecountrycode"] = new EntityReference("daar_country_phone_code", phoneCountryCodeGUID);
                        }
                    }
                    #endregion
                    Account["daar_phoneareacode"] = orgAccount.phoneAreaCode!=string.Empty?orgAccount.phoneAreaCode:string.Empty;
                    Account["telephone1"] = orgAccount.phoneNumber!=string.Empty?orgAccount.phoneNumber:string.Empty;
                    #endregion

                    #region Secondary Phone
                    #region Alternate Telephone Type
                    if (orgAccount.secondaryPhoneTypeNo == "MOBILE")
                    {
                        Account["daar_secondarytelephone"] = new OptionSetValue(1); //2-Mobile
                    }
                    else if (orgAccount.secondaryPhoneTypeNo == "GEN")
                    {
                        Account["daar_secondarytelephone"] = new OptionSetValue(2); //1-Telephone
                    }
                    else if (orgAccount.secondaryPhoneTypeNo == "FAX")
                    {
                        Account["daar_secondarytelephone"] = new OptionSetValue(3); //1-Fax
                    }
                    #endregion

                    #region Alt Phone Country Code
                    if (orgAccount.altphoneAreaCode != null && orgAccount.altphoneAreaCode != string.Empty)
                    {
                        Guid altphoneCountryCodeGUID = getPhoneCountryCodeGUID(orgAccount.altCountryCode);
                        if (altphoneCountryCodeGUID != Guid.Empty)
                        {
                            Account["daar_altphonecountrycode"] = new EntityReference("daar_country_phone_code", altphoneCountryCodeGUID);
                        }
                    }
                    #endregion
                    Account["daar_altphoneareacode"] = orgAccount.phoneAreaCode!=string.Empty? orgAccount.phoneAreaCode:string.Empty;
                    Account["address1_telephone2"] = orgAccount.phoneNumber!=string.Empty? orgAccount.phoneNumber:string.Empty;
                    #endregion
                    Account["emailaddress1"] = orgAccount.emailAddress!=string.Empty? orgAccount.emailAddress:string.Empty;
                    Account["emailaddress2"] = orgAccount.emailAddress2!=string.Empty? orgAccount.emailAddress2:string.Empty;
                    Account["websiteurl"] = orgAccount.website!=string.Empty? orgAccount.website:string.Empty;
                    Account["daar_trnnumber"] = orgAccount.trnNumber != string.Empty ? orgAccount.trnNumber : string.Empty ;
                    #region Broker
                    if (orgAccount.brokerNumber != null && orgAccount.brokerNumber != string.Empty)
                    {
                        Guid brokerGUID = getBrokerGUID(orgAccount.brokerNumber);
                        if (brokerGUID != Guid.Empty)
                        {
                            Account["daar_broker"] = new EntityReference("daar_broker", brokerGUID);
                        }
                    }
                    #endregion
                    #region Title
                    if (orgAccount.titleNo == "DR.")
                    {
                        Account["daar_title"] = new OptionSetValue(1); //2-Dr.
                    }
                    else if (orgAccount.titleNo == "MISS")
                    {
                        Account["daar_title"] = new OptionSetValue(2); //2-Miss
                    }
                    else if (orgAccount.titleNo == "MR.")
                    {
                        Account["daar_title"] = new OptionSetValue(3); //3-Mr.
                    }
                    else if (orgAccount.titleNo == "MRS.")
                    {
                        Account["daar_title"] = new OptionSetValue(4); //4-Mrs
                    }
                    else if (orgAccount.titleNo == "MS.")
                    {
                        Account["daar_title"] = new OptionSetValue(5); //5-Ms.
                    }
                    else if (orgAccount.titleNo == "SIR")
                    {
                        Account["daar_title"] = new OptionSetValue(6); //6-Sir
                    }
                    #endregion
                    Account["daar_firstname"] = orgAccount.firstName!=string.Empty? orgAccount.firstName:string.Empty;
                    Account["daar_middlename"] = orgAccount.middleName!=string.Empty? orgAccount.middleName:string.Empty;
                    Account["daar_lastname"] = orgAccount.lastName!=string.Empty? orgAccount.lastName:string.Empty;
                    Account["name"] = orgAccount.accountName!=string.Empty? orgAccount.accountName:string.Empty;
                    Account["daar_accountnamearabic"] = orgAccount.arabicCompanyName!=string.Empty? orgAccount.arabicCompanyName:string.Empty;
                    Account["daar_nationality"] = orgAccount.nationality!=string.Empty? orgAccount.nationality:string.Empty;
                    if (orgAccount.customerDateofBirthGreg != default(DateTime).ToString() && orgAccount.customerDateofBirthGreg != null && orgAccount.customerDateofBirthGreg!=string.Empty)
                    {
                        Account["daar_dateofbirthgreg"] = Convert.ToDateTime(orgAccount.customerDateofBirthGreg);
                    }
                    Account["daar_hijridateofbirth"] = orgAccount.customerDateofBirthHijri!=string.Empty? orgAccount.customerDateofBirthHijri:string.Empty;
                    #region Gender
                    if (orgAccount.gender == "F")
                    {
                        Account["daar_gender"] = new OptionSetValue(2); //2-Female
                    }
                    else if (orgAccount.gender == "M")
                    {
                        Account["daar_gender"] = new OptionSetValue(1); //1-Male
                    }
                    else if (orgAccount.gender == "N")
                    {
                        Account["daar_gender"] = new OptionSetValue(3); //1-Unspecified
                    }
                    else if (orgAccount.gender == "U")
                    {
                        Account["daar_gender"] = new OptionSetValue(4); //1-Unknown
                    }
                    #endregion
                    #region Customer ID TYpe
                    if (orgAccount.customerIDType == "1")
                    {
                        Account["daar_customeridtype"] = new OptionSetValue(1); //1-National ID
                    }
                    else if (orgAccount.customerIDType == "2")
                    {
                        Account["daar_customeridtype"] = new OptionSetValue(2); //2-Driving License
                    }
                    else if (orgAccount.customerIDType == "3")
                    {
                        Account["daar_customeridtype"] = new OptionSetValue(3); //3-Passport
                    }
                    else if (orgAccount.customerIDType == "4")
                    {
                        Account["daar_customeridtype"] = new OptionSetValue(4); //4-Resident Identity
                    }
                    #endregion
                    Account["daar_customeridnumber"] = orgAccount.customerIdNumber!=string.Empty? orgAccount.customerIdNumber:string.Empty;
                    if (orgAccount.customerIDIssuedDate != default(DateTime).ToString() && orgAccount.customerIDIssuedDate != null && orgAccount.customerIDIssuedDate!=string.Empty)
                    {
                        Account["daar_customeridissueddate"] = Convert.ToDateTime(orgAccount.customerIDIssuedDate);
                    }
                    Account["daar_hijricustomeridissueddate"] = orgAccount.hijriIDIssueDate!=string.Empty? orgAccount.hijriIDIssueDate:string.Empty;
                    if (orgAccount.customerIDExpiryDate != default(DateTime).ToString() && orgAccount.customerIDExpiryDate != null && orgAccount.customerIDExpiryDate!=string.Empty)
                    {
                        Account["daar_customeridexpirydate"] = Convert.ToDateTime(orgAccount.customerIDExpiryDate);
                    }
                    Account["daar_hijricustomeridexpirydate"] = orgAccount.hijriIDExpiryDate!=string.Empty?orgAccount.hijriIDExpiryDate:string.Empty;
                    Account["daar_placeofissue"] = orgAccount.placeOfIssue!=string.Empty? orgAccount.placeOfIssue:string.Empty;
                    Account["daar_passportnumber"] = orgAccount.passportNumber!=string.Empty? orgAccount.passportNumber:string.Empty;
                    if (orgAccount.passportIssueDate != default(DateTime).ToString() && orgAccount.passportIssueDate != null && orgAccount.passportIssueDate!=string.Empty)
                    {
                        Account["daar_passportissuedate"] = Convert.ToDateTime(orgAccount.passportIssueDate);
                    }
                    if (orgAccount.passportExpiryDate != default(DateTime).ToString() && orgAccount.passportExpiryDate != null && orgAccount.passportExpiryDate!=string.Empty)
                    {
                        Account["daar_passportexpirydate"] = Convert.ToDateTime(orgAccount.passportExpiryDate);
                    }
                    #region Legal Authorized
                    if (orgAccount.legallyAuthrorized == "N")
                    {
                        Account["daar_legallyauthorized"] = new OptionSetValue(2); //2=No
                    }
                    else if (orgAccount.legallyAuthrorized == "Y")
                    {
                        Account["daar_legallyauthorized"] = new OptionSetValue(1); //1=Yes
                    }
                    #endregion
                    Account["daar_legalagentname"] = orgAccount.legalAgentName!=string.Empty? orgAccount.legalAgentName:string.Empty;
                    Account["daar_agentfunctionality"] = orgAccount.agentFunctionality!=string.Empty? orgAccount.agentFunctionality:string.Empty;
                    Account["daar_agencytype"] = orgAccount.agencyType!=string.Empty? orgAccount.agencyType:string.Empty;
                    Account["daar_agencyregistrationnumber"] = orgAccount.agencyRegistrationNo!=string.Empty? orgAccount.agencyRegistrationNo:string.Empty;
                    if (orgAccount.agencyExpiryDate != default(DateTime).ToString() && orgAccount.agencyExpiryDate != null && orgAccount.agencyExpiryDate!=string.Empty)
                    {
                        Account["daar_agencyexpirydate"] = Convert.ToDateTime(orgAccount.agencyExpiryDate);
                    }
                    Account["daar_agencyexpirydatehijri"] = orgAccount.hijriAgencyExpiryDate!=string.Empty? orgAccount.hijriAgencyExpiryDate:string.Empty;
                    Account["daar_legalremarks"] = orgAccount.legalRemarks!=string.Empty? orgAccount.legalRemarks:string.Empty;
                    Account["daar_companyname"] = orgAccount.companyName!=string.Empty? orgAccount.companyName:string.Empty;
                    if(orgAccount.natureOfBusinessNo=="1")
                    {
                        Account["daar_natureofbusiness"] = new OptionSetValue(Convert.ToInt32(orgAccount.natureOfBusinessNo));
                    }
                    if (orgAccount.licenseSourceNo == "1")
                    {
                        Account["daar_licensesource"] = new OptionSetValue(Convert.ToInt32(orgAccount.licenseSourceNo));
                    }
                    if (orgAccount.licenseTypeNo == "1")
                    {
                        Account["daar_licensetype"] = new OptionSetValue(Convert.ToInt32(orgAccount.licenseTypeNo));
                    }
                    Account["daar_tradelicensenumber"] = orgAccount.tradeLicenseNo!=string.Empty? orgAccount.tradeLicenseNo:string.Empty;
                    if(orgAccount.tradeLicenseCountryCode!=null && orgAccount.tradeLicenseCountryCode!=string.Empty)
                    {
                        Guid tradeLiceCountryGUID = getCountryGUID(orgAccount.tradeLicenseCountryCode);
                        if (tradeLiceCountryGUID != Guid.Empty)
                        {
                            Account["daar_tradelicensecountry"] = new EntityReference("daar_country", tradeLiceCountryGUID);
                        }
                    }
                    if (orgAccount.tradeLicenseIssueDate != default(DateTime).ToString() && orgAccount.tradeLicenseIssueDate != null && orgAccount.tradeLicenseIssueDate!=string.Empty)
                    {
                        Account["daar_tradelicenseissuedate"] = Convert.ToDateTime(orgAccount.tradeLicenseIssueDate);
                    }
                    Account["daar_tradelicenseissuedatehijri"] = orgAccount.hijriTradeLicIssueDate!=string.Empty? orgAccount.hijriTradeLicIssueDate:string.Empty;
                    if (orgAccount.tradeLicenseExpiryDate != default(DateTime).ToString() && orgAccount.tradeLicenseExpiryDate != null && orgAccount.tradeLicenseExpiryDate!=string.Empty)
                    {
                        Account["daar_tradelicenseexpirydate"] = Convert.ToDateTime(orgAccount.tradeLicenseExpiryDate);
                    }
                    Account["daar_tradelicenseexpirydatehijri"] = orgAccount.hijriTradeLicExpiryDate!=string.Empty? orgAccount.hijriTradeLicExpiryDate:string.Empty;
                    Account["daar_reracertificatenumber"] = orgAccount.reraCertificateNo!=string.Empty? orgAccount.reraCertificateNo:string.Empty;
                    if (orgAccount.RERAStartDate != default(DateTime).ToString() && orgAccount.RERAStartDate != null && orgAccount.RERAStartDate!=string.Empty)
                    {
                        Account["daar_rerastartdate"] = Convert.ToDateTime(orgAccount.RERAStartDate);
                    }
                    if (orgAccount.RERAExpiryDate != default(DateTime).ToString() && orgAccount.RERAExpiryDate != null && orgAccount.RERAExpiryDate!=string.Empty)
                    {
                        Account["daar_reraexpirydate"] = Convert.ToDateTime(orgAccount.RERAExpiryDate);
                    }

                    #region Home Address
                    Account["address1_line1"] = orgAccount.homeAddress1!=string.Empty? orgAccount.homeAddress1:string.Empty;
                    Account["address1_line2"] = orgAccount.homeAddress2!=string.Empty? orgAccount.homeAddress2:string.Empty;
                    Account["address1_line3"] = orgAccount.homeAddress3!=string.Empty? orgAccount.homeAddress3:string.Empty;
                    Account["address1_city"] = orgAccount.homeCity!=string.Empty? orgAccount.homeCity:string.Empty;
                    Account["address1_stateorprovince"] = orgAccount.homeState!=string.Empty? orgAccount.homeState:string.Empty;
                    if(orgAccount.homeCountry!=string.Empty && orgAccount.homeCountry!=null)
                    {
                        Guid homeCountryGUID = getCountryGUID(orgAccount.homeCountry);
                        if (homeCountryGUID != Guid.Empty)
                        {
                            Account["daar_homecountry"] = new EntityReference("daar_country", homeCountryGUID);
                        }
                    } 
                    Account["address1_postofficebox"] = orgAccount.homePOBox!=string.Empty? orgAccount.homePOBox:string.Empty;
                    Account["address1_postalcode"] = orgAccount.homePostalCode!=string.Empty? orgAccount.homePostalCode:string.Empty;
                    #endregion

                    #region Office Address
                    Account["address2_line1"] = orgAccount.officeAddress1!=string.Empty? orgAccount.officeAddress1:string.Empty;
                    Account["address2_line2"] = orgAccount.officeAddress2!=string.Empty? orgAccount.officeAddress2:string.Empty;
                    Account["address2_line3"] = orgAccount.officeAddress3!=string.Empty? orgAccount.officeAddress3:string.Empty;
                    Account["address2_city"] = orgAccount.officeCity!=string.Empty? orgAccount.officeCity:string.Empty;
                    Account["address2_stateorprovince"] = orgAccount.officeState!=string.Empty? orgAccount.officeState:string.Empty;
                    if(orgAccount.officeCountry!=null && orgAccount.officeCountry!=string.Empty)
                    {
                        Guid officeCountryGUID = getCountryGUID(orgAccount.officeCountry);
                        if (officeCountryGUID != Guid.Empty)
                        {
                            Account["daar_officecountry"] = new EntityReference("daar_country", officeCountryGUID);
                        }
                    }
                    
                    Account["address2_postofficebox"] = orgAccount.officePOBox!=string.Empty? orgAccount.officePOBox:string.Empty;
                    Account["address2_postalcode"] = orgAccount.officePostalCode!=string.Empty? orgAccount.officePostalCode:string.Empty;
                    #endregion

                    #region Arabic Address
                    Account["daar_address1"] = orgAccount.arabicAddress1!=string.Empty? orgAccount.arabicAddress1:string.Empty;
                    Account["daar_address2"] = orgAccount.arabicAddress2!=string.Empty? orgAccount.arabicAddress2:string.Empty;
                    Account["daar_address3"] = orgAccount.arabicAddress3!=string.Empty? orgAccount.arabicAddress3:string.Empty;
                    Account["daar_city"] = orgAccount.arabicCity!=string.Empty? orgAccount.arabicCity:string.Empty;
                    Account["daar_arabicstate"] = orgAccount.arabicState!=string.Empty? orgAccount.arabicState:string.Empty;
                    Account["daar_country"] = orgAccount.arabicCountry!=string.Empty? orgAccount.arabicCountry:string.Empty;
                    Account["daar_pobox"] = orgAccount.arabicPOBox!=string.Empty? orgAccount.arabicPOBox:string.Empty;
                    Account["daar_postalcodezipcode"] = orgAccount.arabicPostalCode!=string.Empty? orgAccount.arabicPostalCode:string.Empty;
                    #endregion

                   

                    //Guid AccountGuid = service.Create(Account);
                    //orgAccount.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                }
                UpsertRequest request = new UpsertRequest()
                {
                    Target = Account
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    orgAccount.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    orgAccount.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();
            }
            #endregion
            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgAccount;
        }

        [HttpPost]
        [Route("Broker")]
        public createUpdateBroker CreateUpdateBroker(createUpdateBroker orgBroker)
        {
            #region Try
            try
            {
                Entity Broker = new Entity();
                if (orgBroker != null)
                {
                    if (!string.IsNullOrEmpty(orgBroker.brokerNumber))
                    {
                        Broker = new Entity("daar_broker", "daar_brokernumber", orgBroker.brokerNumber);
                    }
                    Broker["daar_oraclebrokerid"] = orgBroker.brokerID != string.Empty ? orgBroker.brokerID : string.Empty;
                    if(orgBroker.brokerType=="I")
                    {
                        Broker["daar_brokertype"] = new OptionSetValue(1); //1-Individual
                    }
                    else if(orgBroker.brokerType=="O")
                    {
                        Broker["daar_brokertype"] = new OptionSetValue(2); //2-Organization
                    }
                    Broker["daar_name"] = orgBroker.brokerName != string.Empty ? orgBroker.brokerName : string.Empty;
                    Broker["daar_nameinarabic"] = orgBroker.brokerNameArabic != string.Empty ? orgBroker.brokerNameArabic : string.Empty;
                    if (orgBroker.contractDate != default(DateTime).ToString() && orgBroker.contractDate!=null && orgBroker.contractDate!=string.Empty)
                    {
                        Broker["daar_contractdate"] = Convert.ToDateTime(orgBroker.contractDate);
                    }
                    Broker["daar_contracthijridate"] = orgBroker.hijriContractDate != string.Empty ? orgBroker.hijriContractDate : string.Empty;
                    if (orgBroker.countryCode != null && orgBroker.countryCode != string.Empty)
                    {
                        Guid countryGUID = getCountryGUID(orgBroker.countryCode);
                        if (countryGUID != Guid.Empty)
                        {
                            Broker["daar_countryc"] = new EntityReference("daar_country", countryGUID);
                        }
                    }
                    if (orgBroker.activeFlag == "Y")
                    {
                        Broker["statecode"] = new OptionSetValue(1);
                    }
                    else if (orgBroker.activeFlag == "N")
                    {
                        Broker["statecode"] = new OptionSetValue(0);
                    }
                    if (orgBroker.contractStatus == "COMPLETE")
                    {
                        Broker["daar_contractstatus"] = new OptionSetValue(1);
                    }
                    else if (orgBroker.contractStatus == "Cancel")
                    {
                        Broker["daar_contractstatus"] = new OptionSetValue(7);
                    }
                    else if (orgBroker.contractStatus == "LEGAL")
                    {
                        Broker["daar_contractstatus"] = new OptionSetValue(4);
                    }
                    else if (orgBroker.contractStatus == "PC")
                    {
                        Broker["daar_contractstatus"] = new OptionSetValue(5);
                    }
                    else if (orgBroker.contractStatus == "PENDING")
                    {
                        Broker["daar_contractstatus"] = new OptionSetValue(2);
                    }
                    else if (orgBroker.contractStatus == "READY")
                    {
                        Broker["daar_contractstatus"] = new OptionSetValue(3);
                    }
                    else if (orgBroker.contractStatus == "SIGNED")
                    {
                        Broker["daar_contractstatus"] = new OptionSetValue(6);
                    }
                    if (orgBroker.contractExpiryDate != default(DateTime).ToString() && orgBroker.contractExpiryDate != null && orgBroker.contractExpiryDate != string.Empty)
                    {
                        Broker["daar_contractexpiredate"] = Convert.ToDateTime(orgBroker.contractExpiryDate);
                    }

                    //Broker["daar_contractstatus"] = new OptionSetValue(orgBroker.contractStatus);

                    //Broker["daar_idtype"] = new OptionSetValue(orgBroker.IDType);
                    //Broker["daar_idnumber"] = orgBroker.IDNumber!=string.Empty? orgBroker.IDNumber:string.Empty;
                    //if (orgBroker.IDExpiryDate != default(DateTime).ToString() && orgBroker.IDExpiryDate!=null && orgBroker.IDExpiryDate!=string.Empty)
                    //{
                    //    Broker["daar_idexpirydate"] = Convert.ToDateTime(orgBroker.IDExpiryDate);
                    //}
                    //Broker["daar_nationality"] = orgBroker.Nationality!=string.Empty? orgBroker.Nationality:string.Empty;
                    //Broker["daar_addressline1"] = orgBroker.addressLine1!=string.Empty? orgBroker.addressLine1:string.Empty;
                    //Broker["daar_addressline2"] = orgBroker.addressLine2!=string.Empty? orgBroker.addressLine2:string.Empty;
                    //Broker["daar_addressline3"] = orgBroker.addressLine3!=string.Empty? orgBroker.addressLine3:string.Empty;
                    //Broker["daar_addressline4"] = orgBroker.addressLine4!=string.Empty? orgBroker.addressLine4:string.Empty;
                    //Broker["daar_emailaddress"] = orgBroker.emailAddress!=string.Empty? orgBroker.emailAddress:string.Empty;
                    //Broker["daar_countrycode"] = orgBroker.countryCode!=string.Empty? orgBroker.countryCode:string.Empty;

                    //Broker["daar_areacode"] = orgBroker.areaCode!=string.Empty? orgBroker.areaCode:string.Empty;
                    //Broker["daar_telephonenumber"] = orgBroker.telephoneNumber!=string.Empty? orgBroker.telephoneNumber:string.Empty;
                    //Broker["daar_city"] = orgBroker.city!=string.Empty? orgBroker.city:string.Empty;
                    //Broker["daar_cityarabic"] = orgBroker.cityArabic!=string.Empty? orgBroker.cityArabic:string.Empty;
                    //Broker["daar_state"] = orgBroker.state!=string.Empty? orgBroker.state:string.Empty;
                    //Broker["daar_postalcode"] = orgBroker.postalCode!=string.Empty? orgBroker.postalCode:string.Empty;
                    //Broker["daar_country"] = orgBroker.Country!=string.Empty? orgBroker.Country:string.Empty;
                    //Broker["daar_countryarabic"] = orgBroker.countryArabic!=string.Empty? orgBroker.countryArabic:string.Empty;

                    //Broker["daar_tradelicensenumber"] = orgBroker.tradeLicenseNumber!=string.Empty? orgBroker.tradeLicenseNumber:string.Empty;
                    //if (orgBroker.tradeLicenseExpiryDate != default(DateTime).ToString() && orgBroker.tradeLicenseExpiryDate!=null && orgBroker.tradeLicenseExpiryDate!=string.Empty)
                    //{
                    //    Broker["daar_tradelicenseexpirydate"] = Convert.ToDateTime(orgBroker.tradeLicenseExpiryDate);
                    //}
                    //if (orgBroker.ndaSignedDate != default(DateTime).ToString() && orgBroker.ndaSignedDate!=null && orgBroker.ndaSignedDate!=string.Empty)
                    //{
                    //    Broker["daar_ndasigneddate"] = Convert.ToDateTime(orgBroker.ndaSignedDate);
                    //}
                    //if (orgBroker.ndaExpiryDate != default(DateTime).ToString() && orgBroker.ndaExpiryDate!=null && orgBroker.ndaExpiryDate!=string.Empty)
                    //{
                    //    Broker["daar_ndaexpirydate"] = Convert.ToDateTime(orgBroker.ndaExpiryDate);
                    //}
                    //Broker["daar_reranumber"] = orgBroker.reraNumber;
                    //if (orgBroker.reraStartDate != default(DateTime).ToString() && orgBroker.reraStartDate!=null && orgBroker.reraStartDate!=string.Empty)
                    //{
                    //    Broker["daar_rerastartdate"] = Convert.ToDateTime(orgBroker.reraStartDate);
                    //}
                    //if (orgBroker.reraExpiryDate != default(DateTime).ToString() && orgBroker.reraExpiryDate!=null && orgBroker.reraExpiryDate!=string.Empty)
                    //{
                    //    Broker["daar_reraexpirydate"] = Convert.ToDateTime(orgBroker.reraExpiryDate);
                    //}
                    //if (orgBroker.agencyAgreementSignedDate != default(DateTime).ToString() && orgBroker.agencyAgreementSignedDate!=null && orgBroker.agencyAgreementSignedDate!=string.Empty)
                    //{
                    //    Broker["daar_agencyagreementsigneddate"] = Convert.ToDateTime(orgBroker.agencyAgreementSignedDate);
                    //}
                    //if (orgBroker.agencyAgreementExpiryDate != default(DateTime).ToString() && orgBroker.agencyAgreementExpiryDate!=null && orgBroker.agencyAgreementExpiryDate!=string.Empty)
                    //{
                    //    Broker["daar_agencyagreementexpirydate"] = Convert.ToDateTime(orgBroker.agencyAgreementExpiryDate);
                    //}

                }

                UpsertRequest request = new UpsertRequest()
                {
                    Target = Broker
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    orgBroker.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    orgBroker.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgBroker;
        }

        [HttpPost]
        [Route("User")]
        public createUpdateUser CreateUpdateUser(createUpdateUser orgUser)
        {
            #region Try
            try
            {

                Guid defaultBusinessUnitGUID = getDefaultBusinessUnitGUID(service);

                Entity User = new Entity();
                if(orgUser!=null)
                {
                    if(!string.IsNullOrEmpty(orgUser.employeeID))
                    {
                        User = new Entity("systemuser", "daar_employeeid", orgUser.employeeID);
                    }
                    User["domainname"] = orgUser.domainName + "\\" + orgUser.userName;
                    User["firstname"] = orgUser.firstName;
                    User["lastname"] = orgUser.lastName;
                    User["businessunitid"] = new EntityReference("businessunit", defaultBusinessUnitGUID);
                    User["title"] = orgUser.title;
                    User["internalemailaddress"] = orgUser.emailID;
                    User["mobilephone"] = orgUser.mobilePhone;
                    if (orgUser.managerEmployeeID != null && orgUser.managerEmployeeID != string.Empty)
                    {
                        Guid managerID = getManagerGUID(orgUser.managerEmployeeID);
                        if (managerID != Guid.Empty)
                        {
                            User["parentsystemuserid"] = new EntityReference("systemuser", managerID);
                        }
                    }
                    UpsertRequest request = new UpsertRequest()
                    {
                        Target = User
                    };
                    UpsertResponse response = (UpsertResponse)service.Execute(request);

                    if (response.RecordCreated)
                        orgUser.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                    else
                        orgUser.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();
                }
               

                //Entity User = new Entity("systemuser");
                //User["domainname"] = orgUser.domainName + "\\" + orgUser.userName;
                //User["firstname"] = orgUser.firstName;
                //User["lastname"] = orgUser.lastName;
                //User["businessunitid"] = new EntityReference("businessunit", defaultBUGuid);
                //User["title"] = orgUser.title;
                //User["internalemailaddress"] = orgUser.emailID;
                //User["mobilephone"] = orgUser.mobilePhone;
                //        User["daar_employeeid"] = orgUser.employeeID;
                //        if(orgUser.managerEmployeeID!=null && orgUser.managerEmployeeID!=string.Empty)
                //        {
                //            Guid managerID = getManagerGUID(orgUser.managerEmployeeID);
                //            if (managerID != Guid.Empty)
                //            {
                //                User["parentsystemuserid"] = new EntityReference("systemuser", managerID);
                //            }
                //        }

                //        Guid UserId = service.Create(User);

            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgUser;
        }

        [HttpPost]
        [Route("ProposalStatus")]
        public createUpdateProposalStatus ProposalStatusUpdate(createUpdateProposalStatus orgProperty)
        {
            #region Try
            try
            {
                Entity Proposal = new Entity();
                if (orgProperty != null)
                {
                    if (!string.IsNullOrEmpty(orgProperty.proposalID))
                    {
                        Proposal = new Entity("quote", "daar_proposalid", orgProperty.proposalID);
                    }
                    Guid registrationStatusGUID = getRegistrationStatusGUID(orgProperty.status);
                    if (registrationStatusGUID != Guid.Empty)
                    {
                        Proposal["daar_proposalstatus"] = new EntityReference("daar_registrationstatus", registrationStatusGUID);
                    }
                }

                UpsertRequest request = new UpsertRequest()
                {
                    Target = Proposal
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    orgProperty.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    orgProperty.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgProperty;
        }

        [HttpPost]
        [Route("Lead")]
        public createLead CreateLead(createLead orgLead)
        {
            #region Try
            try
            {
                Entity Lead = new Entity("lead");
                Lead["subject"] = orgLead.requirement!=string.Empty? orgLead.requirement:string.Empty;
                Lead["description"] = orgLead.reqDescription!=string.Empty? orgLead.reqDescription:string.Empty;
                Lead["firstname"] = orgLead.firstName!=string.Empty? orgLead.firstName:string.Empty;
                Lead["lastname"] = orgLead.lastName!=string.Empty? orgLead.lastName:string.Empty;
                Lead["daar_leadtype"] = new OptionSetValue(orgLead.leadType);
                Lead["leadsourcecode"] = new OptionSetValue(orgLead.leadSource);
                Lead["leadqualitycode"] = new OptionSetValue(orgLead.rating);
                Lead["telephone1"] = orgLead.businessPhone!=string.Empty? orgLead.businessPhone:string.Empty;
                Lead["mobilephone"] = orgLead.mobilePhone!=string.Empty? orgLead.mobilePhone:string.Empty;
                Lead["emailaddress1"] = orgLead.email!=string.Empty? orgLead.email:string.Empty;
                Guid LeadGuid = service.Create(Lead);
                Lead = service.Retrieve("lead", LeadGuid, new ColumnSet("daar_leadid"));
                orgLead.leadID = Lead.Attributes["daar_leadid"].ToString();
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgLead;
        }

        [HttpPost]
        [Route("TemporaryReceipts")]
        public createTemporaryReceipts CreateTempReceipts(createTemporaryReceipts orgTempReceipts)
        {
            #region Try
            try
            {
                Entity TempReceipts = new Entity();
                TempReceipts = new Entity("daar_temporaryreceipts", "daar_receiptid", orgTempReceipts.receiptID);
                TempReceipts["daar_crmproposalid"] = orgTempReceipts.CRMProposalID != string.Empty? orgTempReceipts.CRMProposalID : string.Empty;
                TempReceipts["daar_accountnumber"] = orgTempReceipts.accountNumber != string.Empty ? orgTempReceipts.accountNumber : string.Empty;
                TempReceipts["daar_registrationid"] = orgTempReceipts.registrationID != string.Empty ? orgTempReceipts.registrationID : string.Empty;
                TempReceipts["daar_reservationno"] = orgTempReceipts.reservationNo != string.Empty ? orgTempReceipts.reservationNo : string.Empty;
                TempReceipts["daar_salescustomerid"] = orgTempReceipts.salesCustomerID != string.Empty ? orgTempReceipts.salesCustomerID : string.Empty;
                TempReceipts["daar_customernumber"] = orgTempReceipts.customerNo != string.Empty ? orgTempReceipts.customerNo : string.Empty;
                TempReceipts["daar_orgid"] = orgTempReceipts.orgID != string.Empty ? orgTempReceipts.orgID : string.Empty;
                //TempReceipts["daar_receiptid"] = orgTempReceipts.receiptID != string.Empty ? orgTempReceipts.receiptID : string.Empty;
                TempReceipts["daar_receiptnumber"] = orgTempReceipts.receiptNumber != string.Empty ? orgTempReceipts.receiptNumber : string.Empty;
                if (orgTempReceipts.receiptDate != default(DateTime).ToString() && orgTempReceipts.receiptDate != null && orgTempReceipts.receiptDate != string.Empty)
                {
                    TempReceipts["daar_receiptdate"] = Convert.ToDateTime(orgTempReceipts.receiptDate);
                }

                TempReceipts["daar_chargetype"] = orgTempReceipts.chargeType != string.Empty ? orgTempReceipts.chargeType : string.Empty;
                TempReceipts["daar_receiptamount"] = new Money(orgTempReceipts.receiptAmount);
                TempReceipts["daar_currencycode"] = orgTempReceipts.currencyCode != string.Empty ? orgTempReceipts.currencyCode : string.Empty;
                TempReceipts["daar_functionalamount"] = new Money(orgTempReceipts.functionalAmount);

                TempReceipts["daar_paymentmethod"] = orgTempReceipts.paymentMethod != string.Empty ? orgTempReceipts.paymentMethod : string.Empty;
                TempReceipts["daar_receiptstatus"] = orgTempReceipts.receiptStatus != string.Empty ? orgTempReceipts.receiptStatus : string.Empty;
                TempReceipts["daar_arcashreceiptid"] = orgTempReceipts.arCashReceiptID != string.Empty ? orgTempReceipts.arCashReceiptID : string.Empty;
                TempReceipts["daar_arreceiptnumber"] = orgTempReceipts.arReceiptNumber != string.Empty ? orgTempReceipts.arReceiptNumber : string.Empty;
                TempReceipts["daar_arvouchernumber"] = orgTempReceipts.arVoucherNumber != string.Empty ? orgTempReceipts.arVoucherNumber : string.Empty;
                if (orgTempReceipts.arReceiptDate != default(DateTime).ToString() && orgTempReceipts.arReceiptDate != null && orgTempReceipts.receiptDate != string.Empty)
                {
                    TempReceipts["daar_arreceiptdate"] = Convert.ToDateTime(orgTempReceipts.receiptDate);
                }
                TempReceipts["daar_advanceinvtrxid"] = orgTempReceipts.advanceInvTrxID != string.Empty ? orgTempReceipts.advanceInvTrxID : string.Empty;
                TempReceipts["daar_advanceinvoiceno"] = orgTempReceipts.advanceInvoiceNo != string.Empty ? orgTempReceipts.advanceInvoiceNo : string.Empty;
                TempReceipts["daar_advancecmtrxid"] = orgTempReceipts.advanceCMTRXID != string.Empty ? orgTempReceipts.advanceCMTRXID : string.Empty;
                TempReceipts["daar_advancecreditmemono"] = orgTempReceipts.advanceCreditMemoNo != string.Empty? orgTempReceipts.advanceCreditMemoNo : string.Empty;               
                Guid customerGUID = getCustomerGUID(orgTempReceipts.customerNo);
                if (customerGUID != Guid.Empty)
                {
                    TempReceipts["daar_accountid"] = new EntityReference("account", customerGUID);
                }

                UpsertRequest request = new UpsertRequest()
                {
                    Target = TempReceipts
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    orgTempReceipts.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    orgTempReceipts.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();

                //orgTempReceipts.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                //Guid TempReceiptGuid = service.Create(TempReceipts);
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgTempReceipts;
        }

        [HttpPost]
        [Route("ActualReceipts")]
        public createActualReceipts CreateActReceipts(createActualReceipts orgActReceipts)
        {
            #region Try
            try
            {
                Entity ActualReceipts = new Entity();
                ActualReceipts = new Entity("daar_actualreceipts", "daar_cashreceiptid", orgActReceipts.cashReceiptID);
                ActualReceipts["daar_crmproposalid"] = orgActReceipts.CRMProposalID != string.Empty ? orgActReceipts.CRMProposalID : string.Empty;
                ActualReceipts["daar_accountnumber"] = orgActReceipts.accountNumber != string.Empty ? orgActReceipts.accountNumber : string.Empty;
                ActualReceipts["daar_registrationid"] = orgActReceipts.registrationID != string.Empty ? orgActReceipts.registrationID : string.Empty;
                ActualReceipts["daar_reservationno"] = orgActReceipts.reservationNo!=string.Empty? orgActReceipts.reservationNo:string.Empty;
                ActualReceipts["daar_salescustomerid"] = orgActReceipts.salesCustomerID != string.Empty ? orgActReceipts.salesCustomerID : string.Empty;
                ActualReceipts["daar_customernumber"] = orgActReceipts.customerNumber != string.Empty ? orgActReceipts.customerNumber : string.Empty;
                //ActualReceipts["daar_cashreceiptid"] = orgActReceipts.cashReceiptID != string.Empty ? orgActReceipts.cashReceiptID : string.Empty;
                ActualReceipts["daar_receiptnumber"] = orgActReceipts.receiptNumber!=string.Empty? orgActReceipts.receiptNumber:string.Empty;
                if (orgActReceipts.receiptDate != default(DateTime).ToString() && orgActReceipts.receiptDate!=null && orgActReceipts.receiptDate!=string.Empty)
                {
                    ActualReceipts["daar_receiptdate"] = Convert.ToDateTime(orgActReceipts.receiptDate);
                }
                if (orgActReceipts.receiptCreationDate != default(DateTime).ToString() && orgActReceipts.receiptCreationDate != null && orgActReceipts.receiptCreationDate != string.Empty)
                {
                    ActualReceipts["daar_receiptcreationdate"] = Convert.ToDateTime(orgActReceipts.receiptCreationDate);
                }
                ActualReceipts["daar_receiptstatus"] = orgActReceipts.receiptStatus != string.Empty ? orgActReceipts.receiptStatus : string.Empty;
                ActualReceipts["daar_amount"] = new Money(orgActReceipts.amount);
                ActualReceipts["daar_currencycode"] = orgActReceipts.currencyCode != string.Empty ? orgActReceipts.currencyCode : string.Empty;
                Guid customerGUID = getCustomerGUID(orgActReceipts.customerNumber);
                if (customerGUID != Guid.Empty)
                {
                    ActualReceipts["daar_accountid"] = new EntityReference("account", customerGUID);
                }

                UpsertRequest request = new UpsertRequest()
                {
                    Target = ActualReceipts
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    orgActReceipts.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    orgActReceipts.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();

                //Guid ActualReceiptsGuid = service.Create(ActualReceipts);
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgActReceipts;
        }

        [HttpPost]
        [Route("SalesContracts")]
        public createSalesContracts CreateSalesContract(createSalesContracts orgSaleContract)
        {
            #region Try
            try
            {
                Entity SalesContract = new Entity("daar_salescontract");
                SalesContract["daar_contractno"] = orgSaleContract.daar_contractno!=string.Empty? orgSaleContract.daar_contractno:string.Empty;
                SalesContract["daar_contracttype"] = orgSaleContract.daar_contracttype!=string.Empty? orgSaleContract.daar_contracttype:string.Empty;
                SalesContract["daar_propertycode"] = orgSaleContract.daar_propertycode!=string.Empty? orgSaleContract.daar_propertycode:string.Empty;
                SalesContract["daar_contractvalue"] = new Money(orgSaleContract.daar_contractvalue);
                if (orgSaleContract.daar_bookeddate != default(DateTime).ToString() && orgSaleContract.daar_bookeddate!=null && orgSaleContract.daar_bookeddate!=string.Empty)
                {
                    SalesContract["daar_bookeddate"] = Convert.ToDateTime(orgSaleContract.daar_bookeddate);
                }
                if (orgSaleContract.daar_solddate != default(DateTime).ToString() && orgSaleContract.daar_solddate!=null && orgSaleContract.daar_solddate!=string.Empty)
                {
                    SalesContract["daar_solddate"] = Convert.ToDateTime(orgSaleContract.daar_solddate);
                }
                if (orgSaleContract.daar_saexecuteddate != default(DateTime).ToString() && orgSaleContract.daar_saexecuteddate!=null && orgSaleContract.daar_saexecuteddate!=string.Empty)
                {
                    SalesContract["daar_saexecuteddate"] = Convert.ToDateTime(orgSaleContract.daar_saexecuteddate);
                }
                Guid customerGUID = getCustomerGUID(orgSaleContract.customerID);
                if (customerGUID != Guid.Empty)
                {
                    SalesContract["daar_accountid"] = new EntityReference("account", customerGUID);
                }
                SalesContract["daar_maintananceeligibility"] = orgSaleContract.daar_maintananceeligibility!=string.Empty? orgSaleContract.daar_maintananceeligibility:string.Empty;
                SalesContract["daar_contractstatus"] = orgSaleContract.daar_contractstatus!=string.Empty? orgSaleContract.daar_contractstatus:string.Empty;

                Guid SalesContractGuid = service.Create(SalesContract);
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgSaleContract;
        }

        [HttpPost]
        [Route("LeasingContracts")]
        public createLeasingContracts CreateLeasingContract(createLeasingContracts orgLeasingContract)
        {
            #region Try
            try
            {
                Entity LeasingContract = new Entity("daar_leasingcontract");
                LeasingContract["daar_contractno"] = orgLeasingContract.daar_contractno!=string.Empty? orgLeasingContract.daar_contractno:string.Empty;
                LeasingContract["daar_contracttype"] = orgLeasingContract.daar_contracttype!=string.Empty? orgLeasingContract.daar_contracttype:string.Empty;
                LeasingContract["daar_propertycode"] = orgLeasingContract.daar_propertycode!=string.Empty? orgLeasingContract.daar_propertycode:string.Empty;
                LeasingContract["daar_contractvalue"] = new Money(orgLeasingContract.daar_contractvalue);
                if (orgLeasingContract.daar_leaseddate != default(DateTime).ToString() && orgLeasingContract.daar_leaseddate!=null && orgLeasingContract.daar_leaseddate!=string.Empty)
                {
                    LeasingContract["daar_leaseddate"] = Convert.ToDateTime(orgLeasingContract.daar_leaseddate);
                }
                if (orgLeasingContract.daar_leasestartdate != default(DateTime).ToString() && orgLeasingContract.daar_leasestartdate!=null && orgLeasingContract.daar_leasestartdate!=string.Empty)
                {
                    LeasingContract["daar_leasestartdate"] = Convert.ToDateTime(orgLeasingContract.daar_leasestartdate);
                }
                if (orgLeasingContract.daar_leaseenddate != default(DateTime).ToString() && orgLeasingContract.daar_leaseenddate!=null && orgLeasingContract.daar_leaseenddate!=string.Empty)
                {
                    LeasingContract["daar_leaseenddate"] = Convert.ToDateTime(orgLeasingContract.daar_leaseenddate);
                }
                Guid customerGUID = getCustomerGUID(orgLeasingContract.customerID);
                if (customerGUID != Guid.Empty)
                {
                    LeasingContract["daar_accountid"] = new EntityReference("account", customerGUID);
                }
                LeasingContract["daar_maintananceeligibility"] = orgLeasingContract.daar_maintananceeligibility!=string.Empty? orgLeasingContract.daar_maintananceeligibility:string.Empty;
                LeasingContract["daar_fitoutperiod"] = orgLeasingContract.daar_fitoutperiod!=string.Empty? orgLeasingContract.daar_fitoutperiod:string.Empty;

                Guid SalesContractGuid = service.Create(LeasingContract);
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgLeasingContract;
        }

        [HttpPost]
        [Route("CustomerInvoice")]
        public createCustomerInvoice CreateCustomerInvoice(createCustomerInvoice orgCustomerInvoice)
        {
            #region Try
            try
            {
                //Entity CustomerInvoice = new Entity("daar_customerinvoice");

                Entity CustomerInvoice = new Entity();
                if (!string.IsNullOrEmpty(orgCustomerInvoice.customertrxID))
                {
                    CustomerInvoice = new Entity("daar_customerinvoice", "daar_customertrxid", orgCustomerInvoice.customertrxID);
                }
                CustomerInvoice["daar_accountnumber"] = orgCustomerInvoice.accountNumber != string.Empty ? orgCustomerInvoice.accountNumber : string.Empty;
                CustomerInvoice["daar_crmproposalid"] = orgCustomerInvoice.CRMProposalID != string.Empty ? orgCustomerInvoice.CRMProposalID : string.Empty;
                CustomerInvoice["daar_registrationid"] = orgCustomerInvoice.registrationID != string.Empty? orgCustomerInvoice.registrationID : string.Empty;
                CustomerInvoice["daar_registrationno"] = orgCustomerInvoice.registrationNo != string.Empty? orgCustomerInvoice.registrationNo : string.Empty;
                CustomerInvoice["daar_orgid"] = orgCustomerInvoice.orgID != string.Empty? orgCustomerInvoice.orgID : string.Empty;
                CustomerInvoice["daar_salescustomerid"] = orgCustomerInvoice.salesCustomerID != string.Empty ? orgCustomerInvoice.salesCustomerID : string.Empty;
                CustomerInvoice["daar_customernumber"] = orgCustomerInvoice.customerNumber != string.Empty ? orgCustomerInvoice.customerNumber : string.Empty;
               
                CustomerInvoice["daar_invoicenumber"] = orgCustomerInvoice.invoiceNumber != string.Empty? orgCustomerInvoice.invoiceNumber : string.Empty;
                if (orgCustomerInvoice.invoiceDate != default(DateTime).ToString() && orgCustomerInvoice.invoiceDate != null && orgCustomerInvoice.invoiceDate != string.Empty)
                {
                    CustomerInvoice["daar_invoicedate"] = Convert.ToDateTime(orgCustomerInvoice.invoiceDate);
                }
                CustomerInvoice["daar_custtrxtypeid"] = orgCustomerInvoice.CustTrxTypeID != string.Empty? orgCustomerInvoice.CustTrxTypeID : string.Empty;
                CustomerInvoice["daar_transactiontype"] = orgCustomerInvoice.transactionType != string.Empty? orgCustomerInvoice.transactionType : string.Empty;
                CustomerInvoice["daar_currencycode"] = orgCustomerInvoice.currencyCode != string.Empty? orgCustomerInvoice.currencyCode : string.Empty;
                CustomerInvoice["daar_amount"] = new Money(orgCustomerInvoice.amount);
                CustomerInvoice["daar_appliedamount"] = new Money(orgCustomerInvoice.appliedAmount);
                CustomerInvoice["daar_creditedamount"] = new Money(orgCustomerInvoice.creditedAmount);
                CustomerInvoice["daar_remainingamount"] = new Money(orgCustomerInvoice.remainingAmount);
                Guid customerGUID = getCustomerGUID(orgCustomerInvoice.customerNumber);
                if (customerGUID != Guid.Empty)
                {
                    CustomerInvoice["daar_accountid"] = new EntityReference("account", customerGUID);
                }

                UpsertRequest request = new UpsertRequest()
                {
                    Target = CustomerInvoice
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    orgCustomerInvoice.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    orgCustomerInvoice.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();

                //Guid CustomerInvoiceGuid = service.Create(CustomerInvoice);
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgCustomerInvoice;
        }

        [HttpPost]
        [Route("Project")]
        public createProject CreateProject(createProject orgProject)
        {
            #region Try
            try
            {
                Entity Project = new Entity();
                if (!string.IsNullOrEmpty(orgProject.propertyID))
                {
                    Project = new Entity("daar_project", "daar_propertyid", orgProject.propertyID);
                }
                if(orgProject.orgID!=null && orgProject.orgID!=string.Empty)
                {
                    Guid organizationGUID = getOrganizationGUID(orgProject.orgID);
                    if (organizationGUID != Guid.Empty)
                    {
                        Project["daar_orgid"] = new EntityReference("daar_organization", organizationGUID);
                    }
                }               
                Project["daar_name"] = orgProject.propertyName != string.Empty ? orgProject.propertyName : string.Empty;
                Project["daar_propertycode"] = orgProject.propertyCode != string.Empty ? orgProject.propertyCode : string.Empty;
                if (orgProject.propertyStatus != null && orgProject.propertyStatus != string.Empty)
                {
                    Guid propertyStatusGUID = getPropertyStatusGUID(orgProject.propertyStatus);
                    if (propertyStatusGUID != Guid.Empty)
                    {
                        Project["daar_propertystatus"] = new EntityReference("daar_projectstatus", propertyStatusGUID);
                    }
                }
                if (orgProject.country != null && orgProject.country != string.Empty)
                {
                    Guid countryGUID = getCountryGUID(orgProject.country);
                    if (countryGUID != Guid.Empty)
                    {
                        Project["daar_country"] = new EntityReference("daar_country", countryGUID);
                    }
                }
                if (orgProject.activeProperty != null && orgProject.activeProperty != string.Empty)
                {
                    Guid yesOrNoGUID = getYesOrNoGUID(orgProject.activeProperty);
                    if (yesOrNoGUID != Guid.Empty)
                    {
                        Project["daar_activeproperty"] = new EntityReference("daar_yesorno", yesOrNoGUID);
                    }
                }
                UpsertRequest request = new UpsertRequest()
                {
                    Target = Project
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    orgProject.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    orgProject.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgProject;
        }

        [HttpPost]
        [Route("PaymentPlans")]
        public createPaymentPlans CreatePaymentPlans(createPaymentPlans orgPaymentPlan)
        {
            #region Try
            try
            {
                Entity PaymentPlan = new Entity();
                if (!string.IsNullOrEmpty(orgPaymentPlan.paymentPlanID))
                {
                    PaymentPlan = new Entity("daar_paymentplan", "daar_paymentplanno", orgPaymentPlan.paymentPlanID);
                }
                PaymentPlan["daar_propertyid"] = orgPaymentPlan.propertyID != string.Empty ? orgPaymentPlan.propertyID : string.Empty;
                if (orgPaymentPlan.propertyID != null && orgPaymentPlan.propertyID != string.Empty)
                {
                    Guid projectGUID = getProjectGUID(orgPaymentPlan.propertyID);
                    if (projectGUID != Guid.Empty)
                    {
                        PaymentPlan["daar_projectid"] = new EntityReference("daar_project", projectGUID);
                    }
                }
                PaymentPlan["daar_paymentplannumber"] = orgPaymentPlan.paymentPlanNo != string.Empty ? orgPaymentPlan.paymentPlanNo : string.Empty;
                PaymentPlan["daar_name"] = orgPaymentPlan.paymentPlanName != string.Empty ? orgPaymentPlan.paymentPlanName : string.Empty;
                PaymentPlan["daar_orgid"] = orgPaymentPlan.orgID != string.Empty ? orgPaymentPlan.orgID : string.Empty;
                if (orgPaymentPlan.paymentPlanStatus != null && orgPaymentPlan.paymentPlanStatus != string.Empty)
                {
                    Guid yesOrNoGUID = getYesOrNoGUID(orgPaymentPlan.paymentPlanStatus);
                    if (yesOrNoGUID != Guid.Empty)
                    {
                        PaymentPlan["daar_activeplan"] = new EntityReference("daar_yesorno", yesOrNoGUID);
                    }
                }

                UpsertRequest request = new UpsertRequest()
                {
                    Target = PaymentPlan
                };
                UpsertResponse response = (UpsertResponse)service.Execute(request);

                if (response.RecordCreated)
                    orgPaymentPlan.outcome = CRMRESTWebAPI.Models.Outcomes.Created.ToString();
                else
                    orgPaymentPlan.outcome = CRMRESTWebAPI.Models.Outcomes.Updated.ToString();
            }
            #endregion

            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (System.TimeoutException ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<SecurityAccessDeniedException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            catch (FaultException<MessageSecurityException> ex)
            {
                oErrorLog.WriteErrorLog(ex.Message);
                throw new Exception("The application terminated with an error." + ex.Message);
            }
            #endregion

            return orgPaymentPlan;
        }

        private Guid getDefaultBusinessUnitGUID(IOrganizationService service)
        {
            Guid defaultBUGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("businessunitid");
            q1.EntityName = "businessunit";
            FilterExpression fe = new FilterExpression();
            fe.AddCondition(new ConditionExpression("parentbusinessunitid", ConditionOperator.Null));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    defaultBUGuid = new Guid(c["businessunitid"].ToString());

                }
            }
            return defaultBUGuid;
        }
        private Guid getOrganizationGUID(string OrgID)
        {
            Guid organizationGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_org_id");
            q1.EntityName = "daar_organization";
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddCondition(new ConditionExpression("daar_org_id", ConditionOperator.Equal, OrgID));
            fe.AddCondition(new ConditionExpression("daar_org_id", ConditionOperator.NotNull));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    organizationGuid = new Guid(c.Attributes["daar_organizationid"].ToString());
                }
            }
            return organizationGuid;
        }
        private Guid getYesOrNoGUID(string YesOrNo)
        {
            Guid YesOrNoGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_code");
            q1.EntityName = "daar_yesorno";
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddCondition(new ConditionExpression("daar_code", ConditionOperator.Equal, YesOrNo));
            fe.AddCondition(new ConditionExpression("daar_code", ConditionOperator.NotNull));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    YesOrNoGuid = new Guid(c.Attributes["daar_yesornoid"].ToString());
                }
            }
            return YesOrNoGuid;
        }
        private Guid getRegistrationStatusGUID(string proposalStatus)
        {
            Guid registrationStatusGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_code");
            q1.EntityName = "daar_registrationstatus";
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddCondition(new ConditionExpression("daar_code", ConditionOperator.Equal, proposalStatus));
            fe.AddCondition(new ConditionExpression("daar_code", ConditionOperator.NotNull));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    registrationStatusGuid = new Guid(c.Attributes["daar_registrationstatusid"].ToString());
                }
            }
            return registrationStatusGuid;
        }
        private Guid getPropertyStatusGUID(string propertyStatus)
        {
            Guid propertyStatusGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_projectstatuscode");
            q1.EntityName = "daar_projectstatus";
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddCondition(new ConditionExpression("daar_projectstatuscode", ConditionOperator.Equal, propertyStatus));
            fe.AddCondition(new ConditionExpression("daar_projectstatuscode", ConditionOperator.NotNull));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    propertyStatusGuid = new Guid(c.Attributes["daar_projectstatusid"].ToString());
                }
            }
            return propertyStatusGuid;
        }
        private Guid getManagerGUID(string managerEmployeeID)
        {
            Guid managerGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_employeeid");
            q1.EntityName = "systemuser";
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddCondition(new ConditionExpression("daar_employeeid", ConditionOperator.Equal, managerEmployeeID));
            fe.AddCondition(new ConditionExpression("daar_employeeid", ConditionOperator.NotNull));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    managerGuid = new Guid(c.Attributes["systemuserid"].ToString());
                }
            }
            return managerGuid;
        }
        private Guid getCustomerGUID(string customerID)
        {
            Guid customerGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("accountnumber");
            q1.EntityName = "account";
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddCondition(new ConditionExpression("accountnumber", ConditionOperator.Equal, customerID));
            fe.AddCondition(new ConditionExpression("accountnumber", ConditionOperator.NotNull));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    customerGuid = new Guid(c.Attributes["accountid"].ToString());
                }
            }
            return customerGuid;
        }
        private Guid getCountryGUID(string codeorname)
        {
            Guid countryGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_code","daar_name");
            q1.EntityName = "daar_country";
            FilterExpression fe = new FilterExpression(LogicalOperator.Or);
            fe.AddCondition(new ConditionExpression("daar_code", ConditionOperator.Equal, codeorname));
            fe.AddCondition(new ConditionExpression("daar_name", ConditionOperator.Equal,codeorname));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    countryGuid = new Guid(c.Attributes["daar_countryid"].ToString());
                }
            }
            return countryGuid;
        }
        private Guid getPhoneCountryCodeGUID(string codeorname)
        {
            Guid countryGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_name");
            q1.EntityName = "daar_country_phone_code";
            FilterExpression fe = new FilterExpression();
            fe.AddCondition(new ConditionExpression("daar_name", ConditionOperator.Equal, codeorname));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    countryGuid = new Guid(c.Attributes["daar_country_phone_codeid"].ToString());
                }
            }
            return countryGuid;
        }
        private Guid getBrokerGUID(string brokerNumber)
        {
            Guid brokerGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_brokernumber");
            q1.EntityName = "daar_broker";
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddCondition(new ConditionExpression("daar_brokernumber", ConditionOperator.Equal, brokerNumber));
            fe.AddCondition(new ConditionExpression("daar_brokernumber", ConditionOperator.NotNull));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    brokerGuid = new Guid(c.Attributes["daar_brokerid"].ToString());
                }
            }
            return brokerGuid;
        }
        private Guid getProjectGUID(string propertyID)
        {
            Guid projectGuid = new Guid();
            QueryExpression q1 = new QueryExpression();
            q1.ColumnSet = new ColumnSet("daar_propertyid");
            q1.EntityName = "daar_project";
            FilterExpression fe = new FilterExpression(LogicalOperator.And);
            fe.AddCondition(new ConditionExpression("daar_propertyid", ConditionOperator.Equal, propertyID));
            fe.AddCondition(new ConditionExpression("daar_propertyid", ConditionOperator.NotNull));
            q1.Criteria = fe;
            EntityCollection ec = service.RetrieveMultiple(q1);
            if (ec.Entities.Count > 0)
            {
                foreach (Entity c in ec.Entities)
                {
                    projectGuid = new Guid(c.Attributes["daar_projectid"].ToString());
                }
            }
            return projectGuid;
        }
    }
}
