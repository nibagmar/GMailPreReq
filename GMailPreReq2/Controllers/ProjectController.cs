using System.Web.Http;

using Google.Apis.CloudResourceManager.v1;
using Google.Apis.Services;

using Data = Google.Apis.CloudResourceManager.v1.Data;

namespace GMailPreReq2.Controllers
{
    public class ProjectController : ApiController
    {
        [HttpPost]
        public IHttpActionResult CreateProject(string projectID, string projectName)
        {
            var service = new CloudResourceManagerService(new BaseClientService.Initializer
            {
                HttpClientInitializer = AppFlowMetadata.UserCredential,
                ApplicationName = AppFlowMetadata.AppName,
            });

            Data.Project requestBody = new Data.Project() { ProjectId = projectID, Name = projectName };
            ProjectsResource.CreateRequest request = service.Projects.Create(requestBody);

            var response = request.Execute();

            return Ok();
        }
    }
}
