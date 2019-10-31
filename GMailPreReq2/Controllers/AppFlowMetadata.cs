using System;
using System.Web.Mvc;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Mvc;
using Google.Apis.Drive.v2;
using Google.Apis.Iam.v1;
using Google.Apis.Util.Store;

namespace GMailPreReq2.Controllers
{
    public class AppFlowMetadata : FlowMetadata
    {
        private static readonly IAuthorizationCodeFlow flow =
            new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = new ClientSecrets
                {
                    ClientId = "995772391856-b044doin3nudcv5lsvjib1th00c1o9dg.apps.googleusercontent.com",
                    ClientSecret = "BR-d1FgkWdJIGA4U540ifREx"
                },
                Scopes = new[] { DriveService.Scope.Drive, IamService.Scope.CloudPlatform, "https://www.googleapis.com/auth/admin.directory.user.readonly", "https://www.googleapis.com/auth/admin.directory.domain" },
                DataStore = new FileDataStore("Drive.Api.Auth.Store")
            });

        public static UserCredential UserCredential { get; set; }

        public const string AppName = "GMailPreReq2";

        public override string GetUserId(Controller controller)
        {
            // In this sample we use the session to store the user identifiers.
            // That's not the best practice, because you should have a logic to identify
            // a user. You might want to use "OpenID Connect".
            // You can read more about the protocol in the following link:
            // https://developers.google.com/accounts/docs/OAuth2Login.
            var user = controller.Session["user"];
            if (user == null)
            {
                user = Guid.NewGuid();
                controller.Session["user"] = user;
            }
            return user.ToString();

        }

        public override IAuthorizationCodeFlow Flow
        {
            get { return flow; }
        }
    }
}