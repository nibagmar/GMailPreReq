using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.CloudResourceManager.v1;
using Google.Apis.Iam.v1;
using Google.Apis.Iam.v1.Data;
using Google.Apis.ServiceManagement.v1;
using Google.Apis.ServiceManagement.v1.Data;
using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Services;
using Data = Google.Apis.CloudResourceManager.v1.Data;
using System.Collections.Generic;
using Google.Apis.Auth.OAuth2;

namespace GMailPreReq2.Controllers
{
    public class HomeController : Controller
    {
        const string AppName = "GMailPreReq2";

        private bool AddDomain(UserCredential userCredential, string domainName, string newDomainName)
        {
            try
            {
                var service = new DirectoryService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = userCredential,
                    ApplicationName = AppName,
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
            catch(Exception)
            {

            }

            return true;
        }

        private bool CreateProject(UserCredential userCredential, string projectID, string projectName)
        {
            try
            {
                var service = new CloudResourceManagerService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = userCredential,
                    ApplicationName = AppName,
                });

                Data.Project requestBody = new Data.Project() { ProjectId = projectID, Name = projectName };
                Google.Apis.CloudResourceManager.v1.ProjectsResource.CreateRequest request = service.Projects.Create(requestBody);

                var response = request.Execute();
            }
            catch (Exception)
            {

            }

            return true;
        }

        private bool CreateServiceAccountAndKey(UserCredential userCredential, string projectID, string accountID, string displayName)
        {
            try
            {
                var service = new IamService(new IamService.Initializer
                {
                    HttpClientInitializer = userCredential,
                    ApplicationName = AppName,
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
            }
            catch(Exception)
            {

            }

            return true;
        }

        private bool EnableAPIs(UserCredential userCredential, string projectID)
        {
            try
            {
                var service = new ServiceManagementService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = userCredential,
                    ApplicationName = AppName,
                });

                EnableServiceRequest servReq = new EnableServiceRequest() { ConsumerId = "project:" + projectID };
                var enableReq = service.Services.Enable(servReq, "gmail.googleapis.com");

                var res = enableReq.Execute();
            }
            catch(Exception)
            {

            }

            return true;
        }

        public async Task<ActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await new AuthorizationCodeMvcApp(this, new AppFlowMetadata()).
                AuthorizeAsync(cancellationToken);

            if (result.Credential != null)
            {
                AppFlowMetadata.UserCredential = result.Credential;

                // Add a domain
                /*this.AddDomain(result.Credential, "blackteatest.in", "newdomain.blackteatest.in");

                // Create a new project
                var projectId = "projectid";
                this.CreateProject(result.Credential, projectId, "projectname");

                Thread.Sleep(60000);

                // Create a new SA   
                // Create service account key
                this.CreateServiceAccountAndKey(result.Credential, projectId, "serviceaccountkey", "SADisplayName");

                // Enable APIs for a project
                this.EnableAPIs(result.Credential, projectId);*/


                return View();
            }
            else
            {
                return new RedirectResult(result.RedirectUri);
            }
        }
    }
}