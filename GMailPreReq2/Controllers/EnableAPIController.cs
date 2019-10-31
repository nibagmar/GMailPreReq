using Google.Apis.ServiceManagement.v1;
using Google.Apis.ServiceManagement.v1.Data;
using Google.Apis.Services;
using System;
using System.Web.Http;

namespace GMailPreReq2.Controllers
{
    public class EnableAPIController : ApiController
    {
        [HttpPost]
        public IHttpActionResult EnableAPIs(string projectID)
        {
            try
            {
                var service = new ServiceManagementService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = AppFlowMetadata.UserCredential,
                    ApplicationName = AppFlowMetadata.AppName,
                });

                EnableServiceRequest servReq1 = new EnableServiceRequest() { ConsumerId = "project:" + projectID };
                var enableReq1 = service.Services.Enable(servReq1, "gmail.googleapis.com");
                var res1 = enableReq1.Execute();

                EnableServiceRequest servReq2 = new EnableServiceRequest() { ConsumerId = "project:" + projectID };
                var enableReq2 = service.Services.Enable(servReq2, "calendar-json.googleapis.com");
                var res2 = enableReq2.Execute();

                EnableServiceRequest servReq3 = new EnableServiceRequest() { ConsumerId = "project:" + projectID };
                var enableReq3 = service.Services.Enable(servReq3, "contacts.googleapis.com");
                var res3 = enableReq3.Execute();
            }
            catch (Exception)
            {

            }

            return Ok();
        }
    }
}
