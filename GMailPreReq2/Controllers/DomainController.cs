using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace GMailPreReq2.Controllers
{
    public class DomainController : ApiController
    {
        [HttpPost]
        public IHttpActionResult AddDomain(string newdomainprefix)
        {
            try
            {
                string domainName = "blackteatest.in";
                string newDomainName = newdomainprefix + "." + domainName;
                var service = new DirectoryService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = AppFlowMetadata.UserCredential,
                    ApplicationName = AppFlowMetadata.AppName,
                });

                UsersResource.ListRequest listrequest = service.Users.List();
                listrequest.Domain = domainName;
                listrequest.MaxResults = 10;

                IList<User> users = listrequest.Execute().UsersValue;
                string custID = string.Empty;
                if (users != null && users.Count > 0)
                {
                    custID = users[0].CustomerId;
                }

                Domains domain = new Domains() { DomainName = newDomainName };
                DomainsResource.InsertRequest insReq = service.Domains.Insert(domain, custID);

                var insResp = insReq.Execute();
            }
            catch (Exception)
            {

            }

            return Ok();
        }
    }
}
