using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using WebApi.Common;

namespace WebApi.Services.Common
{
    public class FirebaseNotifier
    {
        private readonly IConfiguration _configs;
        protected readonly ILogger<FirebaseNotifier> _logger;
        public FirebaseNotifier(IConfiguration configs,
            ILogger<FirebaseNotifier> logger)
        {
            var path = configs.GetValue<string>(AppConstants.FirebaseKeyFilePath);
            if (string.IsNullOrEmpty(path))
            {
                logger.LogWarning("Firebase key file path is not configured.");
                return;
            }
            //if(FirebaseApp.DefaultInstance is not null)
            //{
            //}
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(configs.GetValue<string>(AppConstants.FirebaseKeyFilePath)),
            });
            _configs = configs;
            _logger = logger;
        }

        public Task<string> NotifyAsync(string title, string body)
        {
            var message = new Message()
            {
                Topic = "all",
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            // Send a message to the device corresponding to the provided
            // registration token.
            return FirebaseMessaging.DefaultInstance.SendAsync(message);
        }
    }
}
