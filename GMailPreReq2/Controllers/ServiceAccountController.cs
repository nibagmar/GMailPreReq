using Google.Apis.Iam.v1;
using Google.Apis.Iam.v1.Data;
using System;
using System.Web.Http;

namespace GMailPreReq2.Controllers
{
    public class ServiceAccountController : ApiController
    {
        [HttpPost]
        public IHttpActionResult CreateServiceAccount(string projectID, string accountID, string displayName)
        {
            try
            {
                var service = new IamService(new IamService.Initializer
                {
                    HttpClientInitializer = AppFlowMetadata.UserCredential,
                    ApplicationName = AppFlowMetadata.AppName,
                });

                var request2 = new CreateServiceAccountRequest
                {
                    AccountId = accountID,
                    ServiceAccount = new ServiceAccount
                    {
                        DisplayName = displayName
                    }
                };

                var serviceAccount = service.Projects.ServiceAccounts.Create(
                    request2, "projects/" + projectID).Execute();

                var email = serviceAccount.Email;

                var key = service.Projects.ServiceAccounts.Keys.Create(
                    new CreateServiceAccountKeyRequest(),
                    "projects/-/serviceAccounts/" + email)
                    .Execute();

                return Ok(serviceAccount.UniqueId);
            }
            catch (Exception)
            {

            }

            return Ok();
        }
    }
}
