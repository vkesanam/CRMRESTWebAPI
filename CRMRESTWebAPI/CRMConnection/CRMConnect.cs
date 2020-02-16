
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Web;

namespace CRMRESTWebAPI.CRMConnection
{
    public class CRMConnect
    {
        public static IOrganizationService GetProcessIFD(string uname, string pwd, string organization)
        {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            ClientCredentials credentials = new ClientCredentials();
            //credentials.Windows.ClientCredential = new NetworkCredential(uname, pwd, dmain);
            credentials.UserName.UserName = uname;
            credentials.UserName.Password = pwd;
            Uri OrganizationUri = new Uri(organization);
            Uri HomeRealUri = null;
            IOrganizationService service = null;
            try
            {
                using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, HomeRealUri, credentials, null))
                {
                    serviceProxy.Timeout = new TimeSpan(0, 10, 0);
                    serviceProxy.EnableProxyTypes();
                    service = (IOrganizationService)serviceProxy;
                }
            }
            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                throw new Exception("The application terminated with an error.");
            }
            catch (System.TimeoutException ex)
            {
                throw new Exception("The application terminated with an error.");
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                throw new Exception("The application terminated with an error.");
            }
            catch (Exception ex)
            {
                throw new Exception("The application terminated with an error.");
            }
            #endregion
            return service;
        }

        public static IOrganizationService GetProcessOnPremise(string uname, string pwd,string domain, string organization)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            ClientCredentials credentials = new ClientCredentials();
            credentials.Windows.ClientCredential = new NetworkCredential(uname, pwd, domain);
            //credentials.UserName.UserName = uname;
            //credentials.UserName.Password = pwd;
            Uri OrganizationUri = new Uri(organization);
            Uri HomeRealUri = null;
            IOrganizationService service = null;
            try
            {
                using (OrganizationServiceProxy serviceProxy = new OrganizationServiceProxy(OrganizationUri, HomeRealUri, credentials, null))
                {
                    serviceProxy.Timeout = new TimeSpan(0, 10, 0);
                    serviceProxy.EnableProxyTypes();
                    service = (IOrganizationService)serviceProxy;
                }
            }
            #region Exception
            catch (FaultException<Microsoft.Xrm.Sdk.DiscoveryServiceFault> ex)
            {
                throw new Exception("The application terminated with an error.");
            }
            catch (System.TimeoutException ex)
            {
                throw new Exception("The application terminated with an error.");
            }
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault> ex)
            {
                throw new Exception("The application terminated with an error.");
            }
            catch (Exception ex)
            {
                throw new Exception("The application terminated with an error.");
            }
            #endregion
            return service;
        }
    }
}