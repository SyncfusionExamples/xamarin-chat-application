using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.AccessToken;

namespace AndroidChatApplication.Helpers
{
    public static class TwilioHelper
    {
        public static async Task<string> GetTokenAsync()
        {
            var deviceId = CrossDeviceInfo.Current.Id;
            const string twilioAccountSid = "AC3b4d9037a04f31c8e64fcd02ff2c6a9d";
            const string twilioApiKey = "SK797d3f329ea89c8d0a5eb7a27d92274d";
            const string twilioApiSecret = "B8mbBv19bFxRYZFv75dUFbmSsINeOd1x";

            var identity = "Syncfusion";
            var applicationName = "AndroidChatApplication";
            var device = deviceId;

            var endpointId = string.Format("{0}:{1}:{2}", applicationName, identity, device);

            // Create an Chat grant for this token
            var grant = new ChatGrant
            {
                EndpointId = endpointId,
                ServiceSid = "ISc9730136ea654f0bad6cf664c659e753"
            };


            var grants = new HashSet<IGrant>
                {
                    { grant }
                };

            // Create an Access Token generator
            var token = new Token(
                twilioAccountSid,
                twilioApiKey,
                twilioApiSecret,
                identity,
                grants: grants);

            return token.ToJwt();
        }
    }
}
